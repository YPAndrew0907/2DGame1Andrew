using System.Collections;
using System.Threading;
using Mgr;
using TMPro;
using UI;
using UnityEngine;

namespace AttachMachine
{
    // 发牌状态（AI）
    public class DealCardAIUIState : BaseGameUIState
    {
        IDealCardAIUIState      _dealCardAIuiState;
        public  override string StateID => StateIDStr;
        public const     string StateIDStr = "DealCardAIUIState";

        public override void OnCreate(GameSceneAiui sceneAiui)
        {
            if (sceneAiui is IShuffleUIState uiState)
            {
                _dealCardAIuiState = sceneAiui as IDealCardAIUIState;
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
    public interface IDealCardAIUIState
    {
        public GameObject DealCardAIUI { get;  }
    }
}