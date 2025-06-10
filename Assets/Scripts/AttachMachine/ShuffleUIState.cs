using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mgr;
using Obj;
using UI;
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
        private bool _stealIsOpen;

        public override void OnCreate(IMachineMaster sceneUI)
        {
            if (sceneUI is IShuffleUIState ui)
            {
                _shuffleUIState = ui;
                GameSessionMgr.Instance.NextShuffleRole();
                NotifyMgr.RegisterNotify(NotifyDefine.SELECT_CARD_REMEMBER, OnRememberSelectCard);
                NotifyMgr.RegisterNotify(NotifyDefine.SELECT_CARD_STEAL, OnStealSelectCard);
            }
        }

        public override void OnActive()
        {
            base.OnActive();
            _stealIsOpen    = false;
            _rememberIsOpen = false;
        }

        public override IEnumerator OnEnterAsync(object payload)
        {
            _shuffleUIState.LevelInfoUI.SetCurRound(GameSessionMgr.Instance.RoundTimes);
            if (GameSessionMgr.Instance.WillShuffle)
            {
                var shuffleRole = GameSessionMgr.Instance.CurShuffleRole;
                CardMgr.Instance.ResetCards();
                _shuffleUIState.PlayedCardUI.ClearCards();
                

                if (shuffleRole == PlayerType.Player
                    && GameSessionMgr.Instance.CurPlayerSkills.Contains(PlayerSkill.StealAndInsert))
                {
                    _shuffleUIState.SelectCardUI.Show("选择需要偷取的牌", NotifyDefine.SELECT_CARD_STEAL, 1);
                    _stealIsOpen = true;
                    yield return new WaitUntil(() => !_stealIsOpen);
                    yield return new WaitForSeconds(1);
                }
                else if (shuffleRole == PlayerType.AI
                         && GameSessionMgr.Instance.CurBossSkills.Contains(PlayerSkill.StealAndInsert))
                {
                    var (_, stealCount) = SkillMgr.Instance.GetSkillParameters(PlayerSkill.StealAndInsert);
                    var copyIndexList = AIMgr.AIRandomStealCard(CardMgr.Instance.Cards, (int)stealCount);
                    var cardList      = CardMgr.Instance.StealCard(copyIndexList);
                    NotifyMgr.SendEvent(NotifyDefine.SELECT_CARD_STEAL, new SelectCardData
                    {
                        IsAI        = true,
                        SelectCards = cardList
                    });
                }

                if (shuffleRole == PlayerType.Player
                    && GameSessionMgr.Instance.CurPlayerSkills.Contains(PlayerSkill.Remember))
                {
                    _shuffleUIState.SelectCardUI.Show("选择需要记忆的牌", NotifyDefine.SELECT_CARD_REMEMBER, 1);
                    _rememberIsOpen = true;
                    yield return new WaitUntil(() => !_rememberIsOpen);
                    yield return new WaitForSeconds(1);
                }

                yield return _shuffleUIState.ShuffleUI.CorShuffleStartAni(shuffleRole);

                CardMgr.Instance.Shuffle();
                var list = CardMgr.Instance.Cards.ToList();
                _shuffleUIState.ShuffleUI.SetCard(list);

                XAttachMachine.ExitState(StateIDStr, 0);
            }
            else
            {
                _shuffleUIState.LevelInfoUI.SetCurRound(GameSessionMgr.Instance.RoundTimes);
                XAttachMachine.ExitState(StateIDStr);
            }
        }

        public override IEnumerator OnExitAsync(object payload)
        {
            // 是否洗牌
            if (payload != null)
            {
                CoroutineMgr.Instance.StartCoroutine(_shuffleUIState.ShuffleUI.CorShuffleEndAni());
                GameSessionMgr.Instance.NextShuffleRole();
            }

            yield return XAttachMachine.EnterState(DealCardUIState.StateIDStr, 1);
        }

        public override void OnUpdate(float deltaTime)
        {

        }

        private void OnRememberSelectCard(NotifyMsg obj)
        {
            _rememberIsOpen = false;
        }

        private void OnStealSelectCard(NotifyMsg obj)
        {
            _stealIsOpen = false;
        }
    }

    public interface IShuffleUIState: IBaseAttachUI
    {
        public PlayedCardUI    PlayedCardUI  { get; }
        public TotalCardHeapUI ShuffleUI     { get;  }
        public LevelInfoUI     LevelInfoUI   { get; }
        public SkillsUI        SkillsUI      { get; }
        public InsertAndReplaceUI       InsertAndReplaceUI     { get; }
        public SelectCardUI    SelectCardUI  { get; }
        public SelectSkillUI   SelectSkillUI { get; }
    }
}