using System.Collections;
using System.Threading;
using UI;

namespace AttachMachine
{
    public abstract class BaseGameUIState : IAttachState
    {
        public abstract string      StateID   { get; }
        public          bool        isEntered { get; set; }
        public abstract void        OnCreate(IMachineMaster sceneUI);
        
        private         bool        _active;

        // active 用于进游戏时激活。主要是非游戏流程常驻UI状态
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
        
        // enter 用于UI状态切换。
        public virtual IEnumerator OnEnterAsync(object payload)
        {
            yield break;
        }

        public virtual IEnumerator OnExitAsync(object payload)
        {
            yield break;
        }
        public virtual void OnUpdate(float deltaTime)
        {
            
        }
    }
}