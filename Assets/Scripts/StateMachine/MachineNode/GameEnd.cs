using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace MachineNode
{
    public class GameEnd : IStateNode
    {
        public string StateID { get; }

        public void OnCreate(IMachineMaster inClassName)
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