﻿using System;
using System.Collections.Generic;
using Cfg;
using UnityEngine;
using XYZFrameWork;
using XYZFrameWork.Base;

namespace Mgr
{
    public class NotifyMgr : BaseSingle<NotifyMgr>
    {
        private long _eventUid;
        private long EventUid => _eventUid++;

        private static readonly Dictionary<long, HashSet<Action<NotifyMsg>>> Listener = new();

        public NotifyMgr()
        {
            Init();
        }

        public void Init()
        {
            _eventUid = 1;
        }

        ~NotifyMgr()
        {
            Listener.Clear();
        }

        public static void SendEvent(NotifyMsg msg)
        {
            if (Listener.TryGetValue(msg.EventID, out var listeners))
            {
                foreach (var listener in listeners)
                {
                    listener?.Invoke(msg);
                }

                msg.Release();
            }
        }

        public static void RegisterNotify(long eventId, Action<NotifyMsg> action)
        {
            if (!Listener.TryGetValue(eventId, out var actions))
            {
                actions           = new HashSet<Action<NotifyMsg>>();
                Listener[eventId] = actions;
            }

            actions.Add(action);
        }

        public static void UnRegisterNotify(long eventId, Action<NotifyMsg> action)
        {
            if (Listener.TryGetValue(eventId, out var actions))
            {
                actions.Remove(action);
            }
        }
        public static void RegisterNotify(Dictionary<int, Action<NotifyMsg>> eventDic)
        {
            foreach (var (eventId, action) in eventDic)
            {
                RegisterNotify(eventId, action);
            }
        }

        public static void UnRegisterNotify(Dictionary<int, Action<NotifyMsg>> eventDic)
        {
            foreach (var (eventId, action) in eventDic)
            {
                UnRegisterNotify(eventId, action);
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="param">只能是 int, long, float, double, INotifyValue, ArrayList, </param>
        public static void SendEvent(int eventId, params object[] param)
        {
            var eventMsg = CreateEventMsg(eventId);

            if (param.Length == 0)
            {
                SendEvent(eventMsg);
                return;
            }

            Type firstType = param[0].GetType();
            if (param.Length == 1)
            {
                // 单个的类， 只能用 == 不能用IsAssignableFrom 或者 is 判断符
                if (!firstType.IsValueType && firstType != typeof(string))
                {
                    var paramObj = NotifyParamMgr.CreateNotifyParam<CustomParam>();
                    paramObj.SetValue(param[0]);
                    eventMsg.Param = paramObj;
                }
                else
                {
                    var paramObj = NotifyParamMgr.CreateNotifyParam<NormalParam>();
                    eventMsg.Param = paramObj;
                    if (firstType == typeof(int))
                        paramObj.SetValue((int)param[0]);
                    else if (firstType == typeof(long))
                        paramObj.SetValue((long)param[0]);
                    else if (firstType == typeof(float))
                        paramObj.SetValue((float)param[0]);
                    else if (firstType == typeof(double))
                        paramObj.SetValue((double)param[0]);
                    else if (firstType == typeof(string))
                        paramObj.SetValue((string)param[0]);
                    else if (firstType == typeof(bool))
                        paramObj.SetValue((bool)param[0]);
                    else
                        Debug.LogError(LogTxt.PARAM_ERROR);
                }
            }
            else
            {
                var paramObj = NotifyParamMgr.CreateNotifyParam<CustomParam>();
                paramObj.SetValue(param);
                eventMsg.Param = paramObj;
            }

            SendEvent(eventMsg);
        }
        
        public static NotifyMsg CreateEventMsg(int eventId)
        {
            return new NotifyMsg(eventId, Instance.EventUid);
        }

        public static NotifyMsg CreateEventMsg<T>(int eventId, T value)
        {
            var param = NotifyParamMgr.CreateNotifyParam(value);
            return new NotifyMsg(eventId, Instance.EventUid) { Param = param };
        }
    }
}