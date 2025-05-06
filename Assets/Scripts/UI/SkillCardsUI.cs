using System.Collections.Generic;
using XYZFrameWork.Base;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Base;
using Mgr;
using Obj;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
namespace UI
{
    public class SkillCardsUI : BaseViewMono
    {
		//AUTO-GENERATE
		private Button _btnCheckSelect;
		private CardHeap _monoSkillCardHeap;
		private CardHeap _monoTotalCardHeap;

		protected override void FindUI()
		{
			   _btnCheckSelect = transform.Find("btn_CheckSelect").GetComponent<UnityEngine.UI.Button>();
			_monoSkillCardHeap = transform.Find("mono_SkillCardHeap").GetComponent<UI.CardHeap>();
			_monoTotalCardHeap = transform.Find("mono_TotalCardHeap").GetComponent<UI.CardHeap>();
		}


		//AUTO-GENERATE-END
        
		private  List<CardObj> _skillCardList;
		private int MaxCardCount;
		
		[SerializeField]
		private List<CardObj> termSelectCard;
		
		public int SkillCardCount => _skillCardList.Count;
		
		
		
		public void InitSkillCard(int maxCardCount)
		{
			MaxCardCount = maxCardCount;
			_monoTotalCardHeap.gameObject.SetActive(false);
			if (maxCardCount<=0)
			{
				gameObject.SetActive(false);
			}
			else
			{
				_skillCardList = new List<CardObj>();
			}
		}
		public void RefreshSkillCard()
		{
			_monoSkillCardHeap.SetCard(_skillCardList.ToArray());
		}
		
		public bool AddSkillCard(CardObj[] cardObj)
		{
			if (cardObj.Length + SkillCardCount > 5)
			{
				return false;
			}
			
			_skillCardList.AddRange(cardObj);
			return true;
		}
		public void SelectCard(BaseEventData eventData)
		{
			 var      pointerEventData = eventData as PointerEventData;
			 var      first            = pointerEventData.hovered[0];
			 CardItem cardItem = first.GetComponent<CardItem>();
			 if (cardItem != null)
			 {
				 if (termSelectCard.Contains(cardItem.CardValue))
				 {
					 cardItem.CancelSelect();
					 termSelectCard.Remove(cardItem.CardValue);
				 }
				 else
				 {
					 if (termSelectCard.Count < MaxCardCount)
					 {
						 termSelectCard.Add(cardItem.CardValue);
						 cardItem.Selected();
					 }
				 }
			 }
		}
		public void CheckSelect()
		{
			if (termSelectCard.Count > 0)
			{
				_monoSkillCardHeap.ClearCard();
				_monoSkillCardHeap.AddCard(termSelectCard.ToArray());
				_monoTotalCardHeap.gameObject.SetActive(false);
				NotifyMgr.Instance.SendEvent(NotifyDefine.SKILL_CARD_SELECT_END, termSelectCard.ToArray());
			}
		}
		
    }
}
