using System.Collections;
using System.Threading;
using UI;

namespace AttachMachine
{
    public abstract class BaseGameUIState : IAttachNode
    {
        public abstract string      StateID { get; }
        public abstract void        OnCreate(GameSceneAiui sceneAiui);
        public abstract IEnumerator OnEnterAsync(object payload);
        public abstract IEnumerator OnExitAsync(object payload);
        public abstract void        OnUpdate(float deltaTime);
    }
}