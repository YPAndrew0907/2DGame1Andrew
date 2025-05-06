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
		private GameObject _goMoney;
		private GameObject _goSkill;
		private TextMeshProUGUI _txtLevel;
		private TextMeshProUGUI _txtMoney;
		private TextMeshProUGUI _txtSkillName;

		protected override void FindUI()
		{
			     _goMoney = transform.Find("skillAndMoney/go_money").gameObject;
			     _goMoney = transform.Find("skillAndMoney/go_money").GetComponent<UnityEngine.GameObject>();
			     _goSkill = transform.Find("skillAndMoney/go_skill").gameObject;
			     _goSkill = transform.Find("skillAndMoney/go_skill").GetComponent<UnityEngine.GameObject>();
			    _txtLevel = transform.Find("txt_level").GetComponent<TMPro.TextMeshProUGUI>();
			    _txtMoney = transform.Find("skillAndMoney/go_money/txt_money").GetComponent<TMPro.TextMeshProUGUI>();
			_txtSkillName = transform.Find("skillAndMoney/go_skill/txt_skillName").GetComponent<TMPro.TextMeshProUGUI>();
		}


		//AUTO-GENERATE-END
		
		
		public void SetLevel(int level)
		{
			_txtLevel.text = $"赌局信息：Level{level}" ;
		}
		
		public void SetMoney(int money)
		{
			if (money <= 0)
			{
				_goMoney.SetActive(false);
				_txtMoney.text = "";
			}
			else
			{
				_txtMoney.text = money.ToString();
			}
		}
		public void SetSkillName(List<string> skillNames)
		{
			if (skillNames is not {Count : > 0 })
			{
				_goSkill.SetActive(false);
				_txtSkillName.text = "";
				
			}
			else
			{
				var str = "";
				foreach (var termStr in skillNames)
				{
					str += termStr;
				}

				_txtSkillName.text = str;
			}
		}

		public void Init()
		{
			gameObject.SetActive(false);
		}

		public void ShowUI(int level,int money,List<string> skillName)
		{
			SetLevel(level);
			SetMoney(money);
			SetSkillName(skillName);
			gameObject.SetActive(true);
		}
    }
}
