using System.Collections;
using UI;

namespace AttachMachine
{
    // 要牌状态
    public class AskCardUIState : BaseGameUIState
    {
        public override string      StateID => StateIDStr;
        public const    string      StateIDStr = "AskCardUIState";
        private IAskCardUIState _askCardUI;
        public override void        OnCreate(IMachineMaster sceneUI)
        {
            if (sceneUI is IAskCardUIState askCardUI)
            {
                _askCardUI = askCardUI;
            }
        }

        public override IEnumerator OnEnterAsync(object payload)
        {
            yield break;
            
        }

        public override IEnumerator OnExitAsync(object payload)
        {
            yield break;
        }

        public override void        OnUpdate(float deltaTime)
        {
            
        }
    }

    public interface IAskCardUIState : IBaseAttachUI
    {
        public AskCardUI AskCardUI { get; }
    }
}