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

                NotifyMgr.RegisterNotify(NotifyDefine.GAME_END, OnGameEnd);
                NotifyMgr.RegisterNotify(NotifyDefine.GAME_NEXT_ROUND, OnNextRound);
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

        private void OnGameEnd(NotifyMsg obj)
        {
            if (obj.Param is NormalParam param)
            {
                WillExit(true, param.IntValue);
            }
        }

        private void OnNextRound(NotifyMsg obj)
        {
            WillExit(false,0);
        }

        private void WillExit(bool isEnd, int endState)
        {
            if (!isEnd)
            {
                GameSessionMgr.Instance.NextRound();
                GameSessionMgr.Instance.SwitchShufflePlayer();
                XAttachMachine.SwitchState(StateIDStr, BetUIState.StateIDStr);
                return;
            }
            
            if (endState == 0)
            {
                XAttachMachine.SwitchState(StateIDStr, GameEndUIState.StateIDStr, GameEndCode.GiveUp);
            }
            else
            {
                XAttachMachine.SwitchState(StateIDStr, GameEndUIState.StateIDStr,
                    endState > 0 ? GameEndCode.Win : GameEndCode.Lose);
            }
        }

        private void ClearHandCards()
        {
            _uiState.DealCardPlayerUI.RemoveCurHandCards();
            _uiState.DealCardAIUI.RemoveToPublic();
            var list = new List<CardObj>(GameSessionMgr.Instance.PlayerCards);
            list.AddRange(GameSessionMgr.Instance.AICards);
            NotifyMgr.SendEvent(NotifyDefine.COLLECT_PLAYED_CARD, list);
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