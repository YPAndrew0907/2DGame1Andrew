using System.Collections;
using System.Threading;
using UI;

namespace AttachMachine
{
    public abstract class BaseGameUIState : IAttachState
    {
        public abstract string      StateID { get; }
        public abstract void        OnCreate(IMachineMaster sceneUI);
        public abstract IEnumerator OnEnterAsync(object payload);
        public abstract IEnumerator OnExitAsync(object payload);
        public abstract void        OnUpdate(float deltaTime);
        private         bool        _active;

        public virtual void OnActive()
        {
            if (_active)return;
            _active = true;
        }

        public virtual void OnInActive()
        {
            if (!_active) return;
            _active = false;
        }
    }
}