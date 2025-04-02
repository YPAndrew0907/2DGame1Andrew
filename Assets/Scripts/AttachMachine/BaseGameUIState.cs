using System.Collections;
using System.Collections.Generic;
using System.Threading;
using AttachMachine;

namespace UI
{
    public abstract class BaseGameUIState : IAttachNode
    {
        public abstract string StateID { get; }

        protected GameMachine Machine;
        
        public abstract void                         OnCreate(IMachineMaster machine);
        public void OnCreate(AttachMachine.IMachineMaster machine)
        {
            
        }

        public abstract IEnumerator                  OnEnterAsync(StateTransitionContext context, CancellationToken ct);
        public abstract IEnumerator                  OnExitAsync(StateTransitionContext context, CancellationToken ct);
        public abstract void                         OnUpdate(float deltaTime);
    }
}