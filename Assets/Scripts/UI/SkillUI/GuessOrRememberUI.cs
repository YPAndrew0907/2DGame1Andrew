using XYZFrameWork.Base;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
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
		private Button _btnGuess;
		private Button _btnRemember;
		private GameObject _goGuess;
		private GameObject _goRemember;
		private GameObject _goSelectSkill;

		protected override void FindUI()
		{
			     _btnGuess = transform.Find("go_SelectSkill/btn_Guess").GetComponent<UnityEngine.UI.Button>();
			  _btnRemember = transform.Find("go_SelectSkill/btn_Remember").GetComponent<UnityEngine.UI.Button>();
			      _goGuess = transform.Find("go_Guess").gameObject;
			      _goGuess = transform.Find("go_Guess").GetComponent<UnityEngine.GameObject>();
			   _goRemember = transform.Find("go_Remember").gameObject;
			   _goRemember = transform.Find("go_Remember").GetComponent<UnityEngine.GameObject>();
			_goSelectSkill = transform.Find("go_SelectSkill").gameObject;
			_goSelectSkill = transform.Find("go_SelectSkill").GetComponent<UnityEngine.GameObject>();
		}


		//AUTO-GENERATE-END
		
		
		public List<CardValue> RememberCards { get; set; }
		public bool CheckSelect()
		{
			var flag = DataMgr.Instance.GuessOrRemember;
			return flag != PlayerSkill.None;
		}
		public void ShowSelectPanel()
		{
			gameObject.SetActive(true);
			_goGuess.SetActive(false);
			_goRemember.SetActive(false);
			_goSelectSkill.SetActive(true);
		}
    }
}
