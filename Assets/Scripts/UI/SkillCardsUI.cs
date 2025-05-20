using System.Collections.Generic;
using Base;
using Mgr;
using Obj;
namespace UI
{
	public class SkillCardsUI : BaseViewMono
	{
		//AUTO-GENERATE
		private UI.CardHeap _monoSkillCardHeap;
		private UI.CardHeap MonoSkillCardHeap 
				=> _monoSkillCardHeap ??= transform.Find("go_Bg/go_mono_SkillCardHeap").GetComponent<UI.CardHeap>();

		private UnityEngine.GameObject _goBg;
		private UnityEngine.GameObject GoBg 
				=> _goBg ??= transform.Find("go_Bg").gameObject;

		private UnityEngine.GameObject _goSkillCardHeap;
		private UnityEngine.GameObject GoSkillCardHeap 
				=> _goSkillCardHeap ??= transform.Find("go_Bg/go_mono_SkillCardHeap").gameObject;

		//AUTO-GENERATE-END
		
		/// <summary>
		/// 开局送的牌
		/// </summary>
		private List<CardObj> _skillCardList;
		private int           SkillCardCount => _skillCardList.Count;
		private int           _maxCardCount;
		public void Init()
		{
			GoBg.SetActive(false);
			MonoSkillCardHeap.SetCard(null, CardMgr.IsCardShowSkillCardList);
			MonoSkillCardHeap.RefreshCard();
			_skillCardList = new List<CardObj>();
		}
		public void Show(int maxCardCount)
		{
			_maxCardCount = maxCardCount;
			MonoSkillCardHeap.ClearCard();
			MonoSkillCardHeap.RefreshCard();
			GoBg.SetActive(_maxCardCount > 0);
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
			_skillCardList.Clear();
			if (cardObj.Count + SkillCardCount > 5)
			{
				return false;
			}
			_skillCardList.AddRange(cardObj);
			_skillCardList.Sort();
			return true;
		}

		public void RefreshUI()
		{
			RefreshSkillCard();
		}
	}
}
