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
        public override void   OnCreate(IMachineMaster sceneUI)
        {
            if (sceneUI is IBetUI ui)
            {
                _betUI = ui;
            }
            _betUI.BetUI.InitBetUI();
        }

        public override IEnumerator OnEnterAsync(object payload)
        {
            _betUI.BetUI.ShowBetUI();
            yield break;
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