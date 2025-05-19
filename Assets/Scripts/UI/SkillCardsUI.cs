using System.Collections.Generic;
using System.Linq;
using Base;
using Mgr;
using Obj;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
namespace UI
{
	public class SkillCardsUI : BaseViewMono
	{
		//AUTO-GENERATE
		private UnityEngine.UI.Button _btnCheckSelect;
		private UnityEngine.UI.Button BtnCheckSelect 
				=> _btnCheckSelect ??= transform.Find("go_Bg/btn_CheckSelect").GetComponent<UnityEngine.UI.Button>();

		private UI.CardHeap _monoSkillCardHeap;
		private UI.CardHeap MonoSkillCardHeap 
				=> _monoSkillCardHeap ??= transform.Find("go_Bg/mono_SkillCardHeap").GetComponent<UI.CardHeap>();

		private UI.CardHeap _monoTotalCardHeap;
		private UI.CardHeap MonoTotalCardHeap 
				=> _monoTotalCardHeap ??= transform.Find("go_Bg/mono_TotalCardHeap").GetComponent<UI.CardHeap>();

		private UnityEngine.GameObject _goBg;
		private UnityEngine.GameObject GoBg 
				=> _goBg ??= transform.Find("go_Bg").gameObject;

		//AUTO-GENERATE-END
		private List<CardObj> _skillCardList;
		private int           _maxCardCount;
		private List<CardObj> _termSelectCard;
		public int SkillCardCount => _skillCardList.Count;
		public void Init()
		{
			GoBg.SetActive(false);
			_skillCardList = new List<CardObj>();
			BtnCheckSelect.onClick.RemoveAllListeners();
			BtnCheckSelect.onClick.AddListener(CheckSelect);
		}
		public void Show(int maxCardCount)
		{
			_maxCardCount = maxCardCount;
			GoBg.SetActive(maxCardCount > 0);
		}
		public void Hide()
		{
			GoBg.SetActive(false);
		}

		private void RefreshSkillCard()
		{
			MonoSkillCardHeap.SetCard(_skillCardList.ToArray(),CardMgr.IsCardShowSkillCardList);
		}
		public bool SetSkillCard(List<CardObj> cardObj)
		{
			if (cardObj.Count + SkillCardCount > 5)
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
			CardItem cardItem         = first.GetComponent<CardItem>();
			if (cardItem != null)
			{
				if (_termSelectCard.Contains(cardItem.CardValue))
				{
					cardItem.CancelSelect();
					_termSelectCard.Remove(cardItem.CardValue);
				}
				else
				{
					if (_termSelectCard.Count < _maxCardCount)
					{
						_termSelectCard.Add(cardItem.CardValue);
						cardItem.Selected();
					}
				}
			}
		}

		private void CheckSelect()
		{
			if (_termSelectCard.Count > 0)
			{
				_skillCardList = _termSelectCard;
				_termSelectCard.Clear();
				MonoTotalCardHeap.gameObject.SetActive(false);
				RefreshSkillCard();
				NotifyMgr.Instance.SendEvent(NotifyDefine.SKILL_CARD_SELECT_START, _termSelectCard.ToList());
			}
		}
	}
}
