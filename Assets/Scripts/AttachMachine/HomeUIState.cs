using System.Collections;
using Mgr;
using UI;
using XYZFrameWork;

namespace AttachMachine
{
    public class HomeUIState : BaseGameUIState
    {
        public override string StateID => StateIDStr;
        public const    string StateIDStr = "HomeUIState";
        private         IHomeUIState  _homeUIState;

        public override void OnCreate(IMachineMaster sceneUI)
        {
            if (sceneUI is IHomeUIState ui)
            {
                _homeUIState = ui;
                _homeUIState.HomeUI.Init();
            }
        }

        public override IEnumerator OnEnterAsync(object payload)
        {
            DataMgr.Instance.LoadCurLevelInfo();
            _homeUIState.HomeUI.ShowUI();
            yield break;
        }

        public override IEnumerator OnExitAsync(object payload)
        {
            _homeUIState.HomeUI.HideUI();
            yield break;
        }

        public override void OnUpdate(float deltaTime)
        {
        }
    }

    public interface IHomeUIState : IBaseAttachUI
    {
        HomeUI HomeUI { get; }
    }
}