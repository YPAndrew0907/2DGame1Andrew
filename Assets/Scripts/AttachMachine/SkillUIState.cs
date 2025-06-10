using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cfg;
using Mgr;
using Obj;
using UI;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using XYZFrameWork;

namespace AttachMachine
{
    public class SkillUIState : BaseGameUIState
    {
        public override string   StateID => StateIDStr;
        public const    string   StateIDStr = "SkillUIState";
        private         ISkillUI _skillUI;
        
        private         bool     uiShowing;

        public override void OnCreate(IMachineMaster sceneUI)
        {
            if (sceneUI is ISkillUI uiElement)
            {
                _skillUI = uiElement;
                _skillUI.SkillsUI.Init();
                _skillUI.InsertAndReplaceUI.Init();
                _skillUI.SelectCardUI.Init();
                _skillUI.SelectSkillUI.Init();
                NotifyMgr.RegisterNotify(NotifyDefine.SELECT_CARD_REMEMBER, OnRememberSelectCards);
                NotifyMgr.RegisterNotify(NotifyDefine.SELECT_CARD_COPY, OnCopySelectCards);
                NotifyMgr.RegisterNotify(NotifyDefine.SELECT_CARD_STEAL, OnStealSelectCard);
                NotifyMgr.RegisterNotify(NotifyDefine.REPLACE_CARD, OnReplaceCard);
                NotifyMgr.RegisterNotify(NotifyDefine.SKILL_SELECT, OnSelectSkill);
                NotifyMgr.RegisterNotify(NotifyDefine.SKILL_CLICK, OnSkillClick);
            }
        }

        public override void OnActive()
        {
            base.OnActive();
            _skillUI.SkillsUI.Hide();
            _skillUI.SelectSkillUI.Hide();

            var skills = GameSessionMgr.Instance.CurPlayerSkills;
            if (skills == null)
            {
                return;
            }

            _skillUI.SkillsUI.Show(SkillMgr.Instance.SkillCardCount, GameSessionMgr.Instance.CurPlayerSkills);
        }

        public override void OnInActive()
        {
            base.OnInActive();
            // 这里只使用未初始化的技能列表
            _skillUI.SkillsUI.Show(SkillMgr.Instance.SkillCardCount, SkillMgr.Instance.UnLockSkillList());
        }

        public override IEnumerator OnEnterAsync(object payload)
        {
            var list1 = GameSessionMgr.Instance.CurBossSkills;
            var list2 = GameSessionMgr.Instance.CurPlayerSkills;

            for (var index = 0; index < list1.Count; index++)
            {
                var skill = list1[index];
                switch (skill)
                {
                    case PlayerSkill.GuessOrRemember:
                        var randomSkill = AIMgr.AIRandomGuessOrRemember();
                        Debug.Log($"【选技能】:AI-->{(PlayerSkill)randomSkill}");
                        NotifyMgr.SendEvent(NotifyDefine.SKILL_SELECT,
                            new List<int>() { (int)PlayerType.AI, randomSkill });
                        break;
                    case PlayerSkill.CopyAndSwitch:
                        var (_, stealCount) = SkillMgr.Instance.GetSkillParameters(PlayerSkill.CopyAndSwitch);
                        var copyIndexList = AIMgr.AIRandomCopyCard(CardMgr.Instance.Cards, (int)stealCount);
                        var cardList      = CardMgr.Instance.CopyCard(copyIndexList);
                        NotifyMgr.SendEvent(NotifyDefine.SELECT_CARD_COPY, new SelectCardData
                        {
                            IsAI        = true,
                            SelectCards = cardList
                        });
                        GameSessionMgr.Instance.SelectSkill(PlayerType.AI, PlayerSkill.Switch);
                        break;
                }
            }

            for (var index = 0; index < list2.Count; index++)
            {
                var skill = list2[index];
                switch (skill)
                {
                    case PlayerSkill.GuessOrRemember:
                        _skillUI.SelectSkillUI.Show(PlayerSkill.Guess, PlayerSkill.Remember);
                        uiShowing = true;
                        break;
                    case PlayerSkill.CopyAndSwitch:
                        GameSessionMgr.Instance.SelectSkill(PlayerType.Player, PlayerSkill.Switch);
                        var (_, stealCount) = SkillMgr.Instance.GetSkillParameters(PlayerSkill.CopyAndSwitch,0);
                        _skillUI.SelectCardUI.Show("选择本局需要携带的牌", NotifyDefine.SELECT_CARD_COPY, (int)stealCount);
                        uiShowing = true;
                        break;
                }

                yield return new WaitUntil(() => !uiShowing);
                yield return new WaitForSeconds(0.5f);
            }

            yield return XAttachMachine.ExitStateCor(StateIDStr);
        }

        public override IEnumerator OnExitAsync(object payload)
        {
            _skillUI.SkillsUI.Hide();
            yield return XAttachMachine.EnterState(ShuffleUIState.StateIDStr);
        }

        public override void OnUpdate(float deltaTime)
        {

        }

        private void OnReplaceCard(NotifyMsg obj)
        {
            if (obj.Param is CustomParam param)
            {
                var data = (ReplaceData)param.Value;
                if (data != null)
                {
                    GameSessionMgr.Instance.SwitchCard(data.IsAI, data.CurIndex, data.ReplaceIndex);
                    if(!data.IsAI) _skillUI.InsertAndReplaceUI.Hide();
                }
            }
        }

        private void OnRememberSelectCards(NotifyMsg obj)
        {
            if (obj.Param is CustomParam param)
            {
                var selectData = param.Value as SelectCardData;
                if (selectData is { SelectCards: not null })
                {
                    CardMgr.Instance.RememberCard(selectData.SelectCards);

                    if (!selectData.IsAI)
                    {
                        _skillUI.SkillsUI.SetSkillCard(GameSessionMgr.Instance.PlayerSkillCards);
                        _skillUI.SelectCardUI.Hide();       
                    }

                    Debug.Log($"【记忆牌】IsAI={selectData.IsAI}：{string.Join(",", selectData.SelectCards)}");
                }
                else
                {
                    Debug.LogError(LogTxt.PARAM_ERROR);
                }
            }
        }

        private void OnCopySelectCards(NotifyMsg obj)
        {
            if (obj.Param is CustomParam param)
            {
                var selectData = param.Value as SelectCardData;
                if (selectData != null && selectData.SelectCards != null)
                {
                    var copiedList = new List<CardObj>();
                    foreach (var card in selectData.SelectCards)
                    {
                        var newCard = card.DeepCopy();
                        newCard.IsCopy = true; // 标记为复制牌
                        copiedList.Add(newCard);
                    }

                    GameSessionMgr.Instance.PushSkillCard(selectData.IsAI, copiedList.ToArray());
                    if (!selectData.IsAI)
                    {
                        _skillUI.SkillsUI.SetSkillCard(GameSessionMgr.Instance.PlayerSkillCards);
                        _skillUI.SkillsUI.RefreshUI();
                        _skillUI.SkillsUI.Hide();
                        _skillUI.SelectCardUI.Hide();
                        uiShowing = false;
                    }

                    // UI刷新可选

                    Debug.Log($"【复制牌】IsAI={selectData.IsAI}：{string.Join(",", copiedList)}");
                }
                else
                {
                    Debug.LogError(LogTxt.PARAM_ERROR);
                }
            }
        }

        private void OnStealSelectCard(NotifyMsg obj)
        {
            if (obj.Param is CustomParam param)
            {
                var selectData = param.Value as SelectCardData;
                if (selectData != null && selectData.SelectCards != null)
                {
                    GameSessionMgr.Instance.PushSkillCard(selectData.IsAI, selectData.SelectCards.ToArray());
                    if (!selectData.IsAI)
                    {
                        _skillUI.SkillsUI.SetSkillCard(GameSessionMgr.Instance.PlayerSkillCards);
                        _skillUI.SkillsUI.RefreshUI();
                        _skillUI.SelectCardUI.Hide();
                    }
                    // UI刷新可选

                    Debug.Log($"【偷得牌】IsAI={selectData.IsAI}：{string.Join(",", selectData.SelectCards)}");
                }
                else
                {
                    Debug.LogError(LogTxt.PARAM_ERROR);
                }
            }
        }
        
        private void OnSelectSkill(NotifyMsg obj)
        {
            if (obj.Param is CustomParam param)
            {
                List<int> list = (List<int>)param.Value;
                if (list != null)
                {
                    PlayerType  type  = (PlayerType)list[0];
                    PlayerSkill skill = (PlayerSkill)list[1];
                    GameSessionMgr.Instance.SelectSkill(type, skill);
                    if (type == PlayerType.Player)
                    {
                        _skillUI.SkillsUI.SetSkills(GameSessionMgr.Instance.CurPlayerSkills);
                        uiShowing = false;
                    }

                    _skillUI.SelectSkillUI.Hide();
                }
            }
        }

        private void OnSkillClick(NotifyMsg msg)
        {
            if (msg.Param is NormalParam param)
            {
                var skill = (PlayerSkill)param.IntValue;
                switch (skill)
                {
                    case PlayerSkill.StealAndInsert:
                    case PlayerSkill.Switch:
                        // 都是换牌。只不过有的牌是场外的。
                        var (skillParam, rate) = SkillMgr.Instance.GetSkillParameters(PlayerSkill.Switch,0);
                        _skillUI.InsertAndReplaceUI.ShowReplace(GameSessionMgr.Instance.PlayerSkillCards,
                            GameSessionMgr.Instance.PlayerCards, (int)skillParam);
                        break;
                    default:              throw new ArgumentOutOfRangeException();
                }
                _skillUI.SelectSkillUI.Hide();
            }
        }
    }

    public interface ISkillUI : IBaseAttachUI
    {
        // 选择哪个技能
        public SkillsUI      SkillsUI      { get; }
        public InsertAndReplaceUI     InsertAndReplaceUI     { get; }
        public SelectCardUI  SelectCardUI  { get; }
        public SelectSkillUI SelectSkillUI { get; }
    }
}