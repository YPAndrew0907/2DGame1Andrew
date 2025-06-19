using System;
using System.Collections;
using System.Collections.Generic;
using Mgr;
using Obj;
using UI;
using XYZFrameWork;

namespace AttachMachine
{
    public class CompareCardUIState : BaseGameUIState
    {
        public override string StateID => StateIDStr;

        public const string              StateIDStr = "CompareCardUIState";
        private      ICompareCardUIState _uiState;

        public override void OnCreate(IMachineMaster sceneUI)
        {
            if (sceneUI is ICompareCardUIState ui)
            {
                _uiState = ui;
                _uiState.CompareCardUI.Init();

                NotifyMgr.RegisterNotify(NotifyDefine.GAME_END_GIVEUP, OnGameGiveUp);
            }
        }

        public override IEnumerator OnEnterAsync(object payload)
        {
            ClearHandCards();
            List<KeyValuePair<string, IReadOnlyList<CardObj>>> records;
            if (payload is PlayerType playerType)
            {
                // 出千而结束
                switch (playerType)
                {
                    case PlayerType.Player:
                        records = new()
                        {
                            new KeyValuePair<string, IReadOnlyList<CardObj>>("You", null),
                            new KeyValuePair<string, IReadOnlyList<CardObj>>(LevelMgr.Instance.AIName, GameSessionMgr.Instance.LastRoundAICards)
                        };
                        break;
                    case PlayerType.AI:
                        records = new()
                        {
                            new KeyValuePair<string, IReadOnlyList<CardObj>>("You", GameSessionMgr.Instance.LastRoundPlayerCards),
                            new KeyValuePair<string, IReadOnlyList<CardObj>>(LevelMgr.Instance.AIName,null)
                        };
                        break;
                    default:             
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                records = new()
                {
                    new KeyValuePair<string, IReadOnlyList<CardObj>>("You", GameSessionMgr.Instance.LastRoundPlayerCards),
                    new KeyValuePair<string, IReadOnlyList<CardObj>>(LevelMgr.Instance.AIName, GameSessionMgr.Instance.LastRoundAICards)
                };
            }
            
            _uiState.CompareCardUI.Show(records);
            _uiState.LevelInfoUI.SetMoney(GameSessionMgr.Instance.PlayerChips);
            
            yield break;
        }

        public override IEnumerator OnExitAsync(object payload)
        {
            GameSessionMgr.Instance.NextRound();
            GameSessionMgr.Instance.SwitchShufflePlayer();
            
            
            if (payload == null)
            {
                if (GameSessionMgr.Instance.BossEnough && GameSessionMgr.Instance.PlayerEnough)
                {
                    yield return XAttachMachine.EnterState(BetUIState.StateIDStr);
                }
                else
                {
                    yield return XAttachMachine.EnterState(GameEndUIState.StateIDStr,
                        GameSessionMgr.Instance.PlayerEnough ? GameEndCode.Win : GameEndCode.Lose);
                }
            }
            else
            {
                yield return XAttachMachine.EnterState(GameEndUIState.StateIDStr, GameEndCode.GiveUp);
            }
        }

        public override void OnUpdate(float deltaTime)
        {
            
        }

        private void OnGameGiveUp(NotifyMsg obj)
        {
            XAttachMachine.ExitState(StateIDStr, 1);
        }

        private void ClearHandCards()
        {
            _uiState.DealCardPlayerUI.RemoveCurHandCards();
            _uiState.DealCardAIUI.RemoveToPublic();
            GameSessionMgr.Instance.StoreLastCard();
        }
    }

    public interface ICompareCardUIState : IBaseAttachUI
    {
        public LevelInfoUI      LevelInfoUI      { get; }
        public CompareCardUI    CompareCardUI    { get; }
        public DealCardPlayerUI DealCardPlayerUI { get; }
        public DealCardAIUI     DealCardAIUI     { get; }
    }
}