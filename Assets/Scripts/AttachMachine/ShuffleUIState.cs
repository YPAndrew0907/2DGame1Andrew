using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mgr;
using Obj;
using UI;
using UnityEngine;
using XYZFrameWork;
using InsertCardData = Obj.InsertCardData;

namespace AttachMachine
{
    public class ShuffleUIState : BaseGameUIState
    {
        private         IShuffleUIState _shuffleUIState;
        public override string          StateID => StateIDStr;
        public const    string          StateIDStr = "ShuffleUIState";

        private bool _rememberIsOpen;
        private bool _stealIsOpen;

        private Coroutine _shuffleCor;

        public override void OnCreate(IMachineMaster sceneUI)
        {
            if (sceneUI is IShuffleUIState ui)
            {
                _shuffleUIState = ui;
                GameSessionMgr.Instance.NextShuffleRole();
                NotifyMgr.RegisterNotify(NotifyDefine.CARD_REMEMBER_SELECT, OnRememberSelectCard);
                NotifyMgr.RegisterNotify(NotifyDefine.CARD_STEAL_INSERT, OnStealSelectCard);
                NotifyMgr.RegisterNotify(NotifyDefine.CLOSE_PANEL, OnClosePanel);
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
                _shuffleUIState.PlayedCardUI.ClearCards();

                yield return HandleRememberSkill(shuffleRole);
                if (!isEntered) yield break;

                yield return HandleStealAndInsertSkill(shuffleRole);
                if (!isEntered) yield break;

                _shuffleCor = CoroutineMgr.Instance.StartCoroutine(ShuffleAndShowCards(shuffleRole));
                yield return _shuffleCor;
                if (!isEntered) yield break;
            }
            else
            {
                _shuffleUIState.LevelInfoUI.SetCurRound(GameSessionMgr.Instance.RoundTimes);
            }
            GameSessionMgr.Instance.NextPlayerAskCard();

            yield return XAttachMachine.SwitchStateCor(null, DealCardUIState.StateIDStr, 1);
        }
        
        private IEnumerator HandleRememberSkill(PlayerType shuffleRole)
        {
            if (shuffleRole == PlayerType.Player && GameSessionMgr.Instance.CurPlayerSkills.Contains(PlayerSkill.Remember))
            {
                _shuffleUIState.SelectCardUI.Show("选择需要记忆的牌", NotifyDefine.CARD_REMEMBER_SELECT, 1);
                _rememberIsOpen = true;
                yield return new WaitUntil(() => !_rememberIsOpen);
                yield return new WaitForSeconds(1);
            }
        }

        private IEnumerator HandleStealAndInsertSkill(PlayerType shuffleRole)
        {
            if (shuffleRole == PlayerType.Player && GameSessionMgr.Instance.CurPlayerSkills.Contains(PlayerSkill.StealAndInsert))
            {
                var (_, stealCount) = SkillMgr.Instance.GetSkillParameters(PlayerSkill.StealAndInsert, -1);
                _shuffleUIState.InsertAndReplaceUI.Show(GameSessionMgr.Instance.PlayerSkillCards,
                    CardMgr.Instance.CardsList, true, stealCount);
                _stealIsOpen = true;
                yield return new WaitUntil(() => !_stealIsOpen);
                yield return new WaitForSeconds(0.5f);
            }
            else if (shuffleRole == PlayerType.AI && GameSessionMgr.Instance.CurBossSkills.Contains(PlayerSkill.StealAndInsert))
            {
                var (_, stealCount) = SkillMgr.Instance.GetSkillParameters(PlayerSkill.StealAndInsert, -1);
                var isInsert = AIMgr.RandomStealOrInsert(GameSessionMgr.Instance.AISkillCards.Count);
                if (isInsert)
                {
                    var (toCollect, toTotal) = AIMgr.SelectCards(CardMgr.Instance.Cards, GameSessionMgr.Instance.AISkillCards, stealCount);
                    NotifyMgr.SendEvent(NotifyDefine.CARD_STEAL_INSERT, new InsertCardData
                    {
                        IsAI          = true,
                        ToCollectList = toCollect,
                        ToTotalList   = toTotal
                    });
                }
                else
                {
                    var copyIndexList = AIMgr.AIRandomStealCard(CardMgr.Instance.Cards, stealCount);
                    var cardList      = CardMgr.Instance.StealCard(copyIndexList);
                    NotifyMgr.SendEvent(NotifyDefine.CARD_STEAL_INSERT, new InsertCardData
                    {
                        IsAI          = true,
                        ToCollectList = cardList
                    });
                }
            }
        }
        private IEnumerator ShuffleAndShowCards(PlayerType shuffleRole)
        {
            yield return _shuffleUIState.ShuffleUI.CorShuffleStartAni(shuffleRole);

            CardMgr.Instance.Shuffle();
            var list = CardMgr.Instance.Cards.ToList();
            _shuffleUIState.ShuffleUI.SetCard(list);
            
            GameSessionMgr.Instance.NextShuffleRole();
            yield return _shuffleUIState.ShuffleUI.CorShuffleEndAni();
        }
        
        public override IEnumerator OnExitAsync(object payload)
        {
            if (payload == XAttachMachine.ExitNullObject)
            {
                if (_shuffleCor!= null)
                {
                    yield return _shuffleCor;
                }
            }
            _shuffleCor = null;
        }

        public override void OnUpdate(float deltaTime)
        {

        }

        private void OnRememberSelectCard(NotifyMsg obj)
        {
            if (_rememberIsOpen)
            {
                _rememberIsOpen = false;
            }
        }

        private void OnStealSelectCard(NotifyMsg obj)
        {
            if (_stealIsOpen)
            {
                _stealIsOpen = false;
            }
        }
        
        private void OnClosePanel(NotifyMsg obj)
        {
            if (obj.Param is NormalParam { StrValue: StateIDStr })
            {
                if (_stealIsOpen)
                {
                    _stealIsOpen = false;
                }
                else if (_rememberIsOpen)
                {
                    _rememberIsOpen = false;
                }
            }
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