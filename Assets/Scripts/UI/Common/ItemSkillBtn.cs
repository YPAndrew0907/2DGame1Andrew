using System;
using System.Collections;
using Base;
using Mgr;
using Obj;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ItemSkillBtn : BaseMono
    {
    	private TMPro.TextMeshProUGUI _txtSkillName;
    	private TMPro.TextMeshProUGUI TxtSkillName 
    			=> _txtSkillName ??= transform.Find("txt_SkillName").GetComponent<TMPro.TextMeshProUGUI>();
        
	    
	    private Button          _button;
	    private TextMeshProUGUI _btnTxt;

	    private PlayerSkill _skill;
        
	    public void Init(PlayerSkill skill)
	    {
		    _button ??= GetComponent<Button>();
		    _btnTxt ??= transform.GetChild(0).GetComponent<TextMeshProUGUI>();
		    _skill  =   skill;
		    if (_btnTxt != null)
			    _btnTxt.text = LevelData.GetSkillDesc(skill);
		    if (_button != null)
		    {
			    _button.onClick.RemoveAllListeners();
			    _button.onClick.AddListener(OnSkillClick);
		    }
	    }

	    private void OnSkillClick()
	    {
		    StartCoroutine(DelayResumeEnable());
		    Debug.Log($"【放技能】：{_skill}");
		    NotifyMgr.SendEvent(NotifyDefine.SKILL_CLICK, (int)_skill);
	    }

	    private IEnumerator DelayResumeEnable()
	    {
		    if (_button != null)
		    {
			    _button.interactable = false;
			    yield return new WaitForSeconds(0.5f);
			    _button.interactable = true;
		    }
	    }

	    public int SetShow(bool inMyRound)
	    {
		    var show = false;
		    switch (_skill)
		    {
			    case PlayerSkill.Lie:
				    show = !inMyRound;
				    gameObject.SetActive(show);
				    break;
			    case PlayerSkill.Switch:
				    show = inMyRound;
				    break;
			    case PlayerSkill.Detect:
				    show = !inMyRound;
				    gameObject.SetActive(show);
				    break;
		    }
		    gameObject.SetActive(show);
		    return show? 1 : 0;
	    }
    }
}
