using System;
using System.Collections.Generic;
using UnityEngine;
using XYZFrameWork.Base;

namespace XYZFrameWork
{
    public class NotifyParamMgr : BaseSingle<NotifyParamMgr>
    {
        private long _paramUid;
        private long ParamUid => _paramUid++;

        private readonly LinkedList<INotifyParam>      _totalParams           = new();
        private readonly Dictionary<long, NormalParam> _activeNormalParams    = new();
        private readonly Dictionary<long, CustomParam> _activeCustomParams    = new();
        private readonly Queue<NormalParam>            _availableNormalParams = new();
        private readonly Queue<CustomParam>            _availableCustomParams = new();

        public void Init()
        {
            _paramUid = 1;
        }

        private void Clear()
        {
            foreach (var param in _totalParams)
            {
                Recycle(param);
            }

            _totalParams.Clear();
            _availableNormalParams.Clear();
            _availableCustomParams.Clear();
            _activeNormalParams.Clear();
            _activeCustomParams.Clear();
        }

        ~NotifyParamMgr()
        {
            Clear();
        }

        public static void Recycle(INotifyParam param)
        {
            if (param == null)
                return;

            param.Release();

            if (param is NormalParam normalParam)
            {
                Instance._activeNormalParams.Remove(param.ParmaUid);
                Instance._availableNormalParams.Enqueue(normalParam);
            }
            else if (param is CustomParam customParam)
            {
                Instance._activeCustomParams.Remove(param.ParmaUid);
                Instance._availableCustomParams.Enqueue(customParam);
            }
        }

        private T PopEventParam<T>() where T : class, INotifyParam
        {
            var uid = ParamUid;

            if (typeof(T) == typeof(NormalParam))
            {
                var param = _availableNormalParams.Count > 0 ? _availableNormalParams.Dequeue() : new NormalParam();
                param.ParmaUid           = uid;
                _activeNormalParams[uid] = param;
                _totalParams.AddLast(param);
                return param as T;
            }

            var customParam = _availableCustomParams.Count > 0 ? _availableCustomParams.Dequeue() : new CustomParam();
            customParam.ParmaUid     = uid;
            _activeCustomParams[uid] = customParam;
            _totalParams.AddLast(customParam);
            return customParam as T;
        }

        public static T CreateNotifyParam<T>() where T : class, INotifyParam
        {
            return Instance.PopEventParam<T>();
        }

        public static INotifyParam CreateNotifyParam(int value)    => CreateNormalParam(value);
        public static INotifyParam CreateNotifyParam(long value)   => CreateNormalParam(value);
        public static INotifyParam CreateNotifyParam(string value) => CreateNormalParam(value);
        public static INotifyParam CreateNotifyParam(float value)  => CreateNormalParam(value);
        public static INotifyParam CreateNotifyParam(double value) => CreateNormalParam(value);
        public static INotifyParam CreateNotifyParam(object value) => CreateCustomParam(value);

        private static INotifyParam CreateNormalParam<T>(T value)
        {
            var param = Instance.PopEventParam<NormalParam>();
            param.SetValue(value);
            return param;
        }

        public static INotifyParam CreateCustomParam(object value)
        {
            var param = Instance.PopEventParam<CustomParam>();
            param.SetValue(value);
            return param;
        }
    }
}