using System.Collections.Generic;
using UnityEngine;
using Base;
using DG.Tweening;
using Mgr;
using Obj;
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

    	private TMPro.TextMeshProUGUI _txtTotalNum;
    	private TMPro.TextMeshProUGUI TxtTotalNum 
    			=> _txtTotalNum ??= transform.Find("go_bg/txt_totalNum").GetComponent<TMPro.TextMeshProUGUI>();

    	//AUTO-GENERATE-END
		public void Init()
		{
			GoBg.SetActive(false);
			MonoAICardHeap.SetCard(null,CardMgr.IsCardShowAICardList);
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
			TxtTotalNum.text = MonoAICardHeap.CardNum().ToString();
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
