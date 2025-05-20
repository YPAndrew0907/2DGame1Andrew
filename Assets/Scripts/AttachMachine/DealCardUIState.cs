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
                NotifyMgr.RegisterNotify(NotifyDefine.NEXT_ROUND,OnNextRound);
                NotifyMgr.RegisterNotify(NotifyDefine.SKILL_SELECT,OnSelectSkill);
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
            if (payload == null)
            {
                // 一般发牌
                var card = CardMgr.Instance.Deal();
                card.Owner = DataMgr.Instance.LastPlayerType;

                yield return DealCard(card);
                yield return XAttachMachine.ExitStateCor(StateIDStr);
            }
            else
            {
                // 首次发牌
                var cards = CardMgr.Instance.GetCards(4);

                cards[0].IsFirstCard = true;
                cards[1].IsFirstCard = true;

                for (int i = 0; i < cards.Count; i++)
                {
                    if (i % 2 != 0)
                        cards[i].Owner = PlayerType.Player;
                    else
                        cards[i].Owner = PlayerType.AI;
                }

                yield return DealCards(cards);
                yield return XAttachMachine.ExitStateCor(StateIDStr);
            }
        }

        public override IEnumerator OnExitAsync(object payload)
        {
            yield return XAttachMachine.EnterState(AskCardUIState.StateIDStr);
        }

        public override void OnUpdate(float deltaTime)
        {
            
        }

        private IEnumerator DealCards(List<CardObj> cardObjs)
        {
            while (cardObjs.Count > 0)
            {
                var card = cardObjs.First();
                cardObjs.Remove(card);

                yield return DealCard(card);
            }
        }

        private IEnumerator DealCard(CardObj card)
        {
            Debug.Log($"【获得牌】{card.Owner} --> {card}");
            NotifyMgr.SendEvent(NotifyDefine.DEAL_CARD,card);
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

            playerCards.AddRange(aiCards);
            NotifyMgr.SendEvent(NotifyDefine.COLLECT_PLAYED_CARD, playerCards);
        }

        private void OnSelectSkill(NotifyMsg obj)
        {
            if (obj.Param is NormalParam param)
            {
                PlayerSkill skill = (PlayerSkill)param.IntValue;
                if (skill == PlayerSkill.Guess)
                {
                    _dealCardUI.DealCardAIUI.ShowRangeTxt();
                }
            }
        }
    }
    public interface IDealCardUIState:IBaseAttachUI
    {
        public TotalCardHeapUI  TotalCardHeapUI  { get;  }
        public DealCardAIUI     DealCardAIUI     { get;  }
        public DealCardPlayerUI DealCardPlayerUI { get;}
    }
}