using System;
using System.Collections;
using Mgr;
using Obj;
using UI;
using Unity.VisualScripting;
using XYZFrameWork;

namespace AttachMachine
{
    public class GameEndUIState: BaseGameUIState
    {
        public override string          StateID => StateIDStr;
        public const    string          StateIDStr = "GameEndUIState";
        private         IGameEndUIState _gameEndUI;

        private GameEndCode _curCode;

        public override void OnCreate(IMachineMaster sceneUI)
        {
            if (sceneUI is IGameEndUIState ui)
            {
                _gameEndUI = ui;
                _gameEndUI.GameLossUI.Init();
                _gameEndUI.GameWinUI.Init();
                NotifyMgr.RegisterNotify(NotifyDefine.CLOSE_GAME_END_UI, OnCloseGameEndUI);
            }
        }

        public override IEnumerator OnEnterAsync(object payload)
        {
            if (payload is GameEndCode endCode)
            {
                _curCode = endCode;
                var moneyDelta = DataMgr.Instance.Money - DataMgr.Instance.StartMoney;
                switch (endCode)
                {
                    case GameEndCode.GiveUp:
                    case GameEndCode.Lose:
                        _gameEndUI.GameLossUI.Show(endCode,moneyDelta);
                        break;
                    case GameEndCode.Win:
                        _gameEndUI.GameWinUI.Show(endCode,moneyDelta);
                        break;
                }
            }
            yield break;
        }

        public override IEnumerator OnExitAsync(object payload)
        {
            switch (_curCode)
            {
                case GameEndCode.Win: 
                    _gameEndUI.GameWinUI.Hide();
                    break;
                case GameEndCode.GiveUp:
                case GameEndCode.Lose: 
                    _gameEndUI.GameLossUI.Hide();
                    break;
                default:               throw new ArgumentOutOfRangeException(_curCode.ToString());
            }

            _curCode = GameEndCode.None;
            NotifyMgr.SendEvent(NotifyDefine.GAME_END_BACK_HOME);
            yield break;
        }

        public override void  OnUpdate(float deltaTime)
        {
            
        }

        private void OnCloseGameEndUI(NotifyMsg obj)
        {
            XAttachMachine.ExitState(StateIDStr, 1);
        }
    }

    public interface IGameEndUIState : IBaseAttachUI
    {
        public GameLossUI GameLossUI { get; }
        public GameWinUI  GameWinUI { get; }
    }
}