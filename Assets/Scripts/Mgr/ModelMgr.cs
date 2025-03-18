using System;
using System.Collections.Generic;
using Cfg;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.iOS;
using XYZFrameWork.Base;

namespace Mgr
{
    public class ModelMgr : BaseSingle<ModelMgr>
    {
        private Dictionary<Type,IModel> _modelDic = new Dictionary<Type, IModel>();
        public bool AddModel(Type modelType, params object[] args)
        {
            if (!_modelDic.TryGetValue(modelType, out var model))
            {
                model = Activator.CreateInstance(modelType, args) as IModel;
                _modelDic.Add(modelType,model);
            }
            return true;
        }

        public static T GetModel<T>() where T : class, IModel
        {
            if (Instance._modelDic.TryGetValue(typeof(T), out var model))
            {
                return model as T;
            }
            else
            {
                Debug.LogError(LogTxt.NOT_EXIT_ERROR);
                return null;
            }
        }

        #region interface

        public interface IModel
        {
            public void ReSet();
        }
        

        #endregion
    }
}