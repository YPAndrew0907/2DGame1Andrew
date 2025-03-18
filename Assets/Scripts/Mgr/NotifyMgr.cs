using System;
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

        public static void Register(long eventId, Action<NotifyMsg> action)
        {
            if (!Listener.TryGetValue(eventId, out var actions))
            {
                actions           = new HashSet<Action<NotifyMsg>>();
                Listener[eventId] = actions;
            }

            actions.Add(action);
        }

        public static void UnRegister(long eventId, Action<NotifyMsg> action)
        {
            if (Listener.TryGetValue(eventId, out var actions))
            {
                actions.Remove(action);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="param">只能是 int, long, float, double, INotifyValue, ArrayList, </param>
        public void SendEvent(int eventId, params object[] param)
        {
            var eventMsg = NotifyMgr.CreateEventMsg(eventId);

            if (param.Length == 0)
            {
                NotifyMgr.SendEvent(eventMsg);
                return;
            }

            Type firstType = param[0].GetType();
            if (param.Length == 1)
            {
                // 单个的类， 只能用 == 不能用IsAssignableFrom 或者 is 判断符
                if (firstType == typeof(object))
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

            NotifyMgr.SendEvent(eventMsg);
        }

        public void RegisterEvent(int eventId, Action<NotifyMsg> onEvent)
        {
            NotifyMgr.Register(eventId, onEvent);
        }

        public void UnRegisterEvent(int eventId, Action<NotifyMsg> onEvent)
        {
            NotifyMgr.UnRegister(eventId, onEvent);
        }

        public void RegisterEvent(Dictionary<int, Action<NotifyMsg>> eventDic)
        {
            foreach (var (eventId, action) in eventDic)
            {
                RegisterEvent(eventId, action);
            }
        }

        public void UnRegisterEvent(Dictionary<int, Action<NotifyMsg>> eventDic)
        {
            foreach (var (eventId, action) in eventDic)
            {
                UnRegisterEvent(eventId, action);
            }
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