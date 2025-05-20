using System.Collections;
using Mgr;
using UI;
using XYZFrameWork;

namespace AttachMachine
{
    public class BetUIState : BaseGameUIState
    {
        public override string      StateID => StateIDStr;
        public const string         StateIDStr = "BetUIState";
        private IBetUI              _betUI;
        public override void   OnCreate(IMachineMaster sceneUI)
        {
            if (sceneUI is IBetUI ui)
            {
                _betUI = ui;
            }
            _betUI.BetUI.Init();
            NotifyMgr.RegisterNotify(NotifyDefine.BET_CHIP,OnBetChip);
        }

        public override IEnumerator OnEnterAsync(object payload)
        {
            _betUI.BetUI.ShowBetUI();
            yield break;
        }

        public override IEnumerator OnExitAsync(object payload)
        {
            _betUI.BetUI.Hide();
            yield return XAttachMachine.EnterState(SkillUIState.StateIDStr);
        }

        public override void OnUpdate(float deltaTime)
        {

        }

        private void OnBetChip(NotifyMsg obj)
        {
            if (obj.Param is NormalParam param)
            {
                DataMgr.Instance.BetChip(param.IntValue);
                XAttachMachine.ExitState(StateIDStr);
            }
        }
    }

    public interface IBetUI:IBaseAttachUI
    {
        public BetUI BetUI { get; }
    }
}