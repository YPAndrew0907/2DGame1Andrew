using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Obj;
using UnityEngine;

namespace UI
{
    public class GuessOrRememberState : BaseGameUIState
    {
        public override string                    StateID => "GuessOrRememberState";
        public          SkillType                 Skill   { get; set; }
        private         IGuessOrRememberUIElement _uiElement;
        
        public List<CardValue> RememberCards { get; set; }
            
        public override void OnCreate(IMachineMaster machine)
        {
            Machine = machine as GameMachine;
            Skill = SkillType.None;
        }

        public override IEnumerator OnEnterAsync(StateTransitionContext context, CancellationToken ct)
        {
            if (Skill == SkillType.None)
            {
                _uiElement.GuessOrRememberPanel?.SetActive(true);
            }
            else if (Skill == SkillType.Guess)
            {
                
            }
            yield break;
        }

        public override IEnumerator OnExitAsync(StateTransitionContext context, CancellationToken ct)
        {
            
            yield break;
        }

        public override void OnUpdate(float deltaTime)
        {
            
        }

        public enum SkillType
        {
            None,
            Guess,
            Remember
        }
    }

    public interface IGuessOrRememberUIElement
    {
        // 选择哪个技能
        public GameObject GuessOrRememberPanel { get; }
        
        // 猜测面板
        public GameObject GuessPanel { get; }
        
        // 记忆面板 每次自己洗牌时随机记住某几张牌的具体位置
        public GameObject RememberPanel { get; }
    }
    
}