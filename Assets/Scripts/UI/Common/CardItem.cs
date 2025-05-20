using System;
using Base;
using Cfg;
using Mgr;
using Obj;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace UI
{
    public class CardItem : BaseViewMono
    {
    	//AUTO-GENERATE
    	private UnityEngine.EventSystems.EventTrigger _triggerCard;
    	private UnityEngine.EventSystems.EventTrigger TriggerCard 
    			=> _triggerCard ??= transform.Find("trigger_out_img_card").GetComponent<UnityEngine.EventSystems.EventTrigger>();

    	private UnityEngine.GameObject _goRange;
    	private UnityEngine.GameObject GoRange 
    			=> _goRange ??= transform.Find("go_txt_Range").gameObject;

    	private UnityEngine.UI.Image _imgCard;
    	private UnityEngine.UI.Image ImgCard 
    			=> _imgCard ??= transform.Find("trigger_out_img_card").GetComponent<UnityEngine.UI.Image>();

    	private UnityEngine.UI.Outline _outCard;
    	private UnityEngine.UI.Outline OutCard 
    			=> _outCard ??= transform.Find("trigger_out_img_card").GetComponent<UnityEngine.UI.Outline>();

    	private TMPro.TextMeshProUGUI _txtRange;
    	private TMPro.TextMeshProUGUI TxtRange 
    			=> _txtRange ??= transform.Find("go_txt_Range").GetComponent<TMPro.TextMeshProUGUI>();

    	//AUTO-GENERATE-END
	    private const string  DefaultName = "CardItem (Empty)";
		public        CardObj Value { get; private set;}
		public        bool    isSelected;
		
		private readonly Color _hideColor = new Color(0, 0, 0, 0);
		private readonly Color _showColor = new Color(1, 1, 1, 1);
		public void SetCard(CardObj cardValue, Func<CardObj, bool> isShow)
		{
			if (isSelected)
			{
				CancelSelect();
			}
			Value       = cardValue;
			gameObject.name = Value != null ? Value.ToString() : DefaultName;
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
		        if (cardValue.IsShowRange && cardValue.Owner == PlayerType.AI)
		        {
			        GoRange.SetActive(true);
			        // TxtRange.text = GetRangeStr();
		        }
		        else
		        {
			        GoRange.SetActive(false);
		        }
		        bool faceUp = isShow(cardValue);
		        var img = faceUp ? ResMgr.Instance.GetCardImg(cardValue) : ResMgr.Instance.GetCardBackImg();
		        ImgCard.sprite = img;
		        gameObject.SetActive(true);
		        transform.SetAsLastSibling();
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
	        isSelected             = false;
	        OutCard.effectColor    = Color.black;
	        OutCard.effectDistance = new Vector2(1, -1);
        }
        
        public void Selected()
        {
	        isSelected             = true;
	        OutCard.effectColor    = new Color(1,1,0.26f,1);
	        OutCard.effectDistance = new Vector2(3, -3);
        }
        public void AddTriggerEvent(EventTriggerType type, UnityAction<BaseEventData> action)
        {
	        var entry = TriggerCard.triggers.Find(item => item.eventID == type);
	        if (entry == null)
	        {
		        entry = new EventTrigger.Entry
		        {
			        eventID = type
		        };
		        entry.callback.AddListener(action);
		        TriggerCard.triggers.Add(entry);
	        }
	        else
	        {
		        entry.callback.AddListener(action);
	        }
        }
        public void AddTriggerEvent(EventTriggerType type, EventTrigger.TriggerEvent triggerEvent)
        {
	        var entry = TriggerCard.triggers.Find(item => item.eventID == type);
	        if (entry == null)
	        {
		        entry = new EventTrigger.Entry
		        {
			        eventID  = type,
			        callback = triggerEvent
		        };
		        TriggerCard.triggers.Add(entry);
	        }
	        else
	        {
		        entry.callback = triggerEvent;
	        }
        }
        
        public void CopyEventTrigger(CardItem newCard)
        {
	        if (TriggerCard.triggers is {Count:>0})
	        {
		        foreach (var entry in TriggerCard.triggers)
		        {
			        newCard.AddTriggerEvent(entry.eventID, entry.callback);
		        }
	        }
        }

        // private string GetRangeStr()
        // {
	       //  var realValue = (int)Value.Value;
	       //  var min       =  (CardValue)Math.Clamp( realValue - Random.Range(2,5) , (int)CardValue.A, (int)CardValue.K);
	       //  var max       =  (CardValue)Math.Clamp( realValue + Random.Range(2,5) , (int)CardValue.A, (int)CardValue.K);
	       //  return $"{min.ToShortStr()}|{max.ToShortStr()}";
        // }
    }
}
