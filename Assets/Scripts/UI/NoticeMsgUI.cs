using Base;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class NoticeMsgUI : BaseViewMono
    {
    	//AUTO-GENERATE
    	private UnityEngine.GameObject _goBg;
    	private UnityEngine.GameObject GoBg 
    			=> _goBg ??= transform.Find("go_Bg").gameObject;

    	private TMPro.TextMeshProUGUI _txtNotice;
    	private TMPro.TextMeshProUGUI TxtNotice 
    			=> _txtNotice ??= transform.Find("go_Bg/txt_Notice").GetComponent<TMPro.TextMeshProUGUI>();

    	//AUTO-GENERATE-END


	    public void Notice(string notice)
	    {
		    TxtNotice.text = notice;
		    GoBg.SetActive(true);
		    var curPos = transform.position;
		    TxtNotice.transform.position = new Vector3(curPos.x,0,curPos.z);
		    TxtNotice.color = Color.black;
		    TxtNotice.transform.DOMoveY(transform.position.y + 200, 1.5f);
		    TxtNotice.DOFade(0, 1.5f).OnComplete(() =>
		    {
			    GoBg.SetActive(false);
		    });
		    
	    }

	    public void Init()
	    {
		    GoBg.SetActive(false);
	    }
    }
}
