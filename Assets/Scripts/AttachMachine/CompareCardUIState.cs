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

                NotifyMgr.RegisterNotify(NotifyDefine.NEXT_ROUND, OnNextRound);
                NotifyMgr.RegisterNotify(NotifyDefine.GAME_END_GIVEUP, OnGameGiveUp);
            }
        }

        public override IEnumerator OnEnterAsync(object payload)
        {
            List<KeyValuePair<string, List<CardObj>>> records;
            if (payload is PlayerType playerType)
            {
                // 出千而结束
                switch (playerType)
                {
                    case PlayerType.Player: 
                        records = new()
                        {
                            new KeyValuePair<string, List<CardObj>>(DataMgr.Instance.PlayerName, null),
                            new KeyValuePair<string, List<CardObj>>(DataMgr.Instance.AIName, DataMgr.Instance.LastRoundAICards)
                        };
                        break;
                    case PlayerType.AI:
                        records = new()
                        {
                            new KeyValuePair<string, List<CardObj>>(DataMgr.Instance.PlayerName, DataMgr.Instance.LastRoundPlayerCards),
                            new KeyValuePair<string, List<CardObj>>(DataMgr.Instance.AIName,null)
                        };
                        break;
                    default:             
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                DataMgr.Instance.ClearPlayerCards();
                records = new()
                {
                    new KeyValuePair<string, List<CardObj>>(DataMgr.Instance.PlayerName, DataMgr.Instance.LastRoundPlayerCards),
                    new KeyValuePair<string, List<CardObj>>(DataMgr.Instance.AIName, DataMgr.Instance.LastRoundAICards)
                };
            }
            
            _uiState.CompareCardUI.Show(records);
            yield break;
        }

        public override IEnumerator OnExitAsync(object payload)
        {
            _uiState.CompareCardUI.Hide();
            DataMgr.Instance.TurnCounter();
            if (payload == null)
            {
                if (DataMgr.Instance.BossEnough && DataMgr.Instance.PlayerEnough)
                {
                    yield return XAttachMachine.EnterState(BetUIState.StateIDStr);
                }
                else
                {
                    yield return XAttachMachine.EnterState(GameEndUIState.StateIDStr,
                        DataMgr.Instance.PlayerEnough ? GameEndCode.Win : GameEndCode.Lose);
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

        private void OnNextRound(NotifyMsg obj)
        {
            XAttachMachine.ExitState(StateIDStr);
        }

        private void OnGameGiveUp(NotifyMsg obj)
        {
            XAttachMachine.ExitState(StateIDStr, 1);
        }
    }

    public interface ICompareCardUIState : IBaseAttachUI
    {
        public CompareCardUI CompareCardUI { get; }
    }
}