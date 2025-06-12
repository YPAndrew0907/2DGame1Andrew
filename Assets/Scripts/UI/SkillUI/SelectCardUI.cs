using System.Collections.Generic;
using System.Linq;
using Base;
using Mgr;
using Obj;
using UnityEngine;
using UnityEngine.EventSystems;
namespace UI
{
    public class SelectCardUI : BaseViewMono
    {
    	//AUTO-GENERATE
    	private UnityEngine.UI.Button _btnCheckSelect;
    	private UnityEngine.UI.Button BtnCheckSelect 
    			=> _btnCheckSelect ??= transform.Find("go_bg/btn_CheckSelect").GetComponent<UnityEngine.UI.Button>();

    	private UI.CardZone _monoSelectCardZone;
    	private UI.CardZone MonoSelectCardZone 
    			=> _monoSelectCardZone ??= transform.Find("go_bg/mono_SelectCardHeap").GetComponent<UI.CardZone>();

    	private UnityEngine.GameObject _goBg;
    	private UnityEngine.GameObject GoBg 
    			=> _goBg ??= transform.Find("go_bg").gameObject;

    	//AUTO-GENERATE-END
        
        public BaseCardItem        CardItemPrefab;
		
        private int           _maxCardCount;
        private int           _selectCardId;
        private List<CardObj> _termSelectCard;
        
        public void Init()
        {
            GoBg.SetActive(false);
            _termSelectCard = new List<CardObj>();
			         
            BtnCheckSelect.onClick.RemoveAllListeners();
            BtnCheckSelect.onClick.AddListener(OnCheckSelect);
			         
            CardItemPrefab.AddTriggerEvent(EventTriggerType.PointerClick, SelectCard);
        }
        
        public void Show(string titleStr, int selectEventId, int maxSelectCount)
        {
            
            _selectCardId = selectEventId;
            _termSelectCard.Clear();
            _maxCardCount = maxSelectCount;
            
            MonoSelectCardZone.SetCard(CardMgr.Instance.Cards, CardMgr.IsCardShowSelectCard);
            MonoSelectCardZone.RefreshTitle(titleStr);
            GoBg.SetActive(true);
        }
        
        public void Hide()
        {
            GoBg.SetActive(false);
        }
        
        private void OnCheckSelect()
        {
            if (_termSelectCard.Count > 0)
            {
                NotifyMgr.SendEvent(_selectCardId,  new OperationData()
                {
                    IsAI = false,
                    SelectCards = _termSelectCard
                });
                _termSelectCard.Clear();
                Hide();
            }
        }
        
        public void SelectCard(BaseEventData eventData)
        {
            if (eventData is PointerEventData pointerEventData)
            {
                var      first = pointerEventData.pointerCurrentRaycast.gameObject.transform.parent;
                CardItem item  = first.GetComponent<CardItem>();
                // Debug.Log(item.Value);
                if (item != null)
                {
                    if (_termSelectCard.Contains(item.Value))
                    {
                        item.CancelSelect();
                        _termSelectCard.Remove(item.Value);
                    }
                    else
                    {
                        if (_maxCardCount<0 || _termSelectCard.Count < _maxCardCount)
                        {
                            _termSelectCard.Add(item.Value);
                            item.Selected();
                        }
                    }
                }
            }
        }
    }
}
