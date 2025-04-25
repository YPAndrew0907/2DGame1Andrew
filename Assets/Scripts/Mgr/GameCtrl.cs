using System;
using BattleMachineNode;
using MachineNode;
using XYZFrameWork.Base;

namespace Mgr
{
    public class GameCtrl : BaseMonoSingleton<GameCtrl>
    {
        XStateMachine stateMachine;
        private void Start()
        {
            stateMachine = new XStateMachine(this);
            stateMachine.RegisterState(new GameStart());
            
            stateMachine.RegisterState(new GameEnd());
            stateMachine.RegisterState(new GameTurnAI());
            stateMachine.RegisterState(new GameTurnStart());
            stateMachine.RegisterState(new GameTurnDeal());
            stateMachine.RegisterState(new GameTurnEnd());
            stateMachine.RegisterState(new GameTurnAI());
            stateMachine.RegisterState(new GameTurnNext());
            stateMachine.RegisterState(new GameTurnRequestCard());
            
            
        }
    }
}