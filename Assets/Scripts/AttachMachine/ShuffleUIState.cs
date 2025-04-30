using System.Collections;
using System.Runtime.Serialization;
using System.Threading;
using Mgr;
using TMPro;
using UI;
using UnityEngine;
using XYZFrameWork;

namespace AttachMachine
{
    // 洗牌动画
    public class ShuffleUIState : BaseGameUIState
    {
        IShuffleUIState        _shuffleUIState;
        public  override string StateID => StateIDStr;
        public const string      StateIDStr = "ShuffleUIState";

        public override void OnCreate(GameSceneUI sceneUI)
        {
            if (sceneUI is IShuffleUIState uiState)
            {
                _shuffleUIState = sceneUI as IShuffleUIState;
            }
            
        }
        
        public override IEnumerator OnEnterAsync(object payload)
        {
            NotifyMgr.Instance.SendEvent(NotifyDefine.SHUFFLE_START);
            CardMgr.Instance.Shuffle();
            var dealRole = DataMgr.Instance.CurShuffleRole;

            yield return _shuffleUIState.ShuffleUI.StartShuffle(dealRole);
            yield return OnExitAsync(null);
        }

        public override IEnumerator OnExitAsync(object payload)
        {
            NotifyMgr.Instance.SendEvent(NotifyDefine.SHUFFLE_END);
            DataMgr.Instance.NextDealRole();
            yield break;
        }

        public override void OnUpdate(float deltaTime)
        {
            
        }
    }
    public interface IShuffleUIState
    {
        public ShuffleUI ShuffleUI { get;  }
    }
}