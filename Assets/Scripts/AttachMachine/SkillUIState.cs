using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cfg;
using Mgr;
using Obj;
using UI;
using UnityEngine;

namespace AttachMachine
{
    public class SkillUIState : BaseGameUIState
    {
        public override string    StateID => SkillUIStateID;
        public const    string    SkillUIStateID = "SkillUIState";
        public          List<PlayerSkill> Skill   { get; private set; }
        private         ISkillUI  _ui;
        
        public override void OnCreate(GameSceneUI sceneUI)
        {
            if (sceneUI is ISkillUI uiElement)
            {
                _ui = uiElement;
                
                _ui.SkillCardsUI.InitSkillCard(0);
                List<PlayerSkill> skillList = new List<PlayerSkill>();
                var skills = DataMgr.Instance.CurSkills;
                foreach (var skill in skills)
                {
                    switch (skill)
                    {
                        case PlayerSkill.Guess:
                        case PlayerSkill.Remember:
                        case PlayerSkill.SwitchCard:
                        case PlayerSkill.Lie:
                        case PlayerSkill.GuessOrRemember:
                        case PlayerSkill.StealAndInsert:
                            skillList.Add(skill);
                            break;
                    }
                }
                Skill = skillList;
            }
        }

        public override IEnumerator OnEnterAsync(object payload)
        {
            if (Skill.Contains(PlayerSkill.GuessOrRemember))
            {
                if (_ui.GuessOrRememberPanel.CheckSelect())
                {
                    _ui.GuessOrRememberPanel.ShowSelectPanel();
                }
            }
            yield break;
        }

        public override IEnumerator OnExitAsync(object payload)
        {
            
            yield break;
        }

        public override void OnUpdate(float deltaTime)
        {
            
        }

        public void OnShuffleEnd(object payload)
        {
            
        }

        public void SelectCards()
        {
            _ui.SkillCardsUI.InitSkillCard(0);
            NotifyMgr.Instance.SendEvent(NotifyDefine.SKILL_CARD_SELECT_START);
        }
    }

    public interface ISkillUI:IBaseAttachUI
    {
        // 选择哪个技能
        public GuessOrRememberUI GuessOrRememberPanel { get; }
        
        public SkillCardsUI SkillCardsUI { get; }
    }
    
}