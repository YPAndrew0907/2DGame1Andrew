using AttachMachine;
using Base;
using Mgr;
using UnityEngine;

namespace UI
{
    public class HomeUI : BaseViewMono
    {
    	//AUTO-GENERATE
    	private UnityEngine.UI.Button _btnLastLevel;
    	private UnityEngine.UI.Button BtnLastLevel 
    			=> _btnLastLevel ??= transform.Find("go_bg/btn_LastLevel").GetComponent<UnityEngine.UI.Button>();

    	private UnityEngine.UI.Button _btnNextLevel;
    	private UnityEngine.UI.Button BtnNextLevel 
    			=> _btnNextLevel ??= transform.Find("go_bg/btn_NextLevel").GetComponent<UnityEngine.UI.Button>();

    	private UnityEngine.UI.Button _btnQuit;
    	private UnityEngine.UI.Button BtnQuit 
    			=> _btnQuit ??= transform.Find("go_bg/RightBtns/btn_quit").GetComponent<UnityEngine.UI.Button>();

    	private UnityEngine.UI.Button _btnSkillList;
    	private UnityEngine.UI.Button BtnSkillList 
    			=> _btnSkillList ??= transform.Find("go_bg/RightBtns/btn_SkillList").GetComponent<UnityEngine.UI.Button>();

    	private UnityEngine.UI.Button _btnStart;
    	private UnityEngine.UI.Button BtnStart 
    			=> _btnStart ??= transform.Find("go_bg/RightBtns/btn_Start").GetComponent<UnityEngine.UI.Button>();

    	private UnityEngine.GameObject _goBg;
    	private UnityEngine.GameObject GoBg 
    			=> _goBg ??= transform.Find("go_bg").gameObject;

    	private TMPro.TextMeshProUGUI _txtLevel;
    	private TMPro.TextMeshProUGUI TxtLevel 
    			=> _txtLevel ??= transform.Find("go_bg/txt_level").GetComponent<TMPro.TextMeshProUGUI>();

    	//AUTO-GENERATE-END
	    public void Init()
	    {
		    // BtnLastLevel.gameObject.SetActive(false);
		    // BtnNextLevel.gameObject.SetActive(false);
		    BtnQuit.onClick.AddListener(OnQuitClick);
		    BtnStart.onClick.AddListener(OnStartClick);
		    BtnLastLevel.onClick.AddListener(OnLastLevelClick);
		    BtnNextLevel.onClick.AddListener(OnNextLevelClick);
		    BtnSkillList.onClick.AddListener(OnSkillListClick);
	    }
	    private void Refresh()
	    {
		    TxtLevel.text = "Level: __" + LevelMgr.Instance.CurrentLevel;
	    } 
	    public void ShowUI()
	    {
		    Refresh();
		    GoBg.SetActive(true);
	    }
	    public void HideUI()
	    {
		    GoBg.SetActive(false);
	    }
		private void OnDestroy()
		{
			BtnQuit.onClick.RemoveAllListeners();
			BtnStart.onClick.RemoveAllListeners();
			BtnLastLevel.onClick.RemoveAllListeners();
			BtnNextLevel.onClick.RemoveAllListeners();
			BtnSkillList.onClick.RemoveAllListeners();
		}
		private void OnStartClick()
		{
			if (PlayerProfileMgr.Instance.Money>= LevelMgr.Instance.CurMinBetChip)
			{
				GameSessionMgr.Instance.InitSession(PlayerProfileMgr.Instance.Money, LevelMgr.Instance.BossChip,
					SkillMgr.Instance.UnLockSkillList(), LevelMgr.Instance.LevelBossSkill);
				XAttachMachine.ActiveAll();
				
				XAttachMachine.SwitchState(HomeUIState.StateIDStr, BetUIState.StateIDStr,BetUIState.StateIDStr);
			}
			else
			{
				NotifyMgr.SendEvent(NotifyDefine.NOTICE,"筹码不够，无法开始游戏");
			}
		}
		private void OnQuitClick()
		{
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
			#else 
			Application.Quit();
			#endif
		}

		private void OnSkillListClick()
		{
			XAttachMachine.SwitchState(HomeUIState.StateIDStr, SkillUpgradeUIState.StateIDStr);
		}

		private void OnLastLevelClick()
		{
			LevelMgr.Instance.SetCurrentLevel(LevelMgr.Instance.CurrentLevel - 1);
			Refresh();
		}
		private void OnNextLevelClick()
		{
			LevelMgr.Instance.SetCurrentLevel(LevelMgr.Instance.CurrentLevel + 1);
			Refresh();
		}
    }
}
