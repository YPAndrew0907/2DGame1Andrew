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
        public override string      StateID => CurStateID;
        public const string         CurStateID = "GameEndUIState";
        private IGameEndUIState     _gameEndUI;
        public override void        OnCreate(GameSceneUI sceneUI)
        {
            _gameEndUI = sceneUI;
        }

        public override IEnumerator OnEnterAsync(object payload)
        {
            NotifyMgr.Instance.RegisterEvent(NotifyDefine.GAME_END_BACK_HOME, GameEndBackHome);
            
            if (payload is NormalParam param)
            {
                var code = (GameEndCode)param.IntValue;
                switch (code)
                {
                    case GameEndCode.Lose:
                        _gameEndUI.GameLossUI.ShowUI();
                        break;
                    case GameEndCode.Win:
                        _gameEndUI.GameWinUI.ShowUI();
                        break;
                }
            }
            yield break;
        }

        public override IEnumerator OnExitAsync(object payload)
        {
            
            yield break;
        }

        public override void  OnUpdate(float deltaTime)
        {
            
        }

        private void GameEndBackHome(NotifyMsg msg)
        {
            NotifyMgr.Instance.UnRegisterEvent(NotifyDefine.GAME_END_BACK_HOME, GameEndBackHome);

            CoroutineMgr.Instance.StartCoroutine(OnExitAsync(null));
        }
    }

    public interface IGameEndUIState : IBaseAttachUI
    {
        public GameLossUI GameLossUI { get; }
        public GameWinUI  GameWinUI { get; }
    }
}