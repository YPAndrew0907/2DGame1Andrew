using System.Collections;
using UI;
using UnityEngine;

namespace AttachMachine
{
    public class BetUIState : BaseGameUIState
    {
        public override string      StateID => StateIDStr;
        public const string         StateIDStr = "BetUIState";
        public override void        OnCreate(GameSceneUI sceneUI)
        {
            throw new System.NotImplementedException();
        }

        public override IEnumerator OnEnterAsync(object payload)
        {
            throw new System.NotImplementedException();
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

    public interface IBetUIState
    {
        public BetUI BetUI { get; }
    }
}