using System.Collections;
using System.Linq;
using Mgr;
using Obj;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using XYZFrameWork;

namespace AttachMachine
{
    public class ShuffleUIState : BaseGameUIState
    {
        private         IShuffleUIState _shuffleUIState;
        public override string          StateID => StateIDStr;
        public const    string          StateIDStr = "ShuffleUIState";

        private bool _rememberIsOpen;
        public override void OnCreate(IMachineMaster sceneUI)
        {
            if (sceneUI is IShuffleUIState ui)
            {
                _shuffleUIState = ui;
                DataMgr.Instance.NextShuffleRole();
                
                NotifyMgr.RegisterNotify(NotifyDefine.SKILL_CARD_SELECT,OnRememberSelectCard);
            }
        }

        public override IEnumerator OnEnterAsync(object payload)
        {
            if (DataMgr.Instance.WillShuffle())
            {
                CardMgr.Instance.ResetCards();
                if (DataMgr.Instance.CurSkills.Contains(PlayerSkill.Remember))
                {
                    _shuffleUIState.GuessOrRememberUI.ShowSelectCardPanel();
                    _rememberIsOpen = true;
                }

                yield return new WaitUntil(() => !_rememberIsOpen );
                yield return new WaitForSeconds(1);
                
                var shuffleRole = DataMgr.Instance.CurShuffleRole;
                yield return _shuffleUIState.ShuffleUIState.CorShuffleStart();
                yield return _shuffleUIState.ShuffleUIState.StartShuffle(shuffleRole);
                
                CardMgr.Instance.Shuffle();
                DataMgr.Instance.TurnCounter(true);
                var list = CardMgr.Instance.Cards.ToList();
                _shuffleUIState.ShuffleUIState.SetCard(list.ToList());
                
                XAttachMachine.ExitState(StateIDStr,0);
            }
            else
            {
                XAttachMachine.ExitState(StateIDStr);
            }
        }

        public override IEnumerator OnExitAsync(object payload)
        {
            // 是否洗牌
            if (payload != null)
            {
                CoroutineMgr.Instance.StartCoroutine(_shuffleUIState.ShuffleUIState.CorShuffleEnd());
                DataMgr.Instance.NextShuffleRole();
            }
            
            yield return XAttachMachine.EnterState(DealCardUIState.StateIDStr,1);
        }

        public override void OnUpdate(float deltaTime)
        {
            
        }

        private void OnRememberSelectCard(NotifyMsg obj)
        {
            _rememberIsOpen = false;
            _shuffleUIState.GuessOrRememberUI.Hide();
        }
    }
    public interface IShuffleUIState: IBaseAttachUI
    {
        public TotalCardHeapUI   ShuffleUIState    { get;  }
        public GuessOrRememberUI GuessOrRememberUI { get; }
    }
}