using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;


    /// <summary>
    /// 状态转移上下文
    /// </summary>
    public class StateTransitionContext
    {
        /// <summary>
        /// 来源状态ID
        /// </summary>
        public string FromState { get; set; }

        /// <summary>
        /// 目标状态ID
        /// </summary>
        public string ToState { get; set; }

        /// <summary>
        /// 自定义传递数据
        /// </summary>
        public object Payload { get; set; }
    }

    /// <summary>
    /// 状态转移描述
    /// </summary>
    public struct StateTransition
    {
        /// <summary>
        /// 目标状态ID
        /// </summary>
        public string TargetStateID;

        /// <summary>
        /// 转移条件检测方法
        /// </summary>
        public Func<bool> ConditionChecker;

        /// <summary>
        /// 转移优先级（数值越大优先级越高）
        /// </summary>
        public int Priority;
    }

    #region 状态机核心实现

    public class XStateMachine
    {
        private  MonoBehaviour _owner;
        
        // 运行时数据
        private Dictionary<string, IStateNode> _states = new Dictionary<string, IStateNode>();
        private IStateNode _currentState;
        private Coroutine _activeCoroutine;
        private CancellationTokenSource _cts;
        private readonly List<string> _stateHistory = new List<string>(20);

        // 调试配置
        [Header("Debug Settings")]
        private bool _enableDebugLog = true; 
        private float _minTransitionInterval = 0.1f;
        private float _lastTransitionTime;

        // 公开属性
        public string CurrentStateID => _currentState?.StateID ?? "NONE";
        public IReadOnlyList<string> StateHistory => _stateHistory.AsReadOnly();

        public XStateMachine(MonoBehaviour owner)
        {
            _owner = owner;
        }

        /// <summary>
        /// 注册状态节点
        /// </summary>
        public void RegisterState(IStateNode state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            string stateID = state.StateID;
            if (_states.ContainsKey(stateID))
            {
                Debug.LogWarning($"状态 {stateID} 已存在，注册失败");
                return;
            }

            _states.Add(stateID, state);
            LogDebug($"注册状态：{stateID}");
        }

        /// <summary>
        /// 启动状态机
        /// </summary>
        public void StartMachine(string initialStateID)
        {
            if (!_states.ContainsKey(initialStateID))
            {
                Debug.LogError($"初始状态 {initialStateID} 未注册");
                return;
            }

            _ = _owner.StartCoroutine(TransitionRoutine(initialStateID, null));
        }

        /// <summary>
        /// 请求状态转移
        /// </summary>
        public void RequestTransition(string targetStateID, object payload = null)
        {
            if (string.IsNullOrEmpty(targetStateID))
            {
                Debug.LogError("无效的目标状态ID");
                return;
            }
            
            if (_activeCoroutine != null)
            {
                Debug.LogWarning($"正在从 {CurrentStateID} 向 {targetStateID} 转移中");
                return;
            }

            _activeCoroutine = _owner.StartCoroutine(TransitionRoutine(targetStateID, payload));
        }

        private IEnumerator TransitionRoutine(string targetStateID, object payload)
        {
            // 防止高频切换
            if (Time.time < _lastTransitionTime + _minTransitionInterval)
            {
                Debug.LogWarning($"状态切换过于频繁，冷却时间：{_minTransitionInterval}秒");
                yield return new WaitForSeconds(_minTransitionInterval);
            }

            // 取消之前的异步操作
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            // 创建上下文
            var context = new StateTransitionContext
            {
                FromState = _currentState?.StateID,
                ToState = targetStateID,
                Payload = payload
            };

            // 退出当前状态
            if (_currentState != null)
            {
                LogDebug($"退出状态：{_currentState.StateID}");
                yield return _currentState.OnExitAsync(context, _cts.Token);
            }

            // 进入新状态
            if (_states.TryGetValue(targetStateID, out var newState))
            {
                // 更新历史记录
                _stateHistory.Add(targetStateID);
                if (_stateHistory.Count > 20)
                    _stateHistory.RemoveAt(0);

                // 执行进入逻辑
                LogDebug($"进入状态：{targetStateID}");
                _currentState = newState;
                yield return newState.OnEnterAsync(context, _cts.Token);
            }
            else
            {
                Debug.LogError($"未注册的状态：{targetStateID}");
            }

            // 清理资源
            _activeCoroutine = null;
            _lastTransitionTime = Time.time;
        }

        private void Update()
        {
            try
            {
                _currentState?.OnUpdate(Time.deltaTime);
            }
            catch (Exception e)
            {
                Debug.LogError($"状态更新异常：{e}");
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
    #endregion