using AttachMachine;
using Base;
using Mgr;
using XYZFrameWork;
namespace UI
{
	public class GameSceneUI : BaseViewMono, IMachineMaster,
	                           IHomeUIState, ITotalCardHeapUI, ISkillUI, IShuffleUIState, IDealCardUIState, IBetUI, IPlayerInfoUIState, IAskCardUIState, IPlayedCardUIState, ICompareCardUIState, IGameEndUIState,INoticeMsgState
	{
		//AUTO-GENERATE
		private UI.AskCardUI _monoAskUI;
		private UI.AskCardUI MonoAskUI 
				=> _monoAskUI ??= transform.Find("mono_AskUI").GetComponent<UI.AskCardUI>();

		private UI.BetUI _monoBetPanel;
		private UI.BetUI MonoBetPanel 
				=> _monoBetPanel ??= transform.Find("mono_BetPanel").GetComponent<UI.BetUI>();

		private UI.CompareCardUI _monoCompareCardUI;
		private UI.CompareCardUI MonoCompareCardUI 
				=> _monoCompareCardUI ??= transform.Find("mono_CompareCardUI").GetComponent<UI.CompareCardUI>();

		private UI.DealCardAIUI _monoDealCardAI;
		private UI.DealCardAIUI MonoDealCardAI 
				=> _monoDealCardAI ??= transform.Find("mono_DealCardAI").GetComponent<UI.DealCardAIUI>();

		private UI.DealCardPlayerUI _monoDealPlayerCard;
		private UI.DealCardPlayerUI MonoDealPlayerCard 
				=> _monoDealPlayerCard ??= transform.Find("mono_DealPlayerCard").GetComponent<UI.DealCardPlayerUI>();

		private UI.GameLossUI _monoLossUI;
		private UI.GameLossUI MonoLossUI 
				=> _monoLossUI ??= transform.Find("mono_LossUI").GetComponent<UI.GameLossUI>();

		private UI.GameWinUI _monoWinUI;
		private UI.GameWinUI MonoWinUI 
				=> _monoWinUI ??= transform.Find("mono_WinUI").GetComponent<UI.GameWinUI>();

		private UI.GuessOrRememberUI _monoGuessOrRemember;
		private UI.GuessOrRememberUI MonoGuessOrRemember 
				=> _monoGuessOrRemember ??= transform.Find("mono_GuessOrRemember").GetComponent<UI.GuessOrRememberUI>();

		private UI.HomeUI _monoHomeUI;
		private UI.HomeUI MonoHomeUI 
				=> _monoHomeUI ??= transform.Find("mono_HomeUI").GetComponent<UI.HomeUI>();

		private UI.LevelInfoUI _monoLevelInfo;
		private UI.LevelInfoUI MonoLevelInfo 
				=> _monoLevelInfo ??= transform.Find("mono_LevelInfo").GetComponent<UI.LevelInfoUI>();

		private UI.NoticeMsgUI _monoNoticeMsgUI;
		private UI.NoticeMsgUI MonoNoticeMsgUI 
				=> _monoNoticeMsgUI ??= transform.Find("mono_NoticeMsgUI").GetComponent<UI.NoticeMsgUI>();

		private UI.PlayedCardUI _monoPlayedCardUI;
		private UI.PlayedCardUI MonoPlayedCardUI 
				=> _monoPlayedCardUI ??= transform.Find("mono_PlayedCardUI").GetComponent<UI.PlayedCardUI>();

		private UI.SkillCardsUI _monoSkillCards;
		private UI.SkillCardsUI MonoSkillCards 
				=> _monoSkillCards ??= transform.Find("mono_SkillCards").GetComponent<UI.SkillCardsUI>();

		private UI.TotalCardHeapUI _monoTotalCardHeap;
		private UI.TotalCardHeapUI MonoTotalCardHeap 
				=> _monoTotalCardHeap ??= transform.Find("mono_TotalCardHeap").GetComponent<UI.TotalCardHeapUI>();

		//AUTO-GENERATE-END
		protected void Awake()
		{
			XAttachMachine.SetMaster(this);
			XAttachMachine.RegisterState(new AskCardUIState());
			XAttachMachine.RegisterState(new BetUIState());
			XAttachMachine.RegisterState(new CompareCardUIState());
			XAttachMachine.RegisterState(new DealCardUIState());
			XAttachMachine.RegisterState(new GameEndUIState());
			XAttachMachine.RegisterState(new HomeUIState());
			XAttachMachine.RegisterState(new NoticeMsgUIState());
			XAttachMachine.RegisterState(new PlayerInfoUIState());
			XAttachMachine.RegisterState(new PlayedCardUIState());
			XAttachMachine.RegisterState(new SkillUIState());
			XAttachMachine.RegisterState(new ShuffleUIState());
			XAttachMachine.RegisterState(new TotalCardHeapUIState());
			// 进入首页状态
			
			XAttachMachine.StartMachine(HomeUIState.StateIDStr);
			NotifyMgr.RegisterNotify(NotifyDefine.GAME_READY, OnGameReady);
			NotifyMgr.RegisterNotify(NotifyDefine.GAME_END_BACK_HOME, OnGameEndBackHome);
		}
		private void Update()
		{
			XAttachMachine.Instance.Update();
		}
		private void OnGameReady(NotifyMsg obj)
		{
			XAttachMachine.ActiveAll();
			CoroutineMgr.Instance.StartCoroutine(XAttachMachine.SwitchState(HomeUIState.StateIDStr,
				BetUIState.StateIDStr));
		}
		private void OnGameEndBackHome(NotifyMsg obj)
		{
			XAttachMachine.InActiveAll();
			CoroutineMgr.Instance.StartCoroutine(XAttachMachine.EnterState(HomeUIState.StateIDStr));
		}
        #region UI元素
		public GuessOrRememberUI GuessOrRememberUI => MonoGuessOrRemember;
		public TotalCardHeapUI   TotalCardHeapUI   => MonoTotalCardHeap;
		public DealCardAIUI      DealCardAIUI      => MonoDealCardAI;
		public DealCardAIUI      AICardUI          => MonoDealCardAI;
		public DealCardPlayerUI  DealCardPlayerUI  => MonoDealPlayerCard;
		public DealCardPlayerUI  PlayerUI          => MonoDealPlayerCard;
		public BetUI             BetUI             => MonoBetPanel;
		public LevelInfoUI       LevelInfoUI       => MonoLevelInfo;
		public SkillCardsUI      SkillCardsUI      => MonoSkillCards;
		public GameLossUI        GameLossUI        => MonoLossUI;
		public GameWinUI         GameWinUI         => MonoWinUI;
		public HomeUI            HomeUI            => MonoHomeUI;
		public AskCardUI         AskCardUI         => MonoAskUI;
		public PlayedCardUI      PlayedCardUI      => MonoPlayedCardUI;
		public TotalCardHeapUI   ShuffleUIState    => TotalCardHeapUI;
		public CompareCardUI     CompareCardUI     => MonoCompareCardUI;
		public NoticeMsgUI       NoticeMsgUI       => MonoNoticeMsgUI;
        #endregion
	}
}
