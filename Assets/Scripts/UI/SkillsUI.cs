using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Base;
using Cfg;
using DG.Tweening;
using Mgr;
using Obj;
using UnityEngine;
using UnityEngine.UI;
namespace UI
{
	public class SkillsUI : BaseViewMono, ICanShowDetectFlag
	{
		//AUTO-GENERATE
		private UnityEngine.UI.Button _btnDetectFlag;
		private UnityEngine.UI.Button BtnDetectFlag 
				=> _btnDetectFlag ??= transform.Find("go_Bg/go_btn_img_DetectFlag").GetComponent<UnityEngine.UI.Button>();

		private UI.CardZone _monoSkillCardHeap;
		private UI.CardZone MonoSkillCardHeap 
				=> _monoSkillCardHeap ??= transform.Find("go_Bg/go_mono_SkillCardHeap").GetComponent<UI.CardZone>();

		private UnityEngine.GameObject _goBg;
		private UnityEngine.GameObject GoBg 
				=> _goBg ??= transform.Find("go_Bg").gameObject;

		private UnityEngine.GameObject _goDetectFlag;
		private UnityEngine.GameObject GoDetectFlag 
				=> _goDetectFlag ??= transform.Find("go_Bg/go_btn_img_DetectFlag").gameObject;

		private UnityEngine.GameObject _goSkillCardHeap;
		private UnityEngine.GameObject GoSkillCardHeap 
				=> _goSkillCardHeap ??= transform.Find("go_Bg/go_mono_SkillCardHeap").gameObject;

		private UnityEngine.GameObject _goSkillList;
		private UnityEngine.GameObject GoSkillList 
				=> _goSkillList ??= transform.Find("go_Bg/go_SkillScroll/viewport/go_SkillList").gameObject;

		private UnityEngine.GameObject _goSkillScroll;
		private UnityEngine.GameObject GoSkillScroll 
				=> _goSkillScroll ??= transform.Find("go_Bg/go_SkillScroll").gameObject;

		private UnityEngine.UI.Image _imgDetectFlag;
		private UnityEngine.UI.Image ImgDetectFlag 
				=> _imgDetectFlag ??= transform.Find("go_Bg/go_btn_img_DetectFlag").GetComponent<UnityEngine.UI.Image>();

		private UI.ItemSkillBtn _monoItemSkill;
		private UI.ItemSkillBtn MonoItemSkill 
				=> _monoItemSkill ??= transform.Find("go_Bg/go_SkillScroll/mono_ItemSkill").GetComponent<UI.ItemSkillBtn>();

		//AUTO-GENERATE-END
		
		/// <summary>
		/// 开局送的牌
		/// </summary>
		private List<CardObj> _skillCardList;
		
		private int                                   SkillCardCount => _skillCardList.Count;
		private int                                   _maxCardCount;
		private List<ItemSkillBtn>                    _addedBtn;
		private Dictionary<PlayerSkill, ItemSkillBtn> _skillTypeToBtn; // 技能类型和对应的按钮
		public void Init()
		{
			GoBg.SetActive(false);
			GoDetectFlag.SetActive(false);
			MonoItemSkill.gameObject.SetActive(false);
			MonoSkillCardHeap.SetCard(null, CardMgr.IsCardShowSkillCardList);
			MonoSkillCardHeap.RefreshCard();
			_skillCardList  = new List<CardObj>();
			_addedBtn       = new List<ItemSkillBtn>();
			_skillTypeToBtn = new Dictionary<PlayerSkill, ItemSkillBtn>();
		}
		public void Show(int maxCardCount,IEnumerable<PlayerSkill> skills, IReadOnlyList<CardObj> skillCardList)
		{
			_maxCardCount = maxCardCount;
			MonoSkillCardHeap.ClearCard();
			MonoSkillCardHeap.SetCard(skillCardList, CardMgr.IsCardShowSkillCardList);
			MonoSkillCardHeap.RefreshCard();
			GoSkillCardHeap.SetActive(_maxCardCount!= 0);
			SetSkills(skills);
		}
		public void Hide()
		{
			GoBg.SetActive(false);
		}
		public void SetSkills(IEnumerable<PlayerSkill> skills)
		{
			if (_addedBtn == null)
			{
				Debug.LogError(LogTxt.NOT_SET_INIT_VALUE_ERROR);
				return;
			}
			foreach (var item in _addedBtn)
			{
				Destroy(item.gameObject);
			}
			_skillTypeToBtn.Clear();
			_addedBtn.Clear();
			var playerSkills = skills.ToList();
			var i            = 0;
			for (i = 0; i < playerSkills.Count; i++)
			{
				var script = Instantiate(MonoItemSkill.gameObject, GoSkillList.transform)?.GetComponent<ItemSkillBtn>();
				if (script == null)
				{
					Debug.LogError(LogTxt.TYPE_ERROR);
					return;
				}
				script.Init(playerSkills[i]);
				_skillTypeToBtn.Add(playerSkills[i],script);
				_addedBtn.Add(script);
			}
			if (playerSkills.Count > 0)
			{
				GoBg.SetActive(true);
			}
		}
		
		public bool SetSkillCard(List<CardObj> cardObj)
		{
			_skillCardList.Clear();
			_skillCardList.AddRange(cardObj);
			_skillCardList.Sort();
			return true;
		}
		
		public void RefreshUI()
		{
			MonoSkillCardHeap.SetCard(_skillCardList.ToArray(),CardMgr.IsCardShowSkillCardList);
			if(_skillCardList.Count > 0)
			{
				GoSkillCardHeap.SetActive(true);
			}
		}
		public void ShowSkills(bool inMyRound)
		{
			var count = 0;
			foreach (var (key, value) in _skillTypeToBtn)
			{
				count+= value.SetShow(inMyRound);
			}
			GoSkillList.SetActive(count > 0);
		}
		public void OnDetectFlagClick()
		{
			NotifyMgr.SendEvent(NotifyDefine.SKILL_CLICK, (int)PlayerSkill.Detect);
		}
		public void ShowDetectFlag()
		{
			GoDetectFlag.SetActive(true);
			ImgDetectFlag.color = new Color(0.76f,0.56f,0,  1);
			var tweener = ImgDetectFlag.DOFade(0f, 1f).SetEase(Ease.InOutSine)
			                           .SetLoops(3, LoopType.Yoyo);
			tweener.OnComplete(() =>
			{
				GoDetectFlag.SetActive(false);
			});
		}
	}
}
