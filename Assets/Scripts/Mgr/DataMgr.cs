using Cfg;
using UnityEngine;
using XYZFrameWork;
using XYZFrameWork.Base;

namespace Mgr
{
    public class DataMgr: BaseSingle<DataMgr>
    {
        public DataMgr()
        {
            _curLevel = PlayerPrefs.GetInt(CurLevelStr, 1);
            _money = PlayerPrefs.GetInt(MoneyStr, 200);
            NotifyMgr.Register(NotifyDefine.GAME_END,SaveGameInfo);
        }

        ~DataMgr()
        {
            NotifyMgr.UnRegister(NotifyDefine.GAME_END,SaveGameInfo);
        }

        private int _curLevel = 1;
        public int CurLevel=> _curLevel;

        private int _money = 0;
        public int Money=> _money;

        public void SaveGameInfo(NotifyMsg notifyMsg)
        {
            PlayerPrefs.SetInt(CurLevelStr, 1);
            PlayerPrefs.SetInt(MoneyStr,_money);
        }
        
        private const string CurLevelStr = "CurLevel";
        private const string MoneyStr = "MoneyStr";
    }
}