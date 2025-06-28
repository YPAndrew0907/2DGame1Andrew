using System;
using System.Collections;
using System.Collections.Generic;
using Mgr;
using UnityEngine;
using XYZFrameWork;
using XYZFrameWork.Base;

namespace AttachMachine
{
    public class XAttachMachine : BaseAutoMonoSingle<XAttachMachine>
    {
        public static readonly object ExitNullObject = new DateTime();
        private static IMachineMaster _owner;

        private static readonly Dictionary<string, IAttachState> States = new();
        private static IAttachState _currentState;
        private static Coroutine _currentCoroutine;
        private static bool _isSwitching = false; // 防止并发切换

        public static void SetMaster(IMachineMaster owner) => _owner = owner;

        public static void RegisterState(IAttachState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            string stateID = state.StateID;
            if (States.ContainsKey(stateID))
            {
                Debug.LogWarning($"状态 {stateID} 已存在，注册失败");
                return;
            }
            state.OnCreate(_owner);
            States.Add(stateID, state);
        }

        public static IAttachState GetState(string stateId)
        {
            if (States.TryGetValue(stateId, out var attachState))
                return attachState;
            Debug.LogWarning($"状态 {stateId} 不存在，获取失败");
            return null;
        }

        /// <summary> 启动状态机，支持携带参数 </summary>
        public static void StartMachine(string initialStateID, object payload = null)
        {
            SwitchState(null, initialStateID, payload);
        }

        public static void SwitchState(string exitState, string enterState, object payload = null)
        {
            if (_isSwitching)
            {
                Debug.LogWarning("状态切换中，忽略新切换请求");
                return;
            }
            _isSwitching = true;
            CoroutineMgr.Instance.StartCoroutine(SwitchStateCor(exitState, enterState, payload));
        }

        public static IEnumerator SwitchStateCor(string exitState, string enterState, object payload = null)
        {
            // // 1. 停掉当前状态主协程
            // if (_currentCoroutine != null)
            // {
            //     CoroutineMgr.Instance.StopCoroutine(_currentCoroutine);
            //     _currentCoroutine = null;
            // }
            // 2. 通知当前状态退出
            if (_currentState != null)
            {
                yield return RequestExitCurrentStateCor(exitState == null ? ExitNullObject: payload);
                NotifyMgr.SendEvent(NotifyDefine.X_ATTACH_MACHINE_EXIT_STATE, _currentState.StateID);
            }

            // 3. 切换到新状态
            if (!string.IsNullOrEmpty(enterState) && States.TryGetValue(enterState, out var newState))
            {
                Debug.Log($"【切状态】：{_currentState?.StateID ?? "null"} --> {enterState}");
                _currentState = newState;
                _currentCoroutine = CoroutineMgr.Instance.StartCoroutine(EnterStateCor(_currentState, payload));
            }
            else
            {
                Debug.LogError($"【未注册】： {enterState}");
                _currentState = null;
                _currentCoroutine = null;
            }
            _isSwitching = false;
        }

        private static IEnumerator EnterStateCor(IAttachState state, object payload)
        {
            state.isEntered = true;
            Debug.Log($"【进状态】 :{state.StateID}");
            yield return state.OnEnterAsync(payload);
            NotifyMgr.SendEvent(NotifyDefine.X_ATTACH_MACHINE_ENTER_STATE, state.StateID);
        }
   
        public static void RequestExitCurrentState(object payload = null)
        {
            if (_currentState != null)
            {
                CoroutineMgr.Instance.StartCoroutine(RequestExitCurrentStateCor(payload));
            }
        }

        private static IEnumerator RequestExitCurrentStateCor(object payload)
        {
            // if (_currentCoroutine != null)
            // {
            //     CoroutineMgr.Instance.StopCoroutine(_currentCoroutine);
            //     _currentCoroutine = null;
            // }
            if (_currentState != null)
            {
                _currentState.isEntered = false;
                yield return _currentState.OnExitAsync(payload);
                NotifyMgr.SendEvent(NotifyDefine.X_ATTACH_MACHINE_EXIT_STATE, _currentState.StateID);
            }
        }
        
        public static void ActiveAll()
        {
            foreach (var node in States)
            {
                node.Value.OnActive();
                NotifyMgr.SendEvent(NotifyDefine.X_ATTACH_MACHINE_ACTIVE_STATE, node.Key);
            }
        }

        public static void InActiveAll()
        {
            foreach (var node in States)
            {
                node.Value.OnInActive();
                NotifyMgr.SendEvent(NotifyDefine.X_ATTACH_MACHINE_ACTIVE_STATE, node.Key);
            }
        }

        public void Update()
        {
            _currentState?.OnUpdate(Time.deltaTime);
        }
    }
    public class ExitPayload
    {
        public string NextStateId;
    }

}
