using System;
using AttachMachine;
using UnityEngine;
using XYZFrameWork.Base;

namespace UI
{
    public class GameMachine : BaseViewMono,IMachineMaster,
                               IGuessOrRememberUIElement
    {
        XAttachMachine _xAttachMachine;
        protected override void OnAwake()
        {
            base.OnAwake();
            _xAttachMachine = new XAttachMachine(this);
            _xAttachMachine.RegisterState(new GuessOrRememberState());
            
        }

        #region UI元素

        public GameObject GuessOrRememberPanel { get; }
        public GameObject GuessPanel           { get; }
        public GameObject RememberPanel        { get; }
        
        #endregion
    }
}