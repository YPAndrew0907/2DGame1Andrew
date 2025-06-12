using System.Collections;
using Mgr;
using Obj;
using UI;
using Unity.Mathematics;
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
            var originBet = math.clamp(GameSessionMgr.Instance.CurrentPlayerBet, LevelMgr.Instance.CurMinBetChip,
                LevelMgr.Instance.CurMaxBetChip); 
            _betUI.BetUI.ShowBetUI( originBet,
                LevelMgr.Instance.TableLevel, LevelMgr.Instance.CurMinBetChip, LevelMgr.Instance.CurMaxBetChip, 
                GameSessionMgr.Instance.PlayerChips);
            _betUI.LevelInfoUI.ShowUI(LevelMgr.Instance.CurrentLevel,GameSessionMgr.Instance.PlayerChips, GameSessionMgr.Instance.CurrentPlayerBet);
            yield break;
        }

        public override IEnumerator OnExitAsync(object payload)
        {
            yield return XAttachMachine.EnterState(SkillUIState.StateIDStr);
        }

        public override void OnUpdate(float deltaTime)
        {

        }

        private void OnBetChip(NotifyMsg obj)
        {
            if (obj.Param is NormalParam param)
            {
                GameSessionMgr.Instance.SetBet(PlayerType.Player, param.IntValue);
                var randomBet = AIMgr.RandomBet(GameSessionMgr.Instance.AIChips, LevelMgr.Instance.CurMinBetChip,
                    LevelMgr.Instance.CurMaxBetChip);
                GameSessionMgr.Instance.SetBet(PlayerType.AI, randomBet);
                
                _betUI.LevelInfoUI.SetCurBet(PlayerType.Player, param.IntValue);
                _betUI.LevelInfoUI.SetCurBet(PlayerType.AI, randomBet);
                XAttachMachine.ExitState(StateIDStr);
            }
        }
    }

    public interface IBetUI:IBaseAttachUI
    {
        public BetUI       BetUI       { get; }
        public LevelInfoUI LevelInfoUI { get; }
    }
}