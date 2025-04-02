using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.SceneManagement;

namespace MachineNode
{
    // 关卡结束
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
            
            
            SceneManager.LoadScene("Scenes/Home", LoadSceneMode.Single);
            yield return null;
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