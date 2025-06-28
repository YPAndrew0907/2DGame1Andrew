using System.Collections.Generic;
using System.Linq;
using Base;
using Mgr;
using Obj;
using UnityEngine;
using UnityEngine.EventSystems;
namespace UI
{
    public class SelectSkillUI : BaseViewMono
    {
    	//AUTO-GENERATE
    	private UnityEngine.UI.Button _btnLeftBtn;
    	private UnityEngine.UI.Button BtnLeftBtn 
    			=> _btnLeftBtn ??= transform.Find("go_bg/SelectSkill/btn_LeftBtn").GetComponent<UnityEngine.UI.Button>();

    	private UnityEngine.UI.Button _btnRightBtn;
    	private UnityEngine.UI.Button BtnRightBtn 
    			=> _btnRightBtn ??= transform.Find("go_bg/SelectSkill/btn_RightBtn").GetComponent<UnityEngine.UI.Button>();

    	private UnityEngine.GameObject _goBg;
    	private UnityEngine.GameObject GoBg 
    			=> _goBg ??= transform.Find("go_bg").gameObject;

    	//AUTO-GENERATE-END

	    private PlayerSkill _leftSkill, _rightSkill;
        public void Init()
        {
            GoBg.SetActive(false);
                    
			BtnLeftBtn.onClick.RemoveAllListeners();
			BtnLeftBtn.onClick.AddListener(OnClickLeftSelect);
			         
            BtnRightBtn.onClick.RemoveAllListeners();
            BtnRightBtn.onClick.AddListener(OnClickRightSelect);
        }
        
        
        public void Show(PlayerSkill leftSkill, PlayerSkill rightSkill)
        {
            _leftSkill = leftSkill;
            _rightSkill = rightSkill;
			
            GoBg.SetActive(true);
        }
        public void Hide()
        {
            GoBg.SetActive(false);
        }
        private void OnClickLeftSelect()
        {
            NotifyMgr.SendEvent(NotifyDefine.SKILL_SELECT,
                new List<int> { (int)PlayerType.Player, (int)_leftSkill });
            Hide();
        }
        private void OnClickRightSelect()
        {
            NotifyMgr.SendEvent(NotifyDefine.SKILL_SELECT,
                new List<int> { (int)PlayerType.Player, (int)_rightSkill });
            Hide();
        }
    }
}
