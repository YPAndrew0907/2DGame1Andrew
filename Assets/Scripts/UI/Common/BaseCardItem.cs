using System;
using Base;
using Obj;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI
{
    public abstract class BaseCardItem: BaseViewMono
    {
        protected const string  DefaultName = "CardItemPrefab (Empty)";
        public     CardObj Value { get; protected set; }
        protected        bool    IsSelected;

        protected readonly Color HideColor = new Color(0, 0, 0, 0);
        protected readonly Color ShowColor = new Color(1, 1, 1, 1);

        public abstract void CancelSelect();

        public abstract void Selected();

        public abstract void HideImg();

        public abstract void ShowImg();
        
        public   virtual       CardZone CurrentZone { get; set; }
        public abstract void     AddTriggerEvent(EventTriggerType type, UnityAction<BaseEventData> action);

        public abstract void AddTriggerEvent(EventTriggerType type, EventTrigger.TriggerEvent triggerEvent);
        public abstract void SetCard(CardObj cardValue, Func<CardObj, bool> isShow);

        public abstract void CopyEventTrigger(BaseCardItem newCard);

        public virtual void SetClickCallback(System.Action callback)
        {
            
        }
    }
}