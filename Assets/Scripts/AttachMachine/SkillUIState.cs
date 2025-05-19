using System.Collections;
using System.Collections.Generic;
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
        public          List<PlayerSkill> Skill   { get; private set; }
        private         ISkillUI          _skillUI;
        
        public override void OnCreate(IMachineMaster sceneUI)
        {
            if (sceneUI is ISkillUI uiElement)
            {
                _skillUI = uiElement;
                _skillUI.SkillCardsUI.Init();
                _skillUI.GuessOrRememberUI.Init();
                NotifyMgr.Register(NotifyDefine.SKILL_CARD_SELECT,OnSelectSkillCards);
            }
        }

        public override void OnActive()
        {
            base.OnActive();
            _skillUI.SkillCardsUI.Hide();
            _skillUI.GuessOrRememberUI.Hide();
        }

        public override void OnInActive()
        {
            base.OnInActive();
        }

        public override IEnumerator OnEnterAsync(object payload)
        {
            List<PlayerSkill> skillList = new List<PlayerSkill>();
            if (DataMgr.Instance.CurSkills == null)
            {
                yield break;
            }
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
            
            if (Skill.Contains(PlayerSkill.GuessOrRemember))
            {
                if (_skillUI.GuessOrRememberUI.CheckSelect())
                {
                    _skillUI.GuessOrRememberUI.ShowSelectPanel();
                }
            }
            
            _skillUI.SkillCardsUI.Show(DataMgr.Instance.MaxSkillCardCount);
        }

        public override IEnumerator OnExitAsync(object payload)
        {
            
            _skillUI.SkillCardsUI.Hide();
            yield break;
        }

        public override void OnUpdate(float deltaTime)
        {
            
        }
        
        public void OnSelectSkillCards(NotifyMsg obj)
        {
            if (obj.Param is CustomParam param)
            {
                var list = param.Value as List<CardObj>;
                _skillUI.SkillCardsUI.SetSkillCard(list);
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