using System;
using Base;
using Cfg;
using Mgr;
using Obj;
using UnityEngine;

namespace UI
{
    public class SkillListItem : BaseViewMono
    {
        //AUTO-GENERATE
        private UnityEngine.UI.Button _btnSkillOpera;

        private UnityEngine.UI.Button BtnSkillOpera =>
            _btnSkillOpera ??= transform.Find("btn_SkillOpera").GetComponent<UnityEngine.UI.Button>();

        private UnityEngine.UI.Image _imgSkillItem;

        private UnityEngine.UI.Image ImgSkillItem =>
            _imgSkillItem ??= transform.Find("img_skillItem").GetComponent<UnityEngine.UI.Image>();

        private TMPro.TextMeshProUGUI _txtSkillDesc;

        private TMPro.TextMeshProUGUI TxtSkillDesc =>
            _txtSkillDesc ??= transform.Find("txt_SkillDesc").GetComponent<TMPro.TextMeshProUGUI>();

        private TMPro.TextMeshProUGUI _txtSkillLevel;

        private TMPro.TextMeshProUGUI TxtSkillLevel =>
            _txtSkillLevel ??= transform.Find("txt_SkillLevel").GetComponent<TMPro.TextMeshProUGUI>();

        private TMPro.TextMeshProUGUI _txtSkillName;

        private TMPro.TextMeshProUGUI TxtSkillName =>
            _txtSkillName ??= transform.Find("txt_SkillName").GetComponent<TMPro.TextMeshProUGUI>();

        //AUTO-GENERATE-END
        public  Action      OnUpgradeOrUnlock; // 外部赋值
        private SkillConfig _skillConfig;      // 原始技能数据
        private int         _curLevel;

        public void Init(SkillConfig skill)
        {
            _skillConfig = skill;
            _curLevel    = SkillMgr.Instance.GetSkillLevel(skill.skillType);
            BtnSkillOpera.onClick.RemoveAllListeners();
            BtnSkillOpera.onClick.AddListener(OnOperateClick);
            RefreshUI();
        }

        private void RefreshUI()
        {
            if (_skillConfig == null)
                return;

            TxtSkillName.text  = _skillConfig.name ?? "未知技能";
            TxtSkillLevel.text = $"Lv.{_curLevel}/5";

            var config = SkillMgr.Instance.GetSkillConfig(_skillConfig.skillType);

            string desc = string.Empty;

            var (hitRate, parameter) = SkillMgr.Instance.GetSkillParameters(_skillConfig.skillType, _curLevel);

            switch (_skillConfig.skillType)
            {
                case PlayerSkill.CopyAndSwitch:
                case PlayerSkill.GuessOrRemember:
                case PlayerSkill.StealAndInsert:
                    if (_curLevel > 1)
                    {
                        desc = string.Format(_skillConfig.desc,
                            FormatWithUpgrade(config.baseHitRate, hitRate),
                            config.hitRateIncreasePerLevel.ToString("P0"),
                            FormatWithUpgrade(config.baseParameter, parameter),
                            config.parameterIncreasePerLevel);
                    }
                    else
                    {
                        desc = string.Format(_skillConfig.desc,
                            config.baseHitRate.ToString("P0"),
                            config.hitRateIncreasePerLevel.ToString("P0"),
                            config.baseParameter,
                            config.parameterIncreasePerLevel);
                    }
                    break;

                case PlayerSkill.Lie:
                case PlayerSkill.Detect:
                    if (_curLevel > 1)
                    {
                        desc = string.Format(_skillConfig.desc,
                            FormatWithUpgrade(config.baseHitRate, hitRate),
                            config.hitRateIncreasePerLevel.ToString("P0"));
                    }
                    else
                    {
                        desc = string.Format(_skillConfig.desc,
                            config.baseHitRate.ToString("P0"),
                            config.hitRateIncreasePerLevel.ToString("P0"));
                    }
                    break;

                default:
                    Debug.LogError(LogTxt.PARAM_ERROR);
                    break;
            }


            TxtSkillDesc.text = desc;

            if (ImgSkillItem != null)
            {
                var sprite = Resources.Load<Sprite>($"SkillIcons/{_skillConfig.name}");
                if (sprite != null)
                    ImgSkillItem.sprite = sprite;
            }

            var btnTxt = BtnSkillOpera.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (btnTxt != null)
            {
                btnTxt.text = SkillMgr.Instance.IsUnLock(_skillConfig.skillType) ? "升级" : "未解锁";
                BtnSkillOpera.interactable = SkillMgr.Instance.IsUnLock(_skillConfig.skillType)
                                             && SkillMgr.Instance.SkillPoint > 0;
            }
        }

        private string FormatWithUpgrade(float baseValue, float upgradedValue)
        {
            if (baseValue == upgradedValue)
                return baseValue.ToString("P0");

            return $"{baseValue:P0} <color=#00FF00>(+{(upgradedValue - baseValue):P0})</color>";
        }

        private string FormatWithUpgrade(int baseValue, int upgradedValue)
        {
            if (baseValue == upgradedValue)
                return baseValue.ToString();

            return $"{baseValue} <color=#00FF00>(+{upgradedValue - baseValue})</color>";
        }

        private void OnOperateClick()
        {
            if (SkillMgr.Instance.IsUnLock(_skillConfig.skillType))
            {
                if (SkillMgr.Instance.SkillPoint <= 0)
                {
                    Debug.Log("技能点不足，无法升级技能");
                    return;
                }

                SkillMgr.Instance.SpendSkillPoint(1);
                SkillMgr.Instance.UpgradeSkill(_skillConfig.skillType);
                _curLevel = SkillMgr.Instance.GetSkillLevel(_skillConfig.skillType);
            }
            else
            {
                NotifyMgr.SendEvent(NotifyDefine.NOTICE, "技能未解锁");
                Debug.Log("技能未解锁，无法升级");
            }

            RefreshUI();
            OnUpgradeOrUnlock?.Invoke();
        }

        public void SetActive(bool isActive)
        {
            BtnSkillOpera.interactable = isActive;
        }
    }
}