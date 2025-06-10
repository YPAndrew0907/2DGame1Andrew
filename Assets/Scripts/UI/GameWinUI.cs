using System;
using System.Collections.Generic;
using System.Linq;
using Base;
using Mgr;
using Obj;
namespace UI
{
	public class GameWinUI : BaseViewMono
	{
		//AUTO-GENERATE
		private UnityEngine.UI.Button _btnBackHome;
		private UnityEngine.UI.Button BtnBackHome 
				=> _btnBackHome ??= transform.Find("go_bg/btn_BackHome").GetComponent<UnityEngine.UI.Button>();

		private UnityEngine.GameObject _goBg;
		private UnityEngine.GameObject GoBg 
				=> _goBg ??= transform.Find("go_bg").gameObject;

		private UnityEngine.GameObject _goGetSkill;
		private UnityEngine.GameObject GoGetSkill 
				=> _goGetSkill ??= transform.Find("go_bg/go_txt_GetSkill").gameObject;

		private TMPro.TextMeshProUGUI _txtGetSkill;
		private TMPro.TextMeshProUGUI TxtGetSkill 
				=> _txtGetSkill ??= transform.Find("go_bg/go_txt_GetSkill").GetComponent<TMPro.TextMeshProUGUI>();

		private TMPro.TextMeshProUGUI _txtInfo;
		private TMPro.TextMeshProUGUI TxtInfo 
				=> _txtInfo ??= transform.Find("go_bg/txt_Info").GetComponent<TMPro.TextMeshProUGUI>();

		private TMPro.TextMeshProUGUI _txtTitle;
		private TMPro.TextMeshProUGUI TxtTitle 
				=> _txtTitle ??= transform.Find("go_bg/txt_Title").GetComponent<TMPro.TextMeshProUGUI>();

		//AUTO-GENERATE-END
		private const string WinStr = "闯关成功";

		public void Show(GameEndCode endCode, int earnMoney, IReadOnlyList<PlayerSkill> skills)
		{
			GoGetSkill.SetActive(false);
			switch (endCode)
			{
				case GameEndCode.Win:
					GoBg.SetActive(true);
					TxtTitle.text = WinStr;
					TxtInfo.text  = $"本关共获得筹码 {earnMoney}";
					if (skills is { Count: > 0 })
					{
						TxtGetSkill.text = String.Join(",", LevelData.GetSkillsDesc(skills));
						GoGetSkill.SetActive(true);
					}

					break;
				default: throw new ArgumentOutOfRangeException(nameof(endCode), endCode, null);
			}
		}

		public void Hide()
		{
			GoBg.SetActive(false);
		}
		public void Init()
		{
			Hide();
			BtnBackHome.onClick.RemoveAllListeners();
			BtnBackHome.onClick.AddListener(OnBackHome);
		}
		private void OnBackHome()
		{
			NotifyMgr.SendEvent(NotifyDefine.CLOSE_GAME_END_UI);
		}
	}
}
