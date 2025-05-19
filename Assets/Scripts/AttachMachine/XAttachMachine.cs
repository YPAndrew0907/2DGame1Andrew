using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Mgr;
using UI;
using Unity.VisualScripting;
using XYZFrameWork;

namespace AttachMachine
{
    public class XAttachMachine
    {
        private  IMachineMaster _owner;
        
        // 运行时数据
        private          Dictionary<string, IAttachNode> _nodes = new Dictionary<string, IAttachNode>();
        private          Coroutine                       _activeCoroutine;
        private          CancellationTokenSource         _cts;
        private readonly List<string>                    _stateHistory = new List<string>(20);

        // 调试配置
        [Header("Debug Settings")]
        private bool _enableDebugLog = true; 

        // 公开属性
        public IReadOnlyList<string> StateHistory => _stateHistory.AsReadOnly();

        public XAttachMachine(IMachineMaster owner)
        {
            _owner = owner;
        }

        /// <summary>
        /// 注册节点
        /// </summary>
        public void RegisterState(IAttachNode state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            string stateID = state.StateID;
            if (_nodes.ContainsKey(stateID))
            {
                Debug.LogWarning($"状态 {stateID} 已存在，注册失败");
                return;
            }

            state.OnCreate(_owner);
            _nodes.Add(stateID, state);
        }

        /// <summary>
        /// 启动状态机
        /// </summary>
        public void StartMachine(string initialStateID)
        { 
           CoroutineMgr.Instance.StartCoroutine( EnterState(initialStateID));
        }

        public IEnumerator EnterState(string stateId, object payload = null)
        {
            if (_nodes.TryGetValue(stateId, out var initialState))
            {
                Debug.Log($"【进状态】 :{stateId}");
                yield return initialState.OnEnterAsync(payload);
                NotifyMgr.Instance.SendEvent(NotifyDefine.X_ATTACH_MACHINE_ENTER_STATE, stateId);
                yield break;
            }

            Debug.LogError($"【未注册】： {stateId}");
        }

        public IEnumerator SwitchState(string exitState, string enterState, object payload = null)
        {
            yield return  ExitState(exitState);
            Debug.Log($"【切状态】： {exitState}-->{enterState}");
            yield return EnterState(enterState,payload);
        }
        
        public IEnumerator ExitState(string stateId)
        {
            if (_nodes.TryGetValue(stateId, out var initialState))
            {
                CoroutineMgr.Instance.StartCoroutine(initialState.OnExitAsync(null));
                NotifyMgr.Instance.SendEvent(NotifyDefine.X_ATTACH_MACHINE_EXIT_STATE, stateId);
                yield break;
            }
            
            Debug.LogError($"【未注册】： {stateId}");
        }

        public void ActiveAll()
        {
            foreach (var node in _nodes)
            {
                node.Value.OnActive();
                NotifyMgr.Instance.SendEvent(NotifyDefine.X_ATTACH_MACHINE_ACTIVE_STATE, node.Key);
            }
        }

        public void InActiveAll()
        {
            foreach (var node in _nodes)
            {
                node.Value.OnInActive();
                NotifyMgr.Instance.SendEvent(NotifyDefine.X_ATTACH_MACHINE_ACTIVE_STATE, node.Key);
            }
        }

        public void Update()
        {
            foreach (var (key,node) in _nodes)
            {
                node.OnUpdate(Time.deltaTime);
            }
        }

        private void OnDestroy()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _owner.StopAllCoroutines();
        }
    }
    public interface IMachineMaster
    {
        void StopAllCoroutines();
    }
}