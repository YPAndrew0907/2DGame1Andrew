using System.Collections;
using System.Threading;
using Mgr;
using TMPro;
using UI;
using UnityEngine;

namespace AttachMachine
{
    // 发牌状态（玩家）
    public class DealCardPlayerUIState : BaseGameUIState
    {
        IDealCardPlayerUIState  _dealCardPlayerUIState;
        public  override string StateID => StateIDStr;
        public const     string StateIDStr = "DealCardPlayerUIState";

        public override void OnCreate(GameSceneAiui sceneAiui)
        {
            if (sceneAiui is IShuffleUIState uiState)
            {
                _dealCardPlayerUIState = sceneAiui as IDealCardPlayerUIState;
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
    public interface IDealCardPlayerUIState
    {
        public GameObject DealCardPlayerUI { get;}
    }
}