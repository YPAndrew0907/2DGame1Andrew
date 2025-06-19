using System;
using System.Collections.Generic;
using Base;
using Mgr;
using Obj;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class CardZone : BaseViewMono, IDropHandler
    {
    	//AUTO-GENERATE
    	private UnityEngine.RectTransform _rectCards;
    	private UnityEngine.RectTransform RectCards 
    			=> _rectCards ??= transform.Find("ViewPort/rect_cards").GetComponent<UnityEngine.RectTransform>();

    	private TMPro.TextMeshProUGUI _txtTitleAndNum;
    	private TMPro.TextMeshProUGUI TxtTitleAndNum 
    			=> _txtTitleAndNum ??= transform.Find("txt_TitleAndNum").GetComponent<TMPro.TextMeshProUGUI>();

    	//AUTO-GENERATE-END
        [FormerlySerializedAs("drappable")]
        [Header("配置项")] 
        public bool                droppable = false;
        public           string              prefixTitle = "剩余：{0}张";
        public           BaseCardItem        cardItemPrefab;
        private          Func<CardObj, bool> _isCardShowFunc;
        private readonly List<CardObj>       _cardDataList = new();
        private readonly List<BaseCardItem>  _cardGoList   = new();
        public           List<CardObj>       CardList     => _cardDataList;
        public           List<BaseCardItem> CardItemList => _cardGoList;
        public void OnEnable()
        {
            if (cardItemPrefab != null && cardItemPrefab.gameObject.activeSelf)
            {
                cardItemPrefab.gameObject.SetActive(false);
            }
        }
        public void SetCard(IReadOnlyList<CardObj> cards, Func<CardObj, bool> func, Action<int, BaseCardItem> onClick = null)
        {
            _isCardShowFunc = func;
            ClearCard();
            while (_cardGoList.Count < (cards?.Count ?? 0))
            {
                AddGo();
            }
            for (int i = 0; i < _cardGoList.Count; i++)
            {
                if (i < (cards?.Count ?? 0))
                {
                    _cardGoList[i].SetCard(cards[i], _isCardShowFunc);
                    _cardGoList[i].gameObject.SetActive(true);
                    _cardGoList[i].CurrentZone = this;
                    if (onClick != null)
                    {
                        int idx = i;
                        _cardGoList[i].SetClickCallback(() => onClick(idx, _cardGoList[idx]));
                    }
                }
                else
                {
                    _cardGoList[i].SetCard(null, _isCardShowFunc);
                    _cardGoList[i].gameObject.SetActive(false);
                }
            }
            _cardDataList.Clear();
            if (cards != null)
                _cardDataList.AddRange(cards);
            RefreshTitle();
        }
        
        
        public void RefreshTitle(string prefix = null)
        {
            if (prefix != null) prefixTitle = prefix;
            TxtTitleAndNum.text = prefixTitle.Contains("{") ? string.Format(prefixTitle, _cardDataList.Count) : prefixTitle;
        }
        public void AddCard(IReadOnlyList<CardObj> cards)
        {
            if (cards == null)
            {
                ClearCard();
            }
            else
            {
                foreach (var card in cards)
                    AddCard(card);
            }
            RefreshCard();
        }
        public void AddCard(CardObj cardObj,int idx = -1)
        {
            cardObj.TimeTicks = DateTime.Now.Ticks + _cardDataList.Count;
            if (idx >= 0)
                _cardDataList.Insert(idx, cardObj);
            else 
                _cardDataList.Add(cardObj);
        }
        public void ClearCard()
        {
            for (int i = 0; i < _cardGoList.Count; i++)
            {
                var item = _cardGoList[i];
                if (item != null)
                {
                    Destroy(item.gameObject);
                    _cardGoList[i] = null;
                }
            }
            _cardDataList.Clear();
            _cardGoList.Clear();
        }
        public void RefreshCard()
        {
            int delta = _cardGoList.Count - _cardDataList.Count;
            if (delta < 0)
            {
                for (int i = 0; i < -delta; i++)
                    AddGo();
            }
            for (int i = 0; i < _cardGoList.Count; i++)
            {
                var item = _cardGoList[i];
                
                if (i < _cardDataList.Count)
                {
                    item.SetCard(_cardDataList[i], _isCardShowFunc);
                    item.CurrentZone = this;
                }
                else
                {
                    item.SetCard(null, _isCardShowFunc);
                }
            }
            RefreshTitle();
        }
        public int CardNum()
        {
            int sum = 0;
            foreach (var card in _cardDataList)
                sum += (int)card.Value + 1;
            return sum;
        }
        private void AddGo()
        {
            var item = InstantiateCard();
            _cardGoList.Add(item);
        }
        private BaseCardItem InstantiateCard(CardObj obj = null)
        {
            var go = Instantiate(cardItemPrefab, RectCards);
            go.gameObject.SetActive(false);
            var script = go.GetComponent<BaseCardItem>();
            cardItemPrefab.CopyEventTrigger(script);
            if (obj != null)
                script.SetCard(obj, _isCardShowFunc);
            return script;
        }
        // 在 CardZone.OnDrop 中：
        public void OnDrop(PointerEventData eventData)
        {
            if (!droppable || eventData.pointerDrag == null) return;
            if (eventData.pointerDrag.transform.parent.TryGetComponent(out DraggableCard draggedCard))
            {
                if (draggedCard.CurrentZone != this)
                {
                    var data= new InsertMoveData()
                    {
                        TargetCard = draggedCard.Value,
                        FromZone = draggedCard.CurrentZone,
                        FromIdx = draggedCard.CurrentZone.CardList.IndexOf(draggedCard.Value),
                        ToZone = this,
                        ToIdx = -1
                    };
                    
                    draggedCard.CurrentZone.CardList.Remove(draggedCard.Value);
                    AddCard(draggedCard.Value);
                    // draggedCard.CurrentZone = this;
                    
                    NotifyMgr.SendEvent(NotifyDefine.MOVE_CARD, data);
                    StartCoroutine(DelayRefreshCard(data.FromZone));
                }
            }
        }
        private System.Collections.IEnumerator DelayRefreshCard(CardZone lastZone)
        {
            yield return null; // 等待一帧，让 OnEndDrag 执行完
            lastZone?.RefreshCard();
            RefreshCard();
            GetComponent<ScrollRect>().verticalNormalizedPosition = 0;
        }
    }
}
