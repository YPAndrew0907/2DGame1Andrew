using System.Collections;
using Mgr;
using UI;

namespace AttachMachine
{
    public class SkillCardsUIState : BaseGameUIState
    {
        public override string StateID => StateIDStr;
        public const    string StateIDStr = "SkillCardsUIState";
        private ISkillCardsUIState _state;
        public override void        OnCreate(GameSceneUI sceneUI)
        {
            _state = sceneUI;
            
            _state.SkillCardsUI.InitSkillCard(0);
        }

        public override IEnumerator OnEnterAsync(object payload)
        {
            _state.SkillCardsUI.InitSkillCard(0);
            NotifyMgr.Instance.SendEvent(NotifyDefine.SKILL_CARD_SELECT_START);
            yield break;
            
        }

        public override IEnumerator OnExitAsync(object payload)
        {
            throw new System.NotImplementedException();
        }

        public override void        OnUpdate(float deltaTime)
        {
            throw new System.NotImplementedException();
        }
    }

    public interface ISkillCardsUIState
    {
        public SkillCardsUI SkillCardsUI { get; }
    }
}