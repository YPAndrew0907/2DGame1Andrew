using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cfg;
using Mgr;
using Obj;
using UI;
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
                NotifyMgr.RegisterNotify(NotifyDefine.CARD_REMEMBER_SELECT, OnRememberSelectCards);
                NotifyMgr.RegisterNotify(NotifyDefine.CARD_COPY_SELECT, OnCopySelectCards);
                NotifyMgr.RegisterNotify(NotifyDefine.CARD_STEAL_INSERT, OnStealAndInsertCard);
                NotifyMgr.RegisterNotify(NotifyDefine.REPLACE_CARD, OnReplaceCard);
                NotifyMgr.RegisterNotify(NotifyDefine.SKILL_SELECT, OnSelectSkill);
                NotifyMgr.RegisterNotify(NotifyDefine.SKILL_CLICK, OnSkillClick);
                NotifyMgr.RegisterNotify(NotifyDefine.FIRE_SKILL, OnFireSkill);
                
                NotifyMgr.RegisterNotify(NotifyDefine.CLOSE_PANEL, OnClosePanel);
            }
        }

        public override void OnActive()
        {
            base.OnActive();
            _skillUI.SelectSkillUI.Hide();

            var skills = GameSessionMgr.Instance.CurPlayerSkills;
            if (skills == null)
            {
                return;
            }
        }

        public override void OnInActive()
        {
            base.OnInActive();
            // 这里只使用未初始化的技能列表
            _skillUI.SkillsUI.Hide();
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
                        var copyIndexList = AIMgr.AIRandomCopyCard(CardMgr.Instance.Cards, stealCount);
                        var cardList      = CardMgr.Instance.CopyCard(copyIndexList);
                        NotifyMgr.SendEvent(NotifyDefine.CARD_COPY_SELECT, new OperationData
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
                yield return new WaitForSeconds(0.3f);
                var skill = list2[index];
                switch (skill)
                {
                    case PlayerSkill.GuessOrRemember:
                        _skillUI.SelectSkillUI.Show(PlayerSkill.Guess, PlayerSkill.Remember);
                        uiShowing = true;
                        break;
                    case PlayerSkill.CopyAndSwitch:
                        GameSessionMgr.Instance.SelectSkill(PlayerType.Player, PlayerSkill.Switch);
                        var (_, stealCount) = SkillMgr.Instance.GetSkillParameters(PlayerSkill.CopyAndSwitch,-1);
                        _skillUI.SelectCardUI.Show("选择本局需要携带的牌", NotifyDefine.CARD_COPY_SELECT, stealCount);
                        uiShowing = true;
                        break;
                }

                yield return new WaitUntil(() => !uiShowing);
            }
            
            _skillUI.SkillsUI.Show(SkillMgr.Instance.SkillCardCount, GameSessionMgr.Instance.CurPlayerSkills);

            yield return XAttachMachine.ExitStateCor(StateIDStr);
        }

        public override IEnumerator OnExitAsync(object payload)
        {
            yield return XAttachMachine.EnterState(ShuffleUIState.StateIDStr);
        }

        public override void OnUpdate(float deltaTime)
        {

        }

        private void OnClosePanel(NotifyMsg obj)
        {
            if (obj.Param is NormalParam { StrValue: StateIDStr })
            {
                uiShowing = false;
            }
        }

        private void OnReplaceCard(NotifyMsg obj)
        {
            if (obj.Param is CustomParam param)
            {
                var data = (ReplaceCardData)param.Value;
                if (data != null)
                {
                    GameSessionMgr.Instance.SwitchCard(data.IsAI, data.TargetList, data.SkillCard);
                    if (data.IsAI)
                        _skillUI.DealCardAIUI.UpdateCards(GameSessionMgr.Instance.AICards);
                    else
                    {
                        _skillUI.DealCardPlayerUI.UpdateCards(GameSessionMgr.Instance.PlayerCards);
                        _skillUI.SkillsUI.SetSkillCard(GameSessionMgr.Instance.PlayerSkillCards);
                        NotifyMgr.SendEvent(NotifyDefine.FIRE_SKILL, new List<int>(){(int)PlayerType.Player,(int)PlayerSkill.Switch});
                    }
                    if(!data.IsAI) _skillUI.InsertAndReplaceUI.Hide();
                }
            }
        }

        private void OnStealAndInsertCard(NotifyMsg obj)
        {
            if (obj.Param is CustomParam param)
            {
                var data = (InsertCardData)param.Value;
                if (data != null)
                {
                    if (data.ToCollectList!= null)
                    {
                        GameSessionMgr.Instance.PushSkillCard(data.IsAI, data.ToCollectList.ToArray());
                        if (!data.IsAI)
                        {
                            _skillUI.SkillsUI.SetSkillCard(GameSessionMgr.Instance.PlayerSkillCards);
                            _skillUI.SkillsUI.RefreshUI();
                        }
                        NotifyMgr.SendEvent(NotifyDefine.FIRE_SKILL, new List<int>(){(int)PlayerType.Player,(int)PlayerSkill.StealAndInsert});
                        Debug.Log($"【偷得牌】IsAI={data.IsAI}：{string.Join(",", data.ToCollectList)}");
                    }

                    if (data.ToTotalList!= null)
                    {
                        CardMgr.Instance.PushCard(data.ToTotalList.ToArray());
                        Debug.Log($"【插入牌】IsAI={data.IsAI}：{string.Join(",", data.ToTotalList)}");
                    }
                    
                    NotifyMgr.SendEvent(NotifyDefine.FIRE_SKILL,
                        new List<int>(){data.IsAI ? (int)PlayerType.AI : (int)PlayerType.Player
                            ,(int)PlayerSkill.StealAndInsert});
                }
            }
            
        }

        private void OnRememberSelectCards(NotifyMsg obj)
        {
            if (obj.Param is CustomParam param)
            {
                var selectData = param.Value as OperationData;
                if (selectData is { SelectCards: not null })
                {
                    CardMgr.Instance.RememberCard(selectData.SelectCards);

                    if (!selectData.IsAI)
                    {
                        _skillUI.SkillsUI.SetSkillCard(GameSessionMgr.Instance.PlayerSkillCards);
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
                var selectData = param.Value as OperationData;
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
                    case PlayerSkill.CopyAndSwitch:
                    case PlayerSkill.Switch:
                        // 都是换牌。只不过有的牌是场外的。
                        var (rate, skillParam) = SkillMgr.Instance.GetSkillParameters(PlayerSkill.CopyAndSwitch,-1);
                        _skillUI.InsertAndReplaceUI.Show(GameSessionMgr.Instance.PlayerSkillCards,
                            GameSessionMgr.Instance.PlayerCards, false, skillParam);
                        break;
                    case PlayerSkill.Detect:
                        XAttachMachine.SwitchState(DealCardUIState.StateIDStr, CompareCardUIState.StateIDStr, PlayerType.AI);
                        break;
                    default:              throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void OnFireSkill(NotifyMsg msg)
        {
            if (msg.Param is CustomParam param)
            {
                var paramList = param.Value as List<int>;
                if (paramList== null) return;
                var playerType = (PlayerType)paramList[0]; 
                var skill = (PlayerSkill)paramList[1];

                if (playerType == PlayerType.AI)
                {
                    if (GameSessionMgr.Instance.CurBossSkills.Contains(skill))
                    {
                        var (rate, paramV) =  SkillMgr.Instance.GetSkillParameters(skill);
                        var isFire         =  AIMgr.IsCanDoSkill(rate);
                        if (isFire)
                        {
                            XAttachMachine.SwitchState(DealCardUIState.StateIDStr,CompareCardUIState.StateIDStr, PlayerType.Player);
                        }
                    }
                }
                else if (playerType == PlayerType.Player)
                {
                    if (GameSessionMgr.Instance.CurPlayerSkills.Contains(PlayerSkill.Detect))
                    {
                        var (rate, paramV) =  SkillMgr.Instance.GetSkillParameters(PlayerSkill.Detect, -1);
                        var isDetect       =  AIMgr.IsCanDoSkill(rate);
                        if (isDetect)
                        {
                            _skillUI.SkillsUI.ShowDetectFlag(true);
                        }
                    }
                }
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
        
        public DealCardPlayerUI DealCardPlayerUI { get; }
        public DealCardAIUI     DealCardAIUI     { get; }
    }
}