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
    	private UI.CardHeap _monoAICardHeap;
    	private UI.CardHeap MonoAICardHeap 
    			=> _monoAICardHeap ??= transform.Find("go_bg/mono_AICardHeap").GetComponent<UI.CardHeap>();

    	private UnityEngine.GameObject _goBg;
    	private UnityEngine.GameObject GoBg 
    			=> _goBg ??= transform.Find("go_bg").gameObject;

    	private UnityEngine.GameObject _goDealDes;
    	private UnityEngine.GameObject GoDealDes 
    			=> _goDealDes ??= transform.Find("go_bg/go_DealDes").gameObject;

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
			card.transform.SetParent(MonoAICardHeap.transform,true);
			card.transform.DOMove(Vector3.zero, 1).OnComplete(() =>
			{
				MonoAICardHeap.RefreshCard();
			});
		}
		public void Show()
		{
			GoBg.SetActive(true);
			MonoAICardHeap.SetCard(null,CardMgr.IsCardShowAICardList);
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
			
			MonoAICardHeap.ClearCard();
			MonoAICardHeap.RefreshCard();
		}

		public void ReceiveCard(CardObj card, Vector3 pos)
		{
			MonoAICardHeap.AddCard(card);
			MonoAICardHeap.RefreshCard();
			var realValue = MonoAICardHeap.CardNum();
			var min       = Math.Max(0, realValue - Random.Range(3, 5));
			var max       = realValue + Random.Range(3, 5);
			TxtRangeNum.text = $"{min} ~ {max}";
		}

		public List<CardObj> RemoveToPublic()
		{
			var termList =  MonoAICardHeap.RemoveAll();
			MonoAICardHeap.RefreshCard();
			return termList;
		}
		public void ClearCard()
		{
			MonoAICardHeap.ClearCard();
		}
		public int CardNum =>  MonoAICardHeap.CardNum();
    }
}
