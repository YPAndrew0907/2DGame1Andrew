using System;
using Base;
using UnityEngine;

namespace XYZFrameWork.Base
{
    // 可被自动化脚本捕捉的基类
    public abstract class BaseViewMono : BaseMono
    {
        private bool _hasFindUI;
        /// <summary>
        /// 查找UI 一般不需要主动调用，都在对应 View 脚本中调用。
        /// 除非直接继承的 BaseViewMono
        /// </summary>
        protected virtual void FindUI()
        {
        }

        private void Awake()
        {
            FindUI();
            OnAwake();
        }

        protected virtual void OnAwake()
        {
            
        }
    }
}