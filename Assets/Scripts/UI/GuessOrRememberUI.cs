using XYZFrameWork.Base;
using TMPro;
using UnityEngine.UI;
using Base;
using UnityEngine;
using UnityEngine.Serialization;
namespace UI
{
    public class GuessOrRememberUI : BaseViewMono
    {
		//AUTO-GENERATE
		private GameObject _goGuess;
		private GameObject _goRemember;

		protected override void FindUI()
		{
			   _goGuess = transform.Find("go_Guess").gameObject;
			   _goGuess = transform.Find("go_Guess").GetComponent<UnityEngine.GameObject>();
			_goRemember = transform.Find("go_Remember").gameObject;
			_goRemember = transform.Find("go_Remember").GetComponent<UnityEngine.GameObject>();
		}


		//AUTO-GENERATE-END
        
    }
}
