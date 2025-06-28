using AttachMachine;
using Base;
using Mgr;
using XYZFrameWork;
namespace UI
{
	public class GameSceneUI : BaseViewMono, IMachineMaster,
	                           IHomeUIState, ITotalCardHeapUI, ISkillUI, IShuffleUIState, IDealCardUIState, IBetUI,
	                           IPlayerInfoUIState, IAskCardUIState, IPlayedCardUIState, ICompareCardUIState,
	                           ISkillUpgradeUIState,
	                           IGameEndUIState, INoticeMsgState
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

		private UI.DealCardPlayerUI _monoDealCardPlayer;
		private UI.DealCardPlayerUI MonoDealCardPlayer 
				=> _monoDealCardPlayer ??= transform.Find("mono_DealCardPlayer").GetComponent<UI.DealCardPlayerUI>();

		private UI.GameLossUI _monoLossUI;
		private UI.GameLossUI MonoLossUI 
				=> _monoLossUI ??= transform.Find("mono_LossUI").GetComponent<UI.GameLossUI>();

		private UI.GameWinUI _monoWinUI;
		private UI.GameWinUI MonoWinUI 
				=> _monoWinUI ??= transform.Find("mono_WinUI").GetComponent<UI.GameWinUI>();

		private UI.HomeUI _monoHomeUI;
		private UI.HomeUI MonoHomeUI 
				=> _monoHomeUI ??= transform.Find("mono_HomeUI").GetComponent<UI.HomeUI>();

		private UI.InsertAndReplaceUI _monoInsertAndReplace;
		private UI.InsertAndReplaceUI MonoInsertAndReplace 
				=> _monoInsertAndReplace ??= transform.Find("mono_InsertAndReplace").GetComponent<UI.InsertAndReplaceUI>();

		private UI.LevelInfoUI _monoLevelInfo;
		private UI.LevelInfoUI MonoLevelInfo 
				=> _monoLevelInfo ??= transform.Find("mono_LevelInfo").GetComponent<UI.LevelInfoUI>();

		private UI.NoticeMsgUI _monoNoticeMsgUI;
		private UI.NoticeMsgUI MonoNoticeMsgUI 
				=> _monoNoticeMsgUI ??= transform.Find("mono_NoticeMsgUI").GetComponent<UI.NoticeMsgUI>();

		private UI.PlayedCardUI _monoPlayedCardUI;
		private UI.PlayedCardUI MonoPlayedCardUI 
				=> _monoPlayedCardUI ??= transform.Find("mono_PlayedCardUI").GetComponent<UI.PlayedCardUI>();

		private UI.SelectCardUI _monoSelectCardUI;
		private UI.SelectCardUI MonoSelectCardUI 
				=> _monoSelectCardUI ??= transform.Find("mono_SelectCardUI").GetComponent<UI.SelectCardUI>();

		private UI.SelectSkillUI _monoSelectSkillUI;
		private UI.SelectSkillUI MonoSelectSkillUI 
				=> _monoSelectSkillUI ??= transform.Find("mono_SelectSkillUI").GetComponent<UI.SelectSkillUI>();

		private UI.SkillsUI _monoSkillUI;
		private UI.SkillsUI MonoSkillUI 
				=> _monoSkillUI ??= transform.Find("mono_SkillUI").GetComponent<UI.SkillsUI>();

		private UI.SkillUpgradeUI _monoSkillUpgradeUI;
		private UI.SkillUpgradeUI MonoSkillUpgradeUI 
				=> _monoSkillUpgradeUI ??= transform.Find("mono_SkillUpgradeUI").GetComponent<UI.SkillUpgradeUI>();

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
			XAttachMachine.RegisterState(new SkillUpgradeUIState());
			XAttachMachine.RegisterState(new ShuffleUIState());
			XAttachMachine.RegisterState(new TotalCardHeapUIState());
			// 进入首页状态
			
			NotifyMgr.RegisterNotify(NotifyDefine.GAME_END_BACK_HOME, OnGameEndBackHome);
			_ = CardMgr.Instance;
			_ = SkillMgr.Instance;
			_ = LevelMgr.Instance;
			_ = GameSessionMgr.Instance;
			_ = PlayerProfileMgr.Instance;
			
			XAttachMachine.StartMachine(HomeUIState.StateIDStr);
		}
		private void Update()
		{
			XAttachMachine.Instance.Update();
		}
		private void OnGameEndBackHome(NotifyMsg obj)
		{
			XAttachMachine.InActiveAll();
			XAttachMachine.StartMachine(HomeUIState.StateIDStr);
		}
        #region UI元素
		public InsertAndReplaceUI InsertAndReplaceUI => MonoInsertAndReplace;
		public SelectCardUI       SelectCardUI       => MonoSelectCardUI;
		public SelectSkillUI      SelectSkillUI      => MonoSelectSkillUI;
		public TotalCardHeapUI    TotalCardHeapUI    => MonoTotalCardHeap;
		public DealCardAIUI       DealCardAIUI       => MonoDealCardAI;
		public DealCardAIUI       AICardUI           => MonoDealCardAI;
		public DealCardPlayerUI   DealCardPlayerUI   => MonoDealCardPlayer;
		public DealCardPlayerUI   PlayerUI           => MonoDealCardPlayer;
		public BetUI              BetUI              => MonoBetPanel;
		public LevelInfoUI        LevelInfoUI        => MonoLevelInfo;
		public SkillsUI           SkillsUI           => MonoSkillUI;
		public GameLossUI         GameLossUI         => MonoLossUI;
		public GameWinUI          GameWinUI          => MonoWinUI;
		public HomeUI             HomeUI             => MonoHomeUI;
		public AskCardUI          AskCardUI          => MonoAskUI;
		public PlayedCardUI       PlayedCardUI       => MonoPlayedCardUI;
		public TotalCardHeapUI    ShuffleUI          => TotalCardHeapUI;
		public CompareCardUI      CompareCardUI      => MonoCompareCardUI;
		public NoticeMsgUI        NoticeMsgUI        => MonoNoticeMsgUI;
		public SkillUpgradeUI     SkillUpgradeUI     => MonoSkillUpgradeUI;
        #endregion
	}
}
