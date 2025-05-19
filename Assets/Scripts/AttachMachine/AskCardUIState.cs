using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
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
                NotifyMgr.Register(NotifyDefine.ASK_CARD,OnAskCard);
            }
        }

        public override IEnumerator OnEnterAsync(object payload)
        {
            var curPlayer = DataMgr.Instance.CurPlayerType;
            if (curPlayer == PlayerType.Public)
            {
                yield return OnExitAsync(null);
            }
            else
            {
                var isAI      = curPlayer == PlayerType.AI;
                var aiAskCard = AIMgr.AIAskCard(DataMgr.Instance.CurAICardNum);
                _askCardUI.AskCardUI.ShowUI(isAI,aiAskCard);
            }
        }

        public override IEnumerator OnExitAsync(object payload)
        {
            _askCardUI.AskCardUI.Hide();

            var curPlayer = DataMgr.Instance.CurPlayerType;
            var nextPlayer     = DataMgr.Instance.NextPlayerAskCard();
            
            Debug.Log($"切换到 {nextPlayer} 要牌");
            if (_isAskCard)
            {
                var card = CardMgr.Instance.Deal();
                card.Owner = curPlayer;
             
                yield return _askCardUI.AttachMachine.EnterState(DealCardUIState.StateIDStr, card);
            }
            else
            {
                var anyIsContinue = DataMgr.Instance.AIIsContinue || DataMgr.Instance.PlayerIsContinue;
                yield return _askCardUI.AttachMachine.EnterState(anyIsContinue ? StateIDStr : CompareCardUIState.StateIDStr);
            }
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
                        DataMgr.Instance.AIIsContinue = false;
                    else
                        DataMgr.Instance.PlayerIsContinue = false;
                }
                CoroutineMgr.Instance.StartCoroutine(OnExitAsync(null));
            }
        }
    }

    public interface IAskCardUIState : IBaseAttachUI
    {
        public AskCardUI AskCardUI { get; }
        
        public DealCardPlayerUI PlayerUI { get;  }
        public DealCardAIUI     AICardUI { get;  }
    }
}