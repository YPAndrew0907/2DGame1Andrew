﻿using System;

namespace XYZFrameWork.Base
{
    public abstract class BaseSingle<T> where T: BaseSingle<T>, new()
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                }
                return _instance;
            }
        }
    }
}