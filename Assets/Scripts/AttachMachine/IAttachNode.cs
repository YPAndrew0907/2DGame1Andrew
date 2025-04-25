using System.Collections;
using System.Threading;
using UI;

namespace AttachMachine
{

    /// <summary>
    /// 增强型状态节点接口
    /// </summary>
    public interface IAttachNode
    {
        /// <summary>
        /// 状态唯一标识（建议使用枚举或字符串常量）
        /// </summary>
        string StateID { get; }

        void OnCreate(GameSceneAiui sceneAiui);
        /// <summary>
        /// 异步进入状态
        /// </summary>
        IEnumerator OnEnterAsync(object payload);

        /// <summary>
        /// 异步退出状态
        /// </summary>
        IEnumerator OnExitAsync(object payload);

        /// <summary>
        /// 状态更新
        /// </summary>
        public void OnUpdate(float deltaTime);
    }
    public interface IMachineMaster
    {
    
    }
}