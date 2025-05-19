using System;
using XYZFrameWork.Base;
using TMPro;
using UnityEngine.UI;
using Base;
using Cfg;
using Mgr;
using Obj;
using UnityEngine;
namespace UI
{
    public class CardItem : BaseViewMono
    {
    	//AUTO-GENERATE
    	private UnityEngine.UI.Image _imgCard;
    	private UnityEngine.UI.Image ImgCard 
    			=> _imgCard ??= transform.Find("out_img_card").GetComponent<UnityEngine.UI.Image>();

    	private UnityEngine.UI.Outline _outCard;
    	private UnityEngine.UI.Outline OutCard 
    			=> _outCard ??= transform.Find("out_img_card").GetComponent<UnityEngine.UI.Outline>();

    	//AUTO-GENERATE-END
		
		public CardObj CardValue { get; private set;}
		
		private readonly Color _hideColor = new Color(0, 0, 0, 0);
		private readonly Color _showColor = new Color(1, 1, 1, 1);
		
        public void SetCard(CardObj cardValue, Func<CardObj,bool> isShow)
        {
	        CardValue = cardValue;
	        RefreshCard(cardValue, isShow);
        }
        
        private void RefreshCard(CardObj cardValue, Func<CardObj, bool> isShow)
        {
	        if (isShow == null)
	        {
		        Debug.LogError(LogTxt.PARAM_ERROR);
		        return;
	        }
	        
	        if (cardValue == null)
	        {
		        ImgCard.sprite = null;
		        gameObject.SetActive(false);
	        }
	        else
	        {
		        bool faceUp = isShow(cardValue);
		        var img = faceUp ? ResMgr.Instance.GetCardImg(cardValue) : ResMgr.Instance.GetCardBackImg();
		        ImgCard.sprite = img;
		        gameObject.SetActive(true);
	        }
        }
        public void HideImg()
        {
	        ImgCard.color = _hideColor;
        }
        public void ShowImg()
        {
	        ImgCard.color = _showColor;
        }
        public void CancelSelect()
        {
	        OutCard.effectColor    = Color.black;
	        OutCard.effectDistance = new Vector2(1, -1);
        }
        
        public void Selected()
        {
	        OutCard.effectColor    = new Color(1,1,0.26f,1);
	        OutCard.effectDistance = new Vector2(3, -3);
        }
    }
}
