using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cfg;
using Mgr;
using Obj;
using TMPro;
using UI;
using UnityEngine;
using XYZFrameWork;

namespace AttachMachine
{
    // 发牌状态
    public class DealCardUIState : BaseGameUIState
    {
        IDealCardUIState      _dealCardUI;
        public  override string StateID => StateIDStr;
        public const     string StateIDStr = "DealCardUIState";

        public override void OnCreate(IMachineMaster sceneUI)
        {
            if ( sceneUI is IDealCardUIState ui)
            {
                _dealCardUI = ui;
                _dealCardUI.DealCardPlayerUI.Init();
                _dealCardUI.DealCardAIUI.Init();
                NotifyMgr.Register(NotifyDefine.NEXT_ROUND,OnNextRound);
            }
        }

        public override void OnActive()
        {
            base.OnActive();
            _dealCardUI.DealCardPlayerUI.Show();
            _dealCardUI.DealCardAIUI.Show();
        }

        public override void OnInActive()
        {
            base.OnInActive();
            _dealCardUI.DealCardPlayerUI.Hide();
            _dealCardUI.DealCardAIUI.Hide();
        }

        public override IEnumerator OnEnterAsync(object payload)
        {
            if (payload is List<CardObj> list)
            {
                yield return DealCards(list);
                yield return OnExitAsync(null);
                yield break;
            }
            if (payload is CardObj cardObj)
            {
                yield return DealCard(cardObj);
                yield return OnExitAsync(null);
                yield break;
            }

            Debug.LogError(LogTxt.PARAM_ERROR);
        }

        public override IEnumerator OnExitAsync(object payload)
        {
            yield return _dealCardUI.AttachMachine.EnterState(AskCardUIState.StateIDStr);
        }

        public override void OnUpdate(float deltaTime)
        {
            
        }

        private IEnumerator DealCards(List<CardObj> cardObjs)
        {
            while (cardObjs.Count > 0)
            {
                var card = cardObjs.Last();
                cardObjs.Remove(card);

                yield return DealCard(card);
            }
        }

        private IEnumerator DealCard(CardObj card)
        {
            Debug.Log($"【获得牌】{card}");
            NotifyMgr.Instance.SendEvent(NotifyDefine.DEAL_CARD,card);
                
            DataMgr.Instance.AddCardToAIOrPlayer(card);
            if (card.Owner == PlayerType.Player)
            {
                _dealCardUI.DealCardPlayerUI.ReceiveCard(card, _dealCardUI.TotalCardHeapUI.transform.position);
            }
            else
            {
                _dealCardUI.DealCardAIUI.ReceiveCard(card, _dealCardUI.TotalCardHeapUI.transform.position);
            }
            
            yield return new WaitForSeconds(0.3f);
        }

        private void OnNextRound(NotifyMsg obj)
        {
            var playerCards = _dealCardUI.DealCardPlayerUI.RemoveToPublic();
            var aiCards     = _dealCardUI.DealCardAIUI.RemoveToPublic();
            _dealCardUI.PlayedCardUI.AddCards(playerCards);
            _dealCardUI.PlayedCardUI.AddCards(aiCards);
        }
    }
    public interface IDealCardUIState:IBaseAttachUI
    {
        public PlayedCardUI     PlayedCardUI     { get; }
        public TotalCardHeapUI  TotalCardHeapUI  { get;  }
        public DealCardAIUI     DealCardAIUI     { get;  }
        public DealCardPlayerUI DealCardPlayerUI { get;}
    }
}