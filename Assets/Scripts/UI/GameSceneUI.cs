using XYZFrameWork.Base;
using UnityEngine.UI;
using AttachMachine;
using Base;
using TMPro;
using UnityEngine;
namespace UI
{
	public class GameSceneUI : BaseViewMono, IMachineMaster,
	                           IGuessOrRememberUIElement,
	                           IShuffleUIState,
	                           IDealCardUIState,
	                           IBetUIState,
	                           IPlayerInfoUIState,
	                           ISkillCardsUIState
	{
		//AUTO-GENERATE
		private BetUI             _monoBetPanel;
		private ShuffleUI         _monoShuffleUI;
		private LevelInfoUI       _monoLevelInfo;
		private DealCardAIUI      _monoDealCardAI;
		private SkillCardsUI      _monoSkillCards;
		private DealCardPlayerUI  _monoDealPlayerCard;
		private GuessOrRememberUI _monoGuessOrRemember;

		protected override void FindUI()
		{
			_monoBetPanel        = transform.Find("mono_BetPanel").GetComponent<UI.BetUI>();
			_monoShuffleUI       = transform.Find("mono_ShuffleUI").GetComponent<UI.ShuffleUI>();
			_monoLevelInfo       = transform.Find("mono_levelInfo").GetComponent<UI.LevelInfoUI>();
			_monoDealCardAI      = transform.Find("mono_DealCardAI").GetComponent<UI.DealCardAIUI>();
			_monoSkillCards      = transform.Find("mono_SkillCards").GetComponent<UI.SkillCardsUI>();
			_monoDealPlayerCard  = transform.Find("mono_DealPlayerCard").GetComponent<UI.DealCardPlayerUI>();
			_monoGuessOrRemember = transform.Find("mono_GuessOrRemember").GetComponent<UI.GuessOrRememberUI>();
		}


		//AUTO-GENERATE-END
		XAttachMachine _xAttachMachine;

		protected override void OnAwake()
		{
			base.OnAwake();
			_xAttachMachine = new XAttachMachine(this);
			_xAttachMachine.RegisterState(new GuessOrRememberState());
			_xAttachMachine.RegisterState(new ShuffleUIState());
			_xAttachMachine.RegisterState(new DealCardUIState());
			_xAttachMachine.StartMachine(ShuffleUIState.StateIDStr);
		}

        #region UI元素

		public GuessOrRememberUI GuessOrRememberPanel => _monoGuessOrRemember;
		public ShuffleUI         ShuffleUI            => _monoShuffleUI;
		public DealCardAIUI      DealCardAIUI         => _monoDealCardAI;
		public DealCardPlayerUI  DealCardPlayerUI     => _monoDealPlayerCard;
		public BetUI             BetUI                => _monoBetPanel;
		public LevelInfoUI       LevelInfoUI          => _monoLevelInfo;
		public SkillCardsUI      SkillCardsUI         => _monoSkillCards;

        #endregion
	}
}
