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
            }
        }

        public override IEnumerator OnEnterAsync(object payload)
        {
            if (payload is GameEndCode endCode)
            {
                switch (endCode)
                {
                    case GameEndCode.GiveUp:
                    case GameEndCode.Lose:
                        _gameEndUI.GameLossUI.Show(endCode);
                        break;
                    case GameEndCode.Win:
                        _gameEndUI.GameWinUI.Show(endCode);
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
                case GameEndCode.Lose: 
                    _gameEndUI.GameLossUI.Hide();
                    break;
            }
            yield break;
        }

        public override void  OnUpdate(float deltaTime)
        {
            
        }
    }

    public interface IGameEndUIState : IBaseAttachUI
    {
        public GameLossUI GameLossUI { get; }
        public GameWinUI  GameWinUI { get; }
    }
}