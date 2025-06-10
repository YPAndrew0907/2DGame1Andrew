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
            LevelMgr.Instance.SetCurrentLevel(LevelMgr.Instance.CurrentLevel);
            _homeUIState.HomeUI.ShowUI();
            yield break;
        }

        public override IEnumerator OnExitAsync(object payload)
        {
            _homeUIState.HomeUI.HideUI();
            if (payload.Equals(SkillUpgradeUIState.StateIDStr))
            {
                yield return XAttachMachine.EnterState(SkillUpgradeUIState.StateIDStr);
            }
            else if (payload.Equals(BetUIState.StateIDStr))
            {
                // 初始化 要牌玩家
                GameSessionMgr.Instance.NextPlayerAskCard();

                XAttachMachine.ActiveAll();
                
                GameSessionMgr.Instance.InitSession(PlayerProfileMgr.Instance.Money, LevelMgr.Instance.BossChip,
                    SkillMgr.Instance.UnLockSkillList(),LevelMgr.Instance.LevelBossSkill);
                yield return XAttachMachine.EnterState(BetUIState.StateIDStr);
            }
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