using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Mgr;

namespace MachineNode
{
    public class GameStart : IStateNode
    {
        public string StateID { get; }

        public void OnCreate(IMachineMaster machine)
        {
            
        }

        public IEnumerator OnEnterAsync(StateTransitionContext context, CancellationToken ct)
        {
            
            NotifyMgr.Instance.SendEvent(NotifyDefine.GAME_START);
            yield break;
        }

        public IEnumerator OnExitAsync(StateTransitionContext context, CancellationToken ct)
        {
            throw new System.NotImplementedException();
            
        }

        public void OnUpdate(float deltaTime)
        {
        }
    }
}