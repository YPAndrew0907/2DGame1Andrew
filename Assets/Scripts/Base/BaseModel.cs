using System;
using System.Collections.Generic;
using Mgr;
using XYZFrameWork;

namespace Base
{
    public abstract class BaseModel: ModelMgr.IModel
    {
        private Action<object> _valueChange;
       
        public BaseModel(params object[] args)
        {
            Init(args);
        }

        private void Init(object[] args)
        {
            OnInit(args);
        }
        
        private void NotifyValueChange(object obj)
        {
            _valueChange?.Invoke(obj);   
            OnValueChange();
        }

        private void Dispose()
        {
            OnDispose();
        }

        protected abstract void OnInit(object[] args);
        protected abstract void OnValueChange();
        protected abstract void OnDispose();
        

        public void AddValueChange(Action<object> valueChange)
        {
            _valueChange+= valueChange;
        }
        public void RemoveValueChange(Action<object> valueChange)
        {
            _valueChange-= valueChange;
        }

        public abstract void ReSet();
    }
}