using System.Collections.Generic;
using XYZFrameWork.Base;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Base;
namespace UI
{
    public class LevelInfoUI : BaseViewMono
    {
    	//AUTO-GENERATE
    	private UnityEngine.GameObject _goBg;
    	private UnityEngine.GameObject GoBg 
    			=> _goBg ??= transform.Find("go_bg").gameObject;

    	private UnityEngine.GameObject _goMoney;
    	private UnityEngine.GameObject GoMoney 
    			=> _goMoney ??= transform.Find("go_bg/skillAndMoney/go_money").gameObject;

    	private UnityEngine.GameObject _goSkill;
    	private UnityEngine.GameObject GoSkill 
    			=> _goSkill ??= transform.Find("go_bg/skillAndMoney/go_skill").gameObject;

    	private TMPro.TextMeshProUGUI _txtLevel;
    	private TMPro.TextMeshProUGUI TxtLevel 
    			=> _txtLevel ??= transform.Find("go_bg/txt_level").GetComponent<TMPro.TextMeshProUGUI>();

    	private TMPro.TextMeshProUGUI _txtMoney;
    	private TMPro.TextMeshProUGUI TxtMoney 
    			=> _txtMoney ??= transform.Find("go_bg/skillAndMoney/go_money/txt_money").GetComponent<TMPro.TextMeshProUGUI>();

    	private TMPro.TextMeshProUGUI _txtSkillName;
    	private TMPro.TextMeshProUGUI TxtSkillName 
    			=> _txtSkillName ??= transform.Find("go_bg/skillAndMoney/go_skill/txt_skillName").GetComponent<TMPro.TextMeshProUGUI>();

    	//AUTO-GENERATE-END
		
		
		public void SetLevel(int level)
		{
			TxtLevel.text = $"赌局信息：Level{level}" ;
		}
		
		public void SetMoney(int money)
		{
			if (money <= 0)
			{
				GoMoney.SetActive(false);
				TxtMoney.text = "";
			}
			else
			{
				TxtMoney.text = money.ToString();
			}
		}
		public void SetSkillName(List<string> skillNames)
		{
			if (skillNames is not {Count : > 0 })
			{
				GoSkill.SetActive(false);
				TxtSkillName.text = "";
				
			}
			else
			{
				var str = "";
				foreach (var termStr in skillNames)
				{
					str += termStr;
				}
				TxtSkillName.text = str;
			}
		}
		public void Init()
		{
			GoBg.SetActive(false);
		}
		public void Hide()
		{
			GoBg.SetActive(false);
		}
		public void ShowUI(int level,int money,List<string> skillName)
		{
			SetLevel(level);
			SetMoney(money);
			SetSkillName(skillName);
			GoBg.SetActive(true);
		}
    }
}
