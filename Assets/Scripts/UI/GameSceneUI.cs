using UnityEngine.UI;
using AttachMachine;
using Base;
using TMPro;
using UnityEngine;
namespace UI
{
    public class GameSceneAiui : BaseViewMono,IMachineMaster,
                                 IGuessOrRememberUIElement,
                                 IShuffleUIState,
                                 IDealCardAIUIState,IDealCardPlayerUIState
    {
		//AUTO-GENERATE
		private CardHeap _monoAICardHeap;
		private CardHeap _monoPlayerCardHeap;
		private CardHeap _monoPublicCardHeap;
		private CardHeap _monoSkillCard;
		private TextMeshProUGUI _txtLevel;
		private TextMeshProUGUI _txtMoney;
		private TextMeshProUGUI _txtSkillName;

		protected override void FindUI()
		{
			    _monoAICardHeap = transform.Find("mono_AICardHeap").GetComponent<CardHeap>();
			_monoPlayerCardHeap = transform.Find("mono_PlayerCardHeap").GetComponent<CardHeap>();
			_monoPublicCardHeap = transform.Find("mono_PublicCardHeap").GetComponent<CardHeap>();
			     _monoSkillCard = transform.Find("mono_SkillCard").GetComponent<CardHeap>();
			          _txtLevel = transform.Find("txt_level").GetComponent<TMPro.TextMeshProUGUI>();
			          _txtMoney = transform.Find("PlayerInfo/money/txt_money").GetComponent<TMPro.TextMeshProUGUI>();
			      _txtSkillName = transform.Find("PlayerInfo/skill/txt_skillName").GetComponent<TMPro.TextMeshProUGUI>();
		}


		//AUTO-GENERATE-END
        XAttachMachine _xAttachMachine;
        protected override void OnAwake()
        {
            base.OnAwake();
            _xAttachMachine = new XAttachMachine(this);
            _xAttachMachine.RegisterState(new GuessOrRememberState());
            _xAttachMachine.RegisterState(new ShuffleUIState());
            _xAttachMachine.RegisterState(new DealCardAIUIState());
            _xAttachMachine.RegisterState(new DealCardPlayerUIState());
            
            _xAttachMachine.StartMachine(ShuffleUIState.StateIDStr);
        }
        #region UI元素
        // IGuessOrRememberUIElement
        public GameObject GuessOrRememberPanel { get; }
        public GameObject GuessPanel           { get; }
        public GameObject RememberPanel        { get; }
        
        // IShuffleUIState
        public GameObject      ShuffleUI   => _monoPublicCardHeap.gameObject;
        
	    public GameObject      DealCardAIUI  => _monoAICardHeap.gameObject;
	    
	    public GameObject      DealCardPlayerUI => _monoPlayerCardHeap.gameObject;
        #endregion

    }
}
