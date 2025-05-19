using System.Collections.Generic;
using XYZFrameWork.Base;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Base;
using Mgr;
using Obj;
namespace UI
{
    public class DealCardPlayerUI : BaseViewMono
    {
    	//AUTO-GENERATE
    	private UI.CardHeap _monoPlayerCardHeap;
    	private UI.CardHeap MonoPlayerCardHeap 
    			=> _monoPlayerCardHeap ??= transform.Find("go_bg/mono_PlayerCardHeap").GetComponent<UI.CardHeap>();

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
			MonoPlayerCardHeap.SetCard(null,CardMgr.IsCardShowPlayerCardList);
		}
		public void Show()
		{
			GoBg.SetActive(true);
		}
		public void Hide()
		{
			GoBg.SetActive(false);
			MonoPlayerCardHeap.ClearCard();
			MonoPlayerCardHeap.RefreshCard();
		}
		public void ReceiveCard(CardObj card, Vector3 pos)
		{
			MonoPlayerCardHeap.AddCard(card);
			MonoPlayerCardHeap.RefreshCard();
			TxtTotalNum.text = MonoPlayerCardHeap.CardNum().ToString();
		}
		public List<CardObj> RemoveToPublic()
		{
			var termList =  MonoPlayerCardHeap.RemoveAll();
			MonoPlayerCardHeap.RefreshCard();
			return termList;
		}

    }
}
