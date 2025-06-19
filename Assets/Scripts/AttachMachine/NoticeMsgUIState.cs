using System.Collections;
using Mgr;
using UI;
using XYZFrameWork;

namespace AttachMachine
{
    public class NoticeMsgUIState : BaseGameUIState
    {
        public override string          StateID => StateIDStr;
        public const    string          StateIDStr = "NoticeMsgUIState";
        private         INoticeMsgState _noticeMsgState;
        public override void        OnCreate(IMachineMaster sceneUI)
        {
            if (sceneUI is INoticeMsgState ui)
            {
                _noticeMsgState = ui;
                _noticeMsgState.NoticeMsgUI.Init();
                NotifyMgr.RegisterNotify(NotifyDefine.NOTICE,Notice);
            }
        }

        public override IEnumerator OnEnterAsync(object payload)
        {
            
            yield break;
        }

        public override IEnumerator OnExitAsync(object payload)
        {
            yield break;
        }

        public override void        OnUpdate(float deltaTime)
        {
            
        }

        private void Notice(NotifyMsg msg)
        {
            if (msg.Param is not NormalParam param)
                return;
            
            var str = param.StrValue;
            _noticeMsgState.NoticeMsgUI.Notice(str);
        }
    }

    public interface INoticeMsgState: IBaseAttachUI
    {
        NoticeMsgUI NoticeMsgUI { get; }
    }
}