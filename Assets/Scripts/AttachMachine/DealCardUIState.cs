using System.Collections;
using System.Threading;
using Mgr;
using TMPro;
using UI;
using UnityEngine;

namespace AttachMachine
{
    // 发牌状态
    public class DealCardUIState : BaseGameUIState
    {
        IDealCardUIState      _dealCardUIState;
        public  override string StateID => StateIDStr;
        public const     string StateIDStr = "DealCardUIState";

        public override void OnCreate(IMachineMaster sceneUI)
        {
            if ( sceneUI is IDealCardUIState ui)
            {
                _dealCardUIState = ui;
            }

            _dealCardUIState.DealCardPlayerUI.Init();
            _dealCardUIState.DealCardAIUI.Init();
        }
        
        public override IEnumerator OnEnterAsync(object payload)
        {
            NotifyMgr.Instance.SendEvent(NotifyDefine.SHUFFLE_START);
            yield break;
        }

        public override IEnumerator OnExitAsync(object payload)
        {
            NotifyMgr.Instance.SendEvent(NotifyDefine.SHUFFLE_END);
            yield break;
        }

        public override void OnUpdate(float deltaTime)
        {
            
        }
        
    }
    public interface IDealCardUIState:IBaseAttachUI
    {
        public ShuffleUI ShuffleUI { get;  }
        public DealCardAIUI DealCardAIUI { get;  }
        public DealCardPlayerUI DealCardPlayerUI { get;}
    }
}