using System;
using System.Collections.Generic;
using System.Linq;
using Base;
using Cfg;
using Mgr;
using Obj;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
namespace UI
{
    public class GuessOrRememberUI : BaseViewMono
    {
    	//AUTO-GENERATE
    	private UnityEngine.UI.Button _btnCheckSelect;
    	private UnityEngine.UI.Button BtnCheckSelect 
    			=> _btnCheckSelect ??= transform.Find("go_bg/go_btn_CheckSelect").GetComponent<UnityEngine.UI.Button>();

    	private UnityEngine.UI.Button _btnGuess;
    	private UnityEngine.UI.Button BtnGuess 
    			=> _btnGuess ??= transform.Find("go_bg/go_SelectSkill/btn_Guess").GetComponent<UnityEngine.UI.Button>();

    	private UnityEngine.UI.Button _btnRemember;
    	private UnityEngine.UI.Button BtnRemember 
    			=> _btnRemember ??= transform.Find("go_bg/go_SelectSkill/btn_Remember").GetComponent<UnityEngine.UI.Button>();

    	private UI.CardHeap _monoRememberCardHeap;
    	private UI.CardHeap MonoRememberCardHeap 
    			=> _monoRememberCardHeap ??= transform.Find("go_bg/go_mono_RememberCardHeap").GetComponent<UI.CardHeap>();

    	private UnityEngine.GameObject _goBg;
    	private UnityEngine.GameObject GoBg 
    			=> _goBg ??= transform.Find("go_bg").gameObject;

    	private UnityEngine.GameObject _goCheckSelect;
    	private UnityEngine.GameObject GoCheckSelect 
    			=> _goCheckSelect ??= transform.Find("go_bg/go_btn_CheckSelect").gameObject;

    	private UnityEngine.GameObject _goRememberCardHeap;
    	private UnityEngine.GameObject GoRememberCardHeap 
    			=> _goRememberCardHeap ??= transform.Find("go_bg/go_mono_RememberCardHeap").gameObject;

    	private UnityEngine.GameObject _goSelectSkill;
    	private UnityEngine.GameObject GoSelectSkill 
    			=> _goSelectSkill ??= transform.Find("go_bg/go_SelectSkill").gameObject;

    	//AUTO-GENERATE-END
		
		public List<CardValue> RememberCards { get; set; }
		public CardItem      cardItem;
		
		private int           _maxCardCount;
		private List<CardObj> _termSelectCard;
		
		
		public void Init()
		{
			GoBg.SetActive(false);
			_termSelectCard = new List<CardObj>();
			
			BtnGuess.onClick.RemoveAllListeners();
			BtnGuess.onClick.AddListener(OnClickGuess);
			
			BtnRemember.onClick.RemoveAllListeners();
			BtnRemember.onClick.AddListener(OnClickRemember);
			
			BtnCheckSelect.onClick.RemoveAllListeners();
			BtnCheckSelect.onClick.AddListener(OnCheckSelect);
			
			cardItem.AddTriggerEvent(EventTriggerType.PointerClick,SelectCard);
		}
		public void Hide()
		{
			GoBg.SetActive(false);
		}
		
		public void ShowSelectSkillPanel()
		{
			Debug.Log($"【选技能】 {LevelData.GetSkillDesc(new [] { PlayerSkill.GuessOrRemember })} 技能");
			
			GoRememberCardHeap.SetActive(false);
			GoSelectSkill.SetActive(true);
			GoCheckSelect.SetActive(false);
			GoBg.SetActive(true);
		}
		public void ShowSelectCardPanel()
		{
			_termSelectCard.Clear();
			MonoRememberCardHeap.SetCard(CardMgr.Instance.Cards,CardMgr.IsCardShowSelectCard);
			GoRememberCardHeap.SetActive(true);
			GoSelectSkill.SetActive(false);
			GoCheckSelect.SetActive(true);
			GoBg.SetActive(true);
		}
		
		public void SelectCard(BaseEventData eventData)
		{
			if (eventData is PointerEventData pointerEventData)
			{
				var      first    = pointerEventData.pointerCurrentRaycast.gameObject.transform.parent;
				CardItem item = first.GetComponent<CardItem>();
				Debug.Log(item.Value);
				if (item != null)
				{
					if (_termSelectCard.Contains(item.Value))
					{
						item.CancelSelect();
						_termSelectCard.Remove(item.Value);
					}
					else
					{
						if (_termSelectCard.Count < DataMgr.Instance.RememberCardCount)
						{
							_termSelectCard.Add(item.Value);
							item.Selected();
						}
					}
				}
			}
		}
		private void OnClickRemember()
		{
			NotifyMgr.SendEvent(NotifyDefine.SKILL_SELECT,(int)PlayerSkill.Remember);
		}
		private void OnClickGuess()
		{
			NotifyMgr.SendEvent(NotifyDefine.SKILL_SELECT,(int)PlayerSkill.Guess);
		}
		
		private void OnCheckSelect()
		{
			if (_termSelectCard.Count > 0)
			{
				NotifyMgr.SendEvent(NotifyDefine.SKILL_CARD_SELECT, _termSelectCard.ToList());
				_termSelectCard.Clear();
			}
		}
    }
}
