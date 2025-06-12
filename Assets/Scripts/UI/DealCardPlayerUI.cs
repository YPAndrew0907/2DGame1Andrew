using System;
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
    	private UI.CardZone _monoPlayerCardZone;
    	private UI.CardZone MonoPlayerCardZone 
    			=> _monoPlayerCardZone ??= transform.Find("go_bg/mono_PlayerCardHeap").GetComponent<UI.CardZone>();

    	private UnityEngine.GameObject _goBg;
    	private UnityEngine.GameObject GoBg 
    			=> _goBg ??= transform.Find("go_bg").gameObject;

    	private TMPro.TextMeshProUGUI _txtTotalNum;
    	private TMPro.TextMeshProUGUI TxtTotalNum 
    			=> _txtTotalNum ??= transform.Find("go_bg/txt_totalNum").GetComponent<TMPro.TextMeshProUGUI>();

    	//AUTO-GENERATE-END
		public void Init()
		{
			GoBg.SetActive(false);
		}
		public void Show()
		{
			GoBg.SetActive(true);
			MonoPlayerCardZone.SetCard(null,CardMgr.IsCardShowPlayerCardList);
			TxtTotalNum.text = String.Empty;
		}
		public void Hide()
		{
			GoBg.SetActive(false);
			MonoPlayerCardZone.ClearCard();
			MonoPlayerCardZone.RefreshCard();
		}
		public void ReceiveCard(CardObj card, Vector3 pos)
		{
			MonoPlayerCardZone.AddCard(card);
			MonoPlayerCardZone.RefreshCard();
			TxtTotalNum.text = MonoPlayerCardZone.CardNum().ToString();
		}
		public void RemoveCurHandCards()
		{
			MonoPlayerCardZone.RemoveAll();
			MonoPlayerCardZone.RefreshCard();
		}
    }
}
