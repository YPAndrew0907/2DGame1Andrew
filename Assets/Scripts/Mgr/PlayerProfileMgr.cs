using System.Collections.Generic;
using Obj;
using UnityEngine;
using XYZFrameWork.Base;

namespace Mgr
{
    /// <summary>
    /// 玩家进度、金币、技能等账号级数据
    /// </summary>
    public class PlayerProfileMgr : BaseSingle<PlayerProfileMgr>
    {
        private const string Key_Money      = "Profile_Money";
        public int Money
        {
            get => _money;
            set
            {
                _money = value;
                NotifyMgr.SendEvent(NotifyDefine.MONEY_CHANGE, _money);
                SaveProfile();
            }
        }
        private int _money;

        public PlayerProfileMgr()
        {
            LoadProfile();
        }


        public void LoadProfile()
        {
            _money = PlayerPrefs.GetInt(Key_Money, 200);
            if (_money < 50)
            {
                SetMoney(50);
            }
        }

        public void SaveProfile()
        {
            PlayerPrefs.SetInt(Key_Money, _money);
            PlayerPrefs.Save();
        }

        public void SetMoney(int value)
        {
            Money = value;
        }
    }
}
