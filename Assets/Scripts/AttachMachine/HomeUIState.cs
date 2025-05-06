using System.Collections;
using UI;

namespace AttachMachine
{
    public class HomeUIState : BaseGameUIState
    {
        public override string StateID => StateIDStr;
        public const    string StateIDStr = "HomeUIState";
        private         IHomeUIState  _homeUIState;
        public override void        OnCreate(IMachineMaster sceneUI)
        {
            if (sceneUI is IHomeUIState ui)
            {
                _homeUIState = ui;
            }
        }

        public override IEnumerator OnEnterAsync(object payload)
        {
            throw new System.NotImplementedException();
        }

        public override IEnumerator OnExitAsync(object payload)
        {
            throw new System.NotImplementedException();
        }

        public override void        OnUpdate(float deltaTime)
        {
        }
    }

    public interface IHomeUIState : IBaseAttachUI
    {
        HomeUI HomeUI { get; }
    }
}