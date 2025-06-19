using System;
using System.Collections;
using Mgr;
using Obj;
using UI;
using XYZFrameWork;

namespace AttachMachine
{
    public class GameEndUIState: BaseGameUIState
    {
        public override string          StateID => StateIDStr;
        public const    string          StateIDStr = "GameEndUIState";
        private         IGameEndUIState _gameEndUI;

        private GameEndCode _curCode;

        public override void OnCreate(IMachineMaster sceneUI)
        {
            if (sceneUI is IGameEndUIState ui)
            {
                _gameEndUI = ui;
                _gameEndUI.GameLossUI.Init();
                _gameEndUI.GameWinUI.Init();
                NotifyMgr.RegisterNotify(NotifyDefine.CLOSE_GAME_END_UI, OnCloseGameEndUI);
            }
        }

        public override IEnumerator OnEnterAsync(object payload)
        {
            if (payload is GameEndCode endCode)
            {
                _curCode = endCode;

                var moneyDelta = GameSessionMgr.Instance.PlayerChips - PlayerProfileMgr.Instance.Money;
                switch (endCode)
                {
                    case GameEndCode.GiveUp: 
                        _gameEndUI.GameLossUI.Show(endCode,0);
                        break;
                    case GameEndCode.Lose:
                        PlayerProfileMgr.Instance.SetMoney(GameSessionMgr.Instance.PlayerChips);
                        PlayerProfileMgr.Instance.SaveProfile();

                        _gameEndUI.GameLossUI.Show(endCode,moneyDelta);
                        break;
                    case GameEndCode.Win:
                        PlayerProfileMgr.Instance.SetMoney(GameSessionMgr.Instance.PlayerChips);
                        PlayerProfileMgr.Instance.SaveProfile();

                        SkillMgr.Instance.WinUnLockSkill(GameSessionMgr.Instance.CurBossSkills);
                        LevelMgr.Instance.SetCurrentLevel(LevelMgr.Instance.CurrentLevel + 1);
                        LevelMgr.Instance.SaveLevel();
                        _gameEndUI.GameWinUI.Show(endCode,moneyDelta, GameSessionMgr.Instance.CurBossSkills);
                        break;
                }
            }
            yield break;
        }

        public override IEnumerator OnExitAsync(object payload)
        {
            _curCode = GameEndCode.None;
            CardMgr.Instance.ResetCards(true);
            NotifyMgr.SendEvent(NotifyDefine.GAME_END_BACK_HOME);
            yield break;
        }

        public override void  OnUpdate(float deltaTime)
        {
            
        }

        private void OnCloseGameEndUI(NotifyMsg obj)
        {
            XAttachMachine.ExitState(StateIDStr, 1);
        }
    }

    public interface IGameEndUIState : IBaseAttachUI
    {
        public GameLossUI GameLossUI { get; }
        public GameWinUI  GameWinUI { get; }
    }
}