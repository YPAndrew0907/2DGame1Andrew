using System;
using System.Collections.Generic;
using UnityEngine;
using Base;
using DG.Tweening;
using Mgr;
using Obj;
using Random = UnityEngine.Random;
namespace UI
{
    public class DealCardAIUI : BaseViewMono
    {
    	//AUTO-GENERATE
    	private UI.CardZone _monoAICardZone;
    	private UI.CardZone MonoAICardZone 
    			=> _monoAICardZone ??= transform.Find("go_bg/mono_AICardHeap").GetComponent<UI.CardZone>();

    	private UnityEngine.GameObject _goBg;
    	private UnityEngine.GameObject GoBg 
    			=> _goBg ??= transform.Find("go_bg").gameObject;

    	private UnityEngine.GameObject _goRangeNum;
    	private UnityEngine.GameObject GoRangeNum 
    			=> _goRangeNum ??= transform.Find("go_bg/go_txt_RangeNum").gameObject;

    	private TMPro.TextMeshProUGUI _txtRangeNum;
    	private TMPro.TextMeshProUGUI TxtRangeNum 
    			=> _txtRangeNum ??= transform.Find("go_bg/go_txt_RangeNum").GetComponent<TMPro.TextMeshProUGUI>();

    	//AUTO-GENERATE-END
		public void Init()
		{
			GoBg.SetActive(false);
		}
		public void DealToDes(GameObject card)
		{
			card.transform.SetParent(MonoAICardZone.transform,true);
			card.transform.DOMove(Vector3.zero, 1).OnComplete(() =>
			{
				MonoAICardZone.RefreshCard();
			});
		}
		public void Show()
		{
			GoBg.SetActive(true);
			MonoAICardZone.SetCard(null,CardMgr.IsCardShowAICardList);
			TxtRangeNum.text = String.Empty;
			GoRangeNum.SetActive(false);
		}
		public void ShowRangeTxt()
		{
			GoRangeNum.SetActive(true);
		}
		
		public void Hide()
		{
			GoBg.SetActive(false);
			
			MonoAICardZone.ClearCard();
			MonoAICardZone.RefreshCard();
		}
		public void ReceiveCard(CardObj card, Vector3 pos)
		{
			MonoAICardZone.AddCard(card);
			MonoAICardZone.RefreshCard();
			RefreshNum();
		}

		private void RefreshNum()
		{
			var realValue = MonoAICardZone.CardNum();
			var min       = Math.Max(0, realValue - Random.Range(3, 5));
			var max       = realValue + Random.Range(3, 5);
			TxtRangeNum.text = $"{min} ~ {max}";
		}

		public void UpdateCards(List<CardObj> cards)
		{
			MonoAICardZone.ClearCard();
			foreach (var card in cards){MonoAICardZone.AddCard(card);}
			
			MonoAICardZone.RefreshCard();
			RefreshNum();
		}
		public void RemoveToPublic()
		{
			MonoAICardZone.ClearCard();
			MonoAICardZone.RefreshCard();
		}
		public void ClearCard()
		{
			MonoAICardZone.ClearCard();
		}
		public int CardNum =>  MonoAICardZone.CardNum();
    }
}
