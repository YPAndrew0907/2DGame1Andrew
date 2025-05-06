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
                NotifyMgr.Instance.RegisterEvent(NotifyDefine.GAME_START, OnGameStart);
                NotifyMgr.Instance.RegisterEvent(NotifyDefine.MONEY_CHANGE, OnMoneyChange);
            }
        }

        private void OnGameStart(NotifyMsg obj)
        {
            _playerInfoUI.LevelInfoUI.ShowUI(DataMgr.Instance.CurLevel,DataMgr.Instance.Money, DataMgr.Instance.CurSkillDesc);
        }

        private void OnMoneyChange(NotifyMsg obj)
        {
            if (obj.Param is NormalParam money)
            {
                _playerInfoUI.LevelInfoUI.SetMoney(DataMgr.Instance.Money);
            }
        }

        public override IEnumerator OnEnterAsync(object payload)
        {
            _playerInfoUI.LevelInfoUI.SetLevel(DataMgr.Instance.CurLevel);
            _playerInfoUI.LevelInfoUI.SetMoney(DataMgr.Instance.Money);
            _playerInfoUI.LevelInfoUI.SetSkillName(null);
            yield break;
        }

        public override IEnumerator OnExitAsync(object payload)
        {
            NotifyMgr.Instance.UnRegisterEvent(NotifyDefine.GAME_START, OnGameStart);
            NotifyMgr.Instance.UnRegisterEvent(NotifyDefine.MONEY_CHANGE, OnMoneyChange);
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