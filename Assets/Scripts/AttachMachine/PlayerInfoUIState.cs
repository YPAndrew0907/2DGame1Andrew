using System.Collections;
using Mgr;
using UI;
using XYZFrameWork;

namespace AttachMachine
{
    public class PlayerInfoUIState : BaseGameUIState
    {
        private         IPlayerInfoUIState _playerInfoUI;
        public override string             StateID => StateIDStr;
        public const    string             StateIDStr = "PlayerInfoUIState";

        public override void OnCreate(IMachineMaster sceneUI)
        {
            if (sceneUI is IPlayerInfoUIState playerInfoUI)
            {
                _playerInfoUI = playerInfoUI;
                _playerInfoUI.LevelInfoUI.Init();
            }
        }

        public override void OnActive()
        {
            base.OnActive();
            _playerInfoUI.LevelInfoUI.ShowUI(0,0,0);
        }

        public override void OnInActive()
        {
            base.OnInActive();
            _playerInfoUI.LevelInfoUI.Hide();
        }

        public override void OnUpdate(float deltaTime)
        {
            
        }

    }

    public interface IPlayerInfoUIState : IBaseAttachUI
    {
        public LevelInfoUI LevelInfoUI { get; }
    }
}