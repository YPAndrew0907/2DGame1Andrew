using System.Collections;
using System.Collections.Generic;
using Mgr;
using Obj;
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
                NotifyMgr.RegisterNotify(NotifyDefine.COLLECT_PLAYED_CARD,OnCollectPlayedCard);
            }
        }

        private void OnCollectPlayedCard(NotifyMsg obj)
        {
            if (obj.Param is CustomParam param)
            {
                List<CardObj> list = param.Value as List<CardObj>;
                _uiState.PlayedCardUI.AddCards(list);
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