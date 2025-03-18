using UnityEngine;

namespace XYZFrameWork
{
   public struct NotifyMsg
    {
        public readonly int FrameCount;
        public readonly int EventID;
        public readonly long EventUid;

        public INotifyParam Param;

        public NotifyMsg(int eventID, long eventUid) : this()
        {
            FrameCount = Time.frameCount;
            EventID = eventID;
            EventUid = eventUid;
        }

        public void Release()
        {
            if (Param == null) return;
            NotifyParamMgr.Recycle(Param);
            Param = null;
        }
    }
}