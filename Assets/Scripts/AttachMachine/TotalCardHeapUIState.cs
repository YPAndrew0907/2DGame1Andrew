using System.Collections;
using System.Linq;
using Mgr;
using Obj;
using UI;
using XYZFrameWork;

namespace AttachMachine
{
    public class TotalCardHeapUIState : BaseGameUIState
    {
        ITotalCardHeapUI        _totalCardHeapUIState;
        public  override string StateID => StateIDStr;
        public const     string StateIDStr = "TotalCardHeapUIState";

        public override void OnCreate(IMachineMaster sceneUI)
        {
            if (sceneUI is ITotalCardHeapUI uiState)
            {
                _totalCardHeapUIState = uiState;
                _totalCardHeapUIState.TotalCardHeapUI.Init();
                NotifyMgr.RegisterNotify(NotifyDefine.DEAL_CARD, OnDealCard);
            }
        }

        public override void OnActive()
        {
            base.OnActive();
            _totalCardHeapUIState.TotalCardHeapUI.Show();   
        }

        public override void OnInActive()
        {
            base.OnInActive();
            _totalCardHeapUIState.TotalCardHeapUI.Hide();
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

        private void OnDealCard(NotifyMsg obj)
        {
            if (obj.Param is CustomParam param)
            {
                CardObj card = param.Value as CardObj;
                _totalCardHeapUIState.TotalCardHeapUI.RefreshTotalCard(card);
            }
        }
    }
    public interface ITotalCardHeapUI :IBaseAttachUI
    {
        public TotalCardHeapUI TotalCardHeapUI { get;  }
    }
}