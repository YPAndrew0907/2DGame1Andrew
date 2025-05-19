using System.Collections;
using Mgr;
using Obj;
using UI;

namespace AttachMachine
{
    public class ShuffleUIState : BaseGameUIState
    {
        private         IShuffleUIState _shuffleUIState;
        public override string          StateID => StateIDStr;
        public const    string          StateIDStr = "ShuffleUIState";

        public override void OnCreate(IMachineMaster sceneUI)
        {
            if (sceneUI is IShuffleUIState ui)
            {
                _shuffleUIState = ui;
                DataMgr.Instance.NextShuffleRole();
            }
        }

        public override IEnumerator OnEnterAsync(object payload)
        {
            if (DataMgr.Instance.WillShuffle())
            {
                NotifyMgr.Instance.SendEvent(NotifyDefine.SHUFFLE_START);
                CardMgr.Instance.Shuffle();
                DataMgr.Instance.TurnCounter(true);
                var shuffleRole = DataMgr.Instance.CurShuffleRole;
                yield return _shuffleUIState.ShuffleUIState.StartShuffle(shuffleRole);
                yield return OnExitAsync(0);
            }
            else
            {
                yield return OnExitAsync(null);
            }
        }

        public override IEnumerator OnExitAsync(object payload)
        {
            // 是否洗牌
            if (payload != null)
            {
                NotifyMgr.Instance.SendEvent(NotifyDefine.SHUFFLE_END);
                DataMgr.Instance.NextShuffleRole();
            }

            var cards = CardMgr.Instance.GetCards(4);

            // 首次发牌
            cards[0].IsFaceUp = true;
            cards[1].IsFaceUp = true;

            for (int i = 0; i < cards.Count; i++)
            {
                if (i % 2 != 0)
                    cards[i].Owner = PlayerType.Player;
                else
                    cards[i].Owner = PlayerType.AI;
            }
            
            yield return _shuffleUIState.AttachMachine.EnterState(DealCardUIState.StateIDStr, cards);
        }

        public override void OnUpdate(float deltaTime)
        {
            
        }
    }
    public interface IShuffleUIState: IBaseAttachUI
    {
        public TotalCardHeapUI ShuffleUIState { get;  }
    }
}