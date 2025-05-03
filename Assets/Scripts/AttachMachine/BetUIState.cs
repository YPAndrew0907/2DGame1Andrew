using System.Collections;
using Mgr;
using Obj;
using UI;
using UnityEngine;

namespace AttachMachine
{
    public class BetUIState : BaseGameUIState
    {
        public override string      StateID => StateIDStr;
        public const string         StateIDStr = "BetUIState";
        private IBetUI              _betUI;
        public override void        OnCreate(GameSceneUI sceneUI)
        {
            _betUI = sceneUI;
        }

        public override IEnumerator OnEnterAsync(object payload)
        {
            if (EnoughMoney())
            {
                _betUI.BetUI.ShowBetUI();
            }
            else
            {
                yield return OnExitAsync(null);
            }
        }

        public override IEnumerator OnExitAsync(object payload)
        {
            _betUI.AttachMachine.StartMachine(GameEndUIState.CurStateID);
            yield break;
        }

        public override void        OnUpdate(float deltaTime)
        {
            
        }
        
        private bool EnoughMoney()
        {
            var money= DataMgr.Instance.Money;
            return money >= DataMgr.Instance.CurMinBetMoney;
        }
    }

    public interface IBetUI:IBaseAttachUI
    {
        public BetUI BetUI { get; }
    }
}