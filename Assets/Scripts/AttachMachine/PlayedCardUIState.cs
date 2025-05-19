using System.Collections;
using UI;
using XYZFrameWork;

namespace AttachMachine
{
    public class PlayedCardUIState : BaseGameUIState
    {
        public override string StateID => StateIDStr; 
        public const    string StateIDStr ="PlayedCardUIState";

        private IPlayedCardUIState _uiState;
        public override void        OnCreate(IMachineMaster sceneUI)
        {
            if (sceneUI is IPlayedCardUIState ui)
            {
                _uiState = ui;
                _uiState.PlayedCardUI.Init();
            }
        }

        public override void OnActive()
        {
            base.OnActive();
            _uiState.PlayedCardUI.Show();
        }

        public override void OnInActive()
        {
            base.OnInActive();
            _uiState.PlayedCardUI.Hide();
        }

        public override IEnumerator OnEnterAsync(object payload)
        {
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

    public interface IPlayedCardUIState: IBaseAttachUI
    {
        public PlayedCardUI PlayedCardUI { get;}
    }
}