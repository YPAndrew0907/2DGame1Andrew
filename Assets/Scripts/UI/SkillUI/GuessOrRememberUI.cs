using System.Collections.Generic;
using System.Linq;
using Base;
using Cfg;
using Mgr;
using Obj;
using UnityEngine;
namespace UI
{
    public class GuessOrRememberUI : BaseViewMono
    {
    	//AUTO-GENERATE
    	private UnityEngine.UI.Button _btnGuess;
    	private UnityEngine.UI.Button BtnGuess 
    			=> _btnGuess ??= transform.Find("go_bg/go_SelectSkill/btn_Guess").GetComponent<UnityEngine.UI.Button>();

    	private UnityEngine.UI.Button _btnRemember;
    	private UnityEngine.UI.Button BtnRemember 
    			=> _btnRemember ??= transform.Find("go_bg/go_SelectSkill/btn_Remember").GetComponent<UnityEngine.UI.Button>();

    	private UnityEngine.GameObject _goBg;
    	private UnityEngine.GameObject GoBg 
    			=> _goBg ??= transform.Find("go_bg").gameObject;

    	private UnityEngine.GameObject _goGuess;
    	private UnityEngine.GameObject GoGuess 
    			=> _goGuess ??= transform.Find("go_bg/go_Guess").gameObject;

    	private UnityEngine.GameObject _goRemember;
    	private UnityEngine.GameObject GoRemember 
    			=> _goRemember ??= transform.Find("go_bg/go_Remember").gameObject;

    	private UnityEngine.GameObject _goSelectSkill;
    	private UnityEngine.GameObject GoSelectSkill 
    			=> _goSelectSkill ??= transform.Find("go_bg/go_SelectSkill").gameObject;

    	//AUTO-GENERATE-END
		
		
		public List<CardValue> RememberCards { get; set; }
		public void Init()
		{
			GoBg.SetActive(false);
		}

		public void Hide()
		{
			GoBg.SetActive(false);
		}
		
		public bool CheckSelect()
		{
			var flag = DataMgr.Instance.GuessOrRemember;
			return flag != PlayerSkill.None;
		}
		public void ShowSelectPanel()
		{
			if (!DataMgr.Instance.CurSkills.Contains(PlayerSkill.GuessOrRemember))
			{
				return;
			}
			
			var skill = DataMgr.Instance.GuessOrRemember;
			switch (skill)
			{
				case PlayerSkill.None:
					Debug.LogError("未初始化 GuessOrRemember 技能");
					break;
				case PlayerSkill.Guess:       
					GoGuess.SetActive(true);
					GoRemember.SetActive(false);
					GoSelectSkill.SetActive(false);
					break;
				case PlayerSkill.Remember:   
					GoGuess.SetActive(false);
					GoRemember.SetActive(true);
					GoSelectSkill.SetActive(false);
					break;
				case PlayerSkill.GuessOrRemember:
					GoGuess.SetActive(false);
					GoRemember.SetActive(false);
					GoSelectSkill.SetActive(true);
					break;
				default:                          
					GoBg.SetActive(false);
					return;
			}
			GoBg.SetActive(true);
		}
    }
}
