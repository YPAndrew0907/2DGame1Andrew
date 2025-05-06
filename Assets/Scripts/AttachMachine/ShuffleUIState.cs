using System.Collections;
using Mgr;
using UI;

namespace AttachMachine
{
    // 洗牌动画
    public class ShuffleUIState : BaseGameUIState
    {
        IShuffleUIState        _shuffleUIState;
        public  override string StateID => StateIDStr;
        public const string      StateIDStr = "ShuffleUIState";

        public override void OnCreate(IMachineMaster sceneUI)
        {
            if (sceneUI is IShuffleUIState uiState)
            {
                _shuffleUIState = uiState;
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
            DataMgr.Instance.NextShuffleRole();
            yield return null;
            _shuffleUIState.AttachMachine.StartMachine(DealCardUIState.StateIDStr);
        }

        public override void OnUpdate(float deltaTime)
        {
            
        }
    }
    public interface IShuffleUIState:IBaseAttachUI
    {
        public ShuffleUI ShuffleUI { get;  }
    }
}