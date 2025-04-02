using System.Collections;
using System.Collections.Generic;
using System.Threading;

/// <summary>
/// 增强型状态节点接口
/// </summary>
public interface IStateNode
{
    /// <summary>
    /// 状态唯一标识（建议使用枚举或字符串常量）
    /// </summary>
    string StateID { get; }

    void OnCreate(IMachineMaster machine);
    /// <summary>
    /// 异步进入状态
    /// </summary>
    IEnumerator OnEnterAsync(StateTransitionContext context, CancellationToken ct);

    /// <summary>
    /// 异步退出状态
    /// </summary>
    IEnumerator OnExitAsync(StateTransitionContext context, CancellationToken ct);

    /// <summary>
    /// 状态更新
    /// </summary>
    void OnUpdate(float deltaTime);

    /// <summary>
    /// 获取所有合法转移路径
    /// </summary>
    IEnumerable<StateTransition> GetTransitions();
}

public interface IMachineMaster
{
    
}