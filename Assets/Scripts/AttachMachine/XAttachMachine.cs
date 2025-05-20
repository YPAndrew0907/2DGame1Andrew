using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mgr;
using XYZFrameWork;
using XYZFrameWork.Base;

namespace AttachMachine
{
    public class XAttachMachine: BaseAutoMonoSingle<XAttachMachine>
    {
        private static IMachineMaster _owner;
        
        private static readonly Dictionary<string, IAttachState> States = new();
        private static readonly Dictionary<string, IAttachState> CurState = new();
        
        public static void SetMaster(IMachineMaster owner)
        {
            _owner = owner;
        }

        /// <summary>
        /// 注册节点
        /// </summary>
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
            if (States.TryGetValue(stateId, out IAttachState attachState))
            {
                return attachState;
            }

            Debug.LogWarning($"状态 {stateId} 不存在，获取失败");
            return null;
        }

        /// <summary>
        /// 启动状态机
        /// </summary>
        public static void StartMachine(string initialStateID)
        {
            CoroutineMgr.Instance.StartCoroutine(EnterState(initialStateID));
        }

        public static IEnumerator EnterState(string stateId, object payload = null)
        {
            if (States.TryGetValue(stateId, out var initialState))
            {
                Debug.Log($"【进状态】 :{stateId}");
                CurState.Add(stateId, initialState);
                yield return initialState.OnEnterAsync(payload);
                NotifyMgr.SendEvent(NotifyDefine.X_ATTACH_MACHINE_ENTER_STATE, stateId);
                yield break;
            }

            Debug.LogError($"【未注册】： {stateId}");
        }

        public static IEnumerator SwitchState(string exitState, string enterState, object payload = null)
        {
            yield return  ExitStateCor(exitState);
            Debug.Log($"【切状态】： {exitState}-->{enterState}");
            yield return EnterState(enterState,payload);
        }

        public static void ExitState(string stateId, object payload = null)
        {
            CoroutineMgr.Instance.StartCoroutine(ExitStateCor(stateId, payload));
        }
        
        public static IEnumerator ExitStateCor(string stateId, object payload = null)
        {
            if (States.TryGetValue(stateId, out var initialState))
            {
                CurState.Remove(stateId);
                yield return initialState.OnExitAsync(payload);
                NotifyMgr.SendEvent(NotifyDefine.X_ATTACH_MACHINE_EXIT_STATE, stateId);
                yield break;
            }
            
            Debug.LogError($"【未注册】： {stateId}");
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
            foreach (var (key,node) in States)
            {
                node.OnUpdate(Time.deltaTime);
            }
        }
    }
}