using System;
using System.Collections;
using UnityEngine;

namespace XYZFrameWork.Base
{
    public class BaseMonoSingleton<T> : BaseMono where T: BaseMono
    {
        private static T _instance;

        public static T Instance => _instance;

        protected virtual void Awake()
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
            OnAwake();
        }

        protected virtual void OnAwake()
        {
            
        }
    }
}