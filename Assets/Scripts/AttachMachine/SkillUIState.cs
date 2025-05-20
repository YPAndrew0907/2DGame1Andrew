using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cfg;
using Mgr;
using Obj;
using UI;
using XYZFrameWork;

namespace AttachMachine
{
    public class SkillUIState : BaseGameUIState
    {
        public override string            StateID => StateIDStr;
        public const    string            StateIDStr = "SkillUIState";
        private         ISkillUI          _skillUI;
        
        public override void OnCreate(IMachineMaster sceneUI)
        {
            if (sceneUI is ISkillUI uiElement)
            {
                _skillUI = uiElement;
                _skillUI.SkillCardsUI.Init();
                _skillUI.GuessOrRememberUI.Init();
                NotifyMgr.RegisterNotify(NotifyDefine.SKILL_CARD_SELECT,OnRememberSelectCards);
                NotifyMgr.RegisterNotify(NotifyDefine.SKILL_SELECT,OnSelectSkill);
            }
        }

        public override void OnActive()
        {
            base.OnActive();
            _skillUI.SkillCardsUI.Hide();
            _skillUI.GuessOrRememberUI.Hide();
            
            var skills = DataMgr.Instance.CurSkills;
            if (skills == null)
            {
                return;
            }
           
            _skillUI.SkillCardsUI.Show(DataMgr.Instance.CollectCardCount);
        }

        public override void OnInActive()
        {
            base.OnInActive();
            _skillUI.SkillCardsUI.Hide();
        }

        public override IEnumerator OnEnterAsync(object payload)
        {
            var list = DataMgr.Instance.CurSkills;
            if (list.Contains(PlayerSkill.GuessOrRemember))
            {
                _skillUI.GuessOrRememberUI.ShowSelectSkillPanel();
            }
            else
            {
                yield return XAttachMachine.ExitStateCor(StateIDStr);
            }

        }

        public override IEnumerator OnExitAsync(object payload)
        {
            _skillUI.SkillCardsUI.Hide();
            yield return XAttachMachine.EnterState(ShuffleUIState.StateIDStr);
        }

        public override void OnUpdate(float deltaTime)
        {
            
        }

        private void OnRememberSelectCards(NotifyMsg obj)
        {
            if (obj.Param is CustomParam param)
            {
                var list = param.Value as List<CardObj>;
                CardMgr.Instance.RememberCard(list);
            }
        }

        // 开局送的牌
        private void OnCollectCard(NotifyMsg obj)
        {
            if (obj.Param is CustomParam param)
            {
                var list = param.Value as List<CardObj>;
                _skillUI.SkillCardsUI.SetSkillCard(list);
                _skillUI.SkillCardsUI.RefreshUI();
                _skillUI.SkillCardsUI.Hide();
            }
        }

        private void OnSelectSkill(NotifyMsg obj)
        {
            if (obj.Param is NormalParam param)
            {
                PlayerSkill skill = (PlayerSkill)param.IntValue;
                
                DataMgr.Instance.SkillSelect(skill);
                _skillUI.GuessOrRememberUI.Hide();
                XAttachMachine.ExitState(StateIDStr);
            }
        }
    }

    public interface ISkillUI: IBaseAttachUI
    {
        // 选择哪个技能
        public GuessOrRememberUI GuessOrRememberUI { get; }
        
        public SkillCardsUI SkillCardsUI { get; }
    }
    
}