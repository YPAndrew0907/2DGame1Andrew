using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Base;
using Cfg;
using Cysharp.Threading.Tasks.Triggers;
using Mgr;
using Obj;
using UnityEngine;

namespace UI
{
	public class SkillsUI : BaseViewMono
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

		private UnityEngine.GameObject _goSkillList;
		private UnityEngine.GameObject GoSkillList 
				=> _goSkillList ??= transform.Find("go_Bg/go_SkillScroll/viewport/go_SkillList").gameObject;

		private UnityEngine.GameObject _goSkillScroll;
		private UnityEngine.GameObject GoSkillScroll 
				=> _goSkillScroll ??= transform.Find("go_Bg/go_SkillScroll").gameObject;

		private UI.ItemSkillBtn _monoItemSkill;
		private UI.ItemSkillBtn MonoItemSkill 
				=> _monoItemSkill ??= transform.Find("go_Bg/go_SkillScroll/mono_ItemSkill").GetComponent<UI.ItemSkillBtn>();

		//AUTO-GENERATE-END
		
		/// <summary>
		/// 开局送的牌
		/// </summary>
		private List<CardObj> _skillCardList;
		private int           SkillCardCount => _skillCardList.Count;
		private int           _maxCardCount;

		private List<ItemSkillBtn>                    _addedBtn;
		private Dictionary<PlayerSkill, ItemSkillBtn> _dictionary = new();
		public void Init()
		{
			GoBg.SetActive(false);
			MonoItemSkill.gameObject.SetActive(false);
			
			MonoSkillCardHeap.SetCard(null, CardMgr.IsCardShowSkillCardList);
			MonoSkillCardHeap.RefreshCard();
			_skillCardList = new List<CardObj>();
			_addedBtn      = new List<ItemSkillBtn>();
		}
		public void Show(int maxCardCount,IEnumerable<PlayerSkill> skills)
		{
			_maxCardCount = maxCardCount;
			MonoSkillCardHeap.ClearCard();
			MonoSkillCardHeap.RefreshCard();
			GoSkillCardHeap.SetActive(_maxCardCount != 0);
			SetSkills(skills);
		}
		public void Hide()
		{
			GoBg.SetActive(false);
		}
		private void RefreshSkillCard()
		{
			MonoSkillCardHeap.SetCard(_skillCardList.ToArray(),CardMgr.IsCardShowSkillCardList);
		}
		public void SetSkills(IEnumerable<PlayerSkill> skills)
		{
			if (_addedBtn == null)
			{
				Debug.LogError(LogTxt.NOT_SET_INIT_VALUE_ERROR);
				return;
			}
			_dictionary.Clear();
			var playerSkills = skills.ToList();
			var i            = 0;
			for (i = 0; i < playerSkills.Count; i++)
			{
				var trans = i < _addedBtn.Count ? _addedBtn[i] : null;
				if (trans == null)
				{
					trans = Instantiate(MonoItemSkill.gameObject, GoSkillList.transform)?.GetComponent<ItemSkillBtn>();
					if (trans == null)
					{
						Debug.LogError(LogTxt.TYPE_ERROR);
						return;
					}
				}
				trans.Init(playerSkills[i]);
				_dictionary.Add(playerSkills[i],trans);
			}

			if (i< _addedBtn.Count)
			{
				for (int j = i; j < _addedBtn.Count; j++)
				{
					_addedBtn[j].gameObject.SetActive(false);
				}
			}
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

		public void ShowSkills(bool inMyRound)
		{
			foreach (var (key, value) in _dictionary)
			{
				value.SetShow(inMyRound);
			}
		}
	}
}
