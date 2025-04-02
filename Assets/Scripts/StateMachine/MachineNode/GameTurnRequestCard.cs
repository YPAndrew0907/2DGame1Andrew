using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace MachineNode
{
    // 要牌
    public class GameTurnRequestCard : IStateNode
    {
        public string                       StateID { get; }
        public void                         OnCreate(IMachineMaster machine)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator                  OnEnterAsync(StateTransitionContext context, CancellationToken ct)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator                  OnExitAsync(StateTransitionContext context, CancellationToken ct)
        {
            throw new System.NotImplementedException();
        }

        public void                         OnUpdate(float deltaTime)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<StateTransition> GetTransitions()
        {
            throw new System.NotImplementedException();
        }
    }
}