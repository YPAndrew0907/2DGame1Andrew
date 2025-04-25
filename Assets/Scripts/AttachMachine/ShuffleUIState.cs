using System.Collections;
using System.Threading;
using Mgr;
using TMPro;
using UI;
using UnityEngine;

namespace AttachMachine
{
    // 洗牌动画
    public class ShuffleUIState : BaseGameUIState
    {
        IShuffleUIState        _shuffleUIState;
        public  override string StateID => StateIDStr;
        public const string      StateIDStr = "ShuffleUIState";

        public override void OnCreate(GameSceneAiui sceneAiui)
        {
            if (sceneAiui is IShuffleUIState uiState)
            {
                _shuffleUIState = sceneAiui as IShuffleUIState;
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
    public interface IShuffleUIState
    {
        public GameObject ShuffleUI { get;  }
    }
}