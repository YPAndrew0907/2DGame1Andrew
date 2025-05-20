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
                NotifyMgr.RegisterNotify(NotifyDefine.MONEY_CHANGE, OnMoneyChange);
            }
        }

        public override void OnActive()
        {
            base.OnActive();
            _playerInfoUI.LevelInfoUI.SetLevel(DataMgr.Instance.CurLevel);
            _playerInfoUI.LevelInfoUI.SetMoney(DataMgr.Instance.Money);
            _playerInfoUI.LevelInfoUI.SetSkillName(null);
            _playerInfoUI.LevelInfoUI.ShowUI(DataMgr.Instance.CurLevel,DataMgr.Instance.Money, DataMgr.Instance.CurSkillDesc);
        }

        public override void OnInActive()
        {
            base.OnInActive();
            _playerInfoUI.LevelInfoUI.Hide();
        }

        public override IEnumerator OnEnterAsync(object payload)
        {
            yield break;
        }

        public override IEnumerator OnExitAsync(object payload)
        {
            NotifyMgr.UnRegisterNotify(NotifyDefine.MONEY_CHANGE, OnMoneyChange);
            yield break;
        }

        public override void OnUpdate(float deltaTime)
        {
            
        }

        private void OnMoneyChange(NotifyMsg obj)
        {
            if (obj.Param is NormalParam money)
            {
                _playerInfoUI.LevelInfoUI.SetMoney(DataMgr.Instance.Money);
            }
        }

    }

    public interface IPlayerInfoUIState : IBaseAttachUI
    {
        public LevelInfoUI LevelInfoUI { get; }
    }
}