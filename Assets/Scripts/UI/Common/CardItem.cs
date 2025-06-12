using System;
using Base;
using Cfg;
using Mgr;
using Obj;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI
{
    public class CardItem : BaseCardItem
    {
        //AUTO-GENERATE
        private UnityEngine.EventSystems.EventTrigger _triggerCard;
        private UnityEngine.EventSystems.EventTrigger TriggerCard
            => _triggerCard ??= transform.Find("trigger_out_img_card").GetComponent<UnityEngine.EventSystems.EventTrigger>();

        private UnityEngine.GameObject _goFlag;
        private UnityEngine.GameObject GoFlag
            => _goFlag ??= transform.Find("go_txt_Flag").gameObject;

        private UnityEngine.UI.Image _imgCard;
        private UnityEngine.UI.Image ImgCard
            => _imgCard ??= transform.Find("trigger_out_img_card").GetComponent<UnityEngine.UI.Image>();

        private UnityEngine.UI.Outline _outCard;
        private UnityEngine.UI.Outline OutCard
            => _outCard ??= transform.Find("trigger_out_img_card").GetComponent<UnityEngine.UI.Outline>();

        private TMPro.TextMeshProUGUI _txtFlag;
        private TMPro.TextMeshProUGUI TxtFlag
            => _txtFlag ??= transform.Find("go_txt_Flag").GetComponent<TMPro.TextMeshProUGUI>();

        //AUTO-GENERATE-END
           public override void SetCard(CardObj cardValue, Func<CardObj, bool> isShow)
        {
            if (IsSelected)
            {
                CancelSelect();
            }
            Value = cardValue;
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
                if (cardValue.IsCopy && cardValue.Owner == PlayerType.AI)
                {
                    GoFlag.SetActive(true);
                    TxtFlag.text = "C";
                }
                else
                {
                    GoFlag.SetActive(false);
                }
                bool faceUp = isShow(cardValue);
                var img = faceUp ? ResMgr.Instance.GetCardImg(cardValue) : ResMgr.Instance.GetCardBackImg();
                ImgCard.sprite = img;
                gameObject.SetActive(true);
                transform.SetAsLastSibling();
            }
        }
        public override void HideImg()
        {
            ImgCard.color = HideColor;
        }
        public override void ShowImg()
        {
            ImgCard.color = ShowColor;
        }

        public override void CancelSelect()
        {
            IsSelected = false;
            OutCard.effectColor = Color.black;
            OutCard.effectDistance = new Vector2(1, -1);
        }

        public override void Selected()
        {
            IsSelected = true;
            OutCard.effectColor = new Color(1, 1, 0.26f, 1);
            OutCard.effectDistance = new Vector2(3, -3);
        }
        public override void AddTriggerEvent(EventTriggerType type, UnityAction<BaseEventData> action)
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
        public override void AddTriggerEvent(EventTriggerType type, EventTrigger.TriggerEvent triggerEvent)
        {
            var entry = TriggerCard.triggers.Find(item => item.eventID == type);
            if (entry == null)
            {
                entry = new EventTrigger.Entry
                {
                    eventID = type,
                    callback = triggerEvent
                };
                TriggerCard.triggers.Add(entry);
            }
            else
            {
                entry.callback = triggerEvent;
            }
        }

        public override void CopyEventTrigger(BaseCardItem newCard)
        {
            if (TriggerCard.triggers is { Count: > 0 })
            {
                foreach (var entry in TriggerCard.triggers)
                {
                    newCard.AddTriggerEvent(entry.eventID, entry.callback);
                }
            }
        }
        
        public override void SetClickCallback(System.Action callback)
        {
            AddTriggerEvent(EventTriggerType.PointerClick, _ => callback?.Invoke());
        }
        
        // private string GetRangeStr()
        // {
        //     var realValue = (int)Value.Value;
        //     var min       =  (CardValue)Math.Clamp( realValue - Random.Range(2,5) , (int)CardValue.A, (int)CardValue.K);
        //     var max       =  (CardValue)Math.Clamp( realValue + Random.Range(2,5) , (int)CardValue.A, (int)CardValue.K);
        //     return $"{min.ToShortStr()}|{max.ToShortStr()}";
        // }
    }
}
