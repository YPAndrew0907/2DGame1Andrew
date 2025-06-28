using System.Collections;
using System.Collections.Generic;
using Cfg;
using Mgr;
using Obj;
using UI;
using UnityEngine;
using XYZFrameWork;

namespace AttachMachine
{
    // 要牌状态
    public class AskCardUIState : BaseGameUIState
    {
        public override string          StateID => StateIDStr;
        public const    string          StateIDStr = "AskCardUIState";
        private         IAskCardUIState _askCardUI;

        private bool _isAskCard;
        private bool _isAi;

        public override void OnCreate(IMachineMaster sceneUI)
        {
            if (sceneUI is IAskCardUIState askCardUI)
            {
                _askCardUI = askCardUI;
                _askCardUI.AskCardUI.Init();
                NotifyMgr.RegisterNotify(NotifyDefine.ASK_CARD,OnAskCard);
            }
        }

        public override void OnActive()
        {
            base.OnActive();
            _askCardUI.AskCardUI.Hide();
        }

        public override void OnInActive()
        {
            base.OnInActive();
            _askCardUI.AskCardUI.Hide();
        }


        public override IEnumerator OnEnterAsync(object payload)
        {
            var curPlayer = GameSessionMgr.Instance.CurPlayerType;
            if (curPlayer == PlayerType.None)
            {
                Debug.LogError("【有错误】逻辑不对，当前玩家为 none");
                yield break;
            }
            else
            {
                var isAI      = curPlayer == PlayerType.AI;
                _askCardUI.SkillsUI.ShowSkills(!isAI);
                if (isAI)
                {
                    var aiAskCard = AIMgr.AIAskCard(GameSessionMgr.Instance.CurAICardTotalNum);
                    _askCardUI.AskCardUI.ShowUI(true,aiAskCard);
                }
                else
                {
                    _askCardUI.AskCardUI.ShowUI(false);
                }
            }
        }

        public override IEnumerator OnExitAsync(object payload)
        {
            if (payload == XAttachMachine.ExitNullObject)
            {
                _askCardUI.AskCardUI.Hide();
                _askCardUI.SkillsUI.Hide();
            }
            else
            {
                _askCardUI.AskCardUI.Hide();
            }
            yield break;
        }

        public override void OnUpdate(float deltaTime)
        {

        }

        private void OnAskCard(NotifyMsg obj)
        {
            if (obj.Param is CustomParam param)
            {
                var list = param.Value as List<bool>;
                if (list is not { Count : > 0 })
                {
                    Debug.LogError(LogTxt.PARAM_TRANSFORM_ERROR);
                    return;
                }

                _isAi      = list[0];
                _isAskCard = list[1];
                if (!_isAskCard)
                {
                    if (_isAi)
                        GameSessionMgr.Instance.AIIsContinue = false;
                    else
                        GameSessionMgr.Instance.PlayerIsContinue = false;
                }
                var curPlayer  = GameSessionMgr.Instance.CurPlayerType;
                var nextPlayer = GameSessionMgr.Instance.NextPlayerAskCard();
                if (curPlayer == PlayerType.AI)
                {
                    // AI换牌
                    var (_, count) = SkillMgr.Instance.GetSkillParameters(PlayerSkill.StealAndInsert);
                    var (curIndex, replaceIndex) = AIMgr.AIReplaceCard(GameSessionMgr.Instance.AICards,
                        GameSessionMgr.Instance.AISkillCards, count);
                    if (replaceIndex is { Count: > 0 })
                    {
                        NotifyMgr.SendEvent(NotifyDefine.REPLACE_CARD, new ReplaceCardData()
                        {
                            IsAI       = true,
                            TargetList = curIndex,
                            SkillCard  = replaceIndex
                        });
                    }
                    else
                    {
                        Debug.Log($"【放技能】：释放技能，无需替换");
                    }
                }

                Debug.Log($"【切要牌】 {nextPlayer} 要牌");
                if (_isAskCard)
                {
                    XAttachMachine.SwitchState(StateIDStr, DealCardUIState.StateIDStr);
                }
                else
                {
                    var anyIsContinue = GameSessionMgr.Instance.AIIsContinue || GameSessionMgr.Instance.PlayerIsContinue;
                    XAttachMachine.SwitchState(StateIDStr, anyIsContinue ? StateIDStr : CompareCardUIState.StateIDStr);
                }
            }
        }
    }

    public interface IAskCardUIState : IBaseAttachUI
    {
        public AskCardUI AskCardUI { get; }
        public SkillsUI  SkillsUI  { get; }
        
        public DealCardPlayerUI PlayerUI { get;  }
        public DealCardAIUI     AICardUI { get;  }
    }
}