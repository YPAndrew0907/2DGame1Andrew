using AttachMachine;
using Base;
using Mgr;
using Obj;
using UnityEngine;
namespace UI
{
    public class QuitUI : BaseViewMono
    {
    	//AUTO-GENERATE
    	private UnityEngine.UI.Button _btnCancel;
    	private UnityEngine.UI.Button BtnCancel 
    			=> _btnCancel ??= transform.Find("go_bg/btns/btn_Cancel").GetComponent<UnityEngine.UI.Button>();

    	private UnityEngine.UI.Button _btnGiveUp;
    	private UnityEngine.UI.Button BtnGiveUp 
    			=> _btnGiveUp ??= transform.Find("go_bg/btns/go_btn_GiveUp").GetComponent<UnityEngine.UI.Button>();

    	private UnityEngine.UI.Button _btnQuit;
    	private UnityEngine.UI.Button BtnQuit 
    			=> _btnQuit ??= transform.Find("go_bg/btns/btn_Quit").GetComponent<UnityEngine.UI.Button>();

    	private UnityEngine.GameObject _goBg;
    	private UnityEngine.GameObject GoBg 
    			=> _goBg ??= transform.Find("go_bg").gameObject;

    	private UnityEngine.GameObject _goGiveUp;
    	private UnityEngine.GameObject GoGiveUp 
    			=> _goGiveUp ??= transform.Find("go_bg/btns/go_btn_GiveUp").gameObject;

    	//AUTO-GENERATE-END
        private void Awake()
        {
            // 注册按钮点击事件
            BtnCancel.onClick.AddListener(OnCancelClick);
            BtnQuit.onClick.AddListener(OnQuitClick);
            BtnGiveUp.onClick.AddListener(OnGiveUp);
            // 初始状态隐藏窗口
            GoBg.SetActive(false);
        }
        private void Update()
        {
            // 检测 Esc 键按下
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleUI();
            }
        }
        private void OnDestroy()
        {
            // 移除按钮点击事件
            BtnCancel.onClick.RemoveAllListeners();
            BtnQuit.onClick.RemoveAllListeners();
        }
        private void ToggleUI()
        {
            // 切换窗口显示状态
            GoBg.SetActive(!GoBg.activeSelf);
            if (GameSessionMgr.Instance.PlayerChips == 0)
            {
                GoGiveUp.SetActive(false);
            }
            else
            {
                GoGiveUp.SetActive(true);
            }
        }
        private void OnCancelClick()
        {
            // 点击取消按钮关闭窗口
            GoBg.SetActive(false);
        }
        private void OnQuitClick()
        {
            // 点击退出按钮关闭游戏
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else 
            Application.Quit();
#endif
        }
        private void OnGiveUp()
        {
            GoBg.SetActive(false);
            XAttachMachine.SwitchState(null, GameEndUIState.StateIDStr, GameEndCode.GiveUp);
        }
    }
}
