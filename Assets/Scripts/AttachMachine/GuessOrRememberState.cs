using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Obj;
using UI;
using UnityEngine;

namespace AttachMachine
{
    public class GuessOrRememberState : BaseGameUIState
    {
        public override string                    StateID => "GuessOrRememberState";
        public const    string                    GuessOrRememberStateID = "GuessOrRememberState";
        public          SkillType                 Skill   { get; private set; }
        private         IGuessOrRememberUIElement _uiElement;
        
        public List<CardValue> RememberCards { get; set; }
            
        public override void OnCreate(GameSceneUI sceneUI)
        {
            if (sceneUI is IGuessOrRememberUIElement uiElement)
            {
                _uiElement = uiElement;
            }
            Skill         = SkillType.None;
        }

        public override IEnumerator OnEnterAsync(object payload)
        {
            if (Skill == SkillType.None)
            {
                _uiElement.GuessOrRememberPanel.gameObject.SetActive(true);
            }
            else if (Skill == SkillType.Guess)
            {
                
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
        public GuessOrRememberUI GuessOrRememberPanel { get; }
    }
    
}