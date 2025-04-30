using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Base;
using Mgr;
using UnityEngine.SceneManagement;
using XYZFrameWork.Base;
namespace UI
{
    public class HomeUI : BaseViewMono
    {
		//AUTO-GENERATE
		private Button _btnStart;
		private Button _btnQuit;
		private TextMeshProUGUI _txtLevel;

		protected override void FindUI()
		{
			_btnStart = transform.Find("btn_Start").GetComponent<UnityEngine.UI.Button>();
			 _btnQuit = transform.Find("btn_quit").GetComponent<UnityEngine.UI.Button>();
			_txtLevel = transform.Find("txt_level").GetComponent<TMPro.TextMeshProUGUI>();
		}
		
		//AUTO-GENERATE-END

		private void OnEnable()
		{
			_btnQuit.onClick.AddListener(OnQuitClick);
			_btnStart.onClick.AddListener(OnStartClick);
			_txtLevel.text = "Level: " + DataMgr.Instance.CurLevel;
		}

		private void OnDisable()
		{
			_btnQuit.onClick.RemoveListener(OnQuitClick);
			_btnStart.onClick.RemoveListener(OnStartClick);
		}

		public void OnStartClick()
		{
			SceneManager.LoadScene("Game", LoadSceneMode.Single);
		}

		public void OnQuitClick()
		{
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
			#else 
			Application.Quit();
			#endif
		}
		
    }
}
