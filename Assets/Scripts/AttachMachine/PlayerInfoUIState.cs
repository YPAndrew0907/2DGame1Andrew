using System.Collections;
using Mgr;
using UI;

namespace AttachMachine
{
    public class PlayerInfoUIState : BaseGameUIState
    {
        private         IPlayerInfoUIState _playerInfoUI;
        public override string             StateID => StateIDStr;
        public const    string             StateIDStr = "PlayerInfoUIState";

        public override void OnCreate(GameSceneUI sceneUI)
        {
            if (sceneUI is IPlayerInfoUIState playerInfoUI)
            {
                _playerInfoUI = playerInfoUI;
            }
        }

        public override IEnumerator OnEnterAsync(object payload)
        {
            _playerInfoUI.LevelInfoUI.SetLevel(DataMgr.Instance.CurLevel);
            _playerInfoUI.LevelInfoUI.SetMoney(DataMgr.Instance.Money);
            _playerInfoUI.LevelInfoUI.SetSkillName("");
            yield break;
        }

        public override IEnumerator OnExitAsync(object payload)
        {
            yield break;
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