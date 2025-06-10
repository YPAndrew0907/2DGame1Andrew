using System;
using System.Collections.Generic;
using Base;
using Obj;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class CardHeap : BaseViewMono
    {
        //AUTO-GENERATE
        private UnityEngine.RectTransform _rectCards;
        private UnityEngine.RectTransform RectCards
            => _rectCards ??= transform.Find("ViewPort/rect_cards").GetComponent<UnityEngine.RectTransform>();

        private TMPro.TextMeshProUGUI _txtTitleAndNum;
        private TMPro.TextMeshProUGUI TxtTitleAndNum
            => _txtTitleAndNum ??= transform.Find("txt_TitleAndNum").GetComponent<TMPro.TextMeshProUGUI>();

        //AUTO-GENERATE-END
        [Header("UI")]
        // public GameObject bgItems; // 牌太多的样式Go

        [Header("数据")]
        public string prefixTitle; // 显示 表达式 --》剩余：{0}张
        private Func<CardObj, bool> _isCardShowFunc;

        private List<CardObj> _cardDataList;  // 数据
        private List<CardItem> _cardGoList;    // 显示
        public CardItem cardItemPrefab; // 模板

        protected void Awake()
        {
            _cardDataList = new List<CardObj>();
            _cardGoList = new List<CardItem>();
            cardItemPrefab.gameObject.SetActive(false);
        }

        public List<CardObj> CardList => _cardDataList;
        public List<CardItem> CardItemList => _cardGoList;

        public void SetCard(IReadOnlyList<CardObj> cards, Func<CardObj, bool> func)
        {
            SetCard(cards, func, null);
        }

        // 支持点击事件注册
        public void SetCard(IReadOnlyList<CardObj> cards, Func<CardObj, bool> func, Action<int, CardItem> onClick)
        {
            _isCardShowFunc = func;
            ClearCard();
            _cardGoList ??= new List<CardItem>();
            // 保证CardItem数量
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
                    // 注册点击
                    if (onClick != null)
                    {
                        int idx = i;
                        _cardGoList[i].AddTriggerEvent(EventTriggerType.PointerClick, (eventData) => onClick(idx, _cardGoList[idx]));
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
            if (prefix != null)
            {
                prefixTitle = prefix;
            }
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
                foreach (var cardObj in cards)
                {
                    AddCard(cardObj);
                }
            }
            RefreshCard();
        }

        public void AddCard(CardObj cardObj)
        {
            if (_cardDataList != null)
            {
                cardObj.TimeTicks = DateTime.Now.Ticks + _cardDataList.Count;
                _cardDataList.Add(cardObj);
            }
        }
        private void AddGo()
        {
            var item = InstantiateCard();
            _cardGoList.Add(item);
        }

        public void ClearCard()
        {
            _cardDataList?.Clear();
        }
        public void RefreshCard()
        {
            var delta = _cardGoList.Count - _cardDataList.Count;
            var absDelta = Math.Abs(delta);
            if (delta < 0)
            {
                for (int i = absDelta; i >= 0; i--)
                {
                    AddGo();
                }
            }
            for (var i = 0; i < _cardGoList.Count; i++)
            {
                if (i < _cardDataList.Count)
                {
                    _cardGoList[i].SetCard(_cardDataList[i], _isCardShowFunc);
                }
                else
                {
                    _cardGoList[i].SetCard(null, _isCardShowFunc);
                }
            }
            TxtTitleAndNum.text = prefixTitle.Contains("{") ? string.Format(prefixTitle, _cardDataList.Count) : string.Format(prefixTitle);
        }
        public void RemoveAll()
        {
            _cardDataList = new List<CardObj>();
        }

        public int CardNum()
        {
            int i = 0;
            foreach (var card in _cardDataList)
            {
                i += (int)card.Value + 1;
            }
            return i;
        }

        private CardItem InstantiateCard(CardObj obj = null)
        {
            if (cardItemPrefab == null)
            {
                Debug.LogError("模板未设置");
                return null;
            }
            var go = Instantiate(cardItemPrefab, RectCards);
            go.gameObject.SetActive(false);
            var script = go.GetComponent<CardItem>();

            cardItemPrefab.CopyEventTrigger(script);

            if (obj != null)
            {
                script.SetCard(obj, _isCardShowFunc);
            }
            return go;
        }
    }
}
