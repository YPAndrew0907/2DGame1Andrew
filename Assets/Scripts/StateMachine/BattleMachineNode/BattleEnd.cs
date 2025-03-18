using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace StateMachine
{
    public class BattleEnd : IStateNode
    {
        public string StateID { get; }

        public void OnCreate(BattleStateMachine inClassName)
        {

        }

        public IEnumerator OnEnterAsync(StateTransitionContext context, CancellationToken ct)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator OnExitAsync(StateTransitionContext context, CancellationToken ct)
        {
            throw new System.NotImplementedException();
        }

        public void OnUpdate(float deltaTime)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<StateTransition> GetTransitions()
        {
            throw new System.NotImplementedException();
        }
    }
}