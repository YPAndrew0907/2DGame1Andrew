﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
namespace AttachMachine
{
    public class XAttachMachine
    {
        private  MonoBehaviour _owner;
        
        // 运行时数据
        private          Dictionary<string, IAttachNode> _nodes = new Dictionary<string, IAttachNode>();
        private          Coroutine                       _activeCoroutine;
        private          CancellationTokenSource         _cts;
        private readonly List<string>                    _stateHistory = new List<string>(20);

        // 调试配置
        [Header("Debug Settings")]
        private bool _enableDebugLog = true; 
        private float _minTransitionInterval = 0.1f;
        private float _lastTransitionTime;

        // 公开属性
        public IReadOnlyList<string> StateHistory => _stateHistory.AsReadOnly();

        public XAttachMachine(MonoBehaviour owner)
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

            _nodes.Add(stateID, state);
            LogDebug($"注册状态：{stateID}");
        }

        /// <summary>
        /// 启动状态机
        /// </summary>
        public void StartMachine(string initialStateID)
        {
            if (!_nodes.ContainsKey(initialStateID))
            {
                Debug.LogError($"依附节点 {initialStateID} 未注册");
            }
            
        }
        
        private void Update()
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

        private void LogDebug(string message)
        {
            if (_enableDebugLog)
                Debug.Log($"[XStateMachine] {message}");
        }
    }
}