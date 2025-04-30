using System.Collections;
using System.Threading;
using Mgr;
using TMPro;
using UI;
using UnityEngine;

namespace AttachMachine
{
    // 发牌状态（AI）
    public class DealCardUIState : BaseGameUIState
    {
        IDealCardUIState      _dealCardUIState;
        public  override string StateID => StateIDStr;
        public const     string StateIDStr = "DealCardUIState";

        public override void OnCreate(GameSceneUI sceneUI)
        {
            if (sceneUI is IShuffleUIState uiState)
            {
                _dealCardUIState = sceneUI as IDealCardUIState;
            }
            
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
    public interface IDealCardUIState
    {
        public DealCardAIUI DealCardAIUI { get;  }
        public DealCardPlayerUI DealCardPlayerUI { get;}
    }
}