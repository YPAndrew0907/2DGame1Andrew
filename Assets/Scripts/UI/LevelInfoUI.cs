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
			if (money <= 0)return;
			else
			{
				_txtMoney.text = money.ToString();
			}
		}
		public void SetSkillName(string skillName)
		{
			if (string.IsNullOrEmpty(skillName))
			{
				_goSkill.SetActive(false);
			}
			else
			{
				_txtSkillName.text = skillName;
			}
		}
    }
}
