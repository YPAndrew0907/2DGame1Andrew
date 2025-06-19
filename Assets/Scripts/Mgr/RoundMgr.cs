using Cfg;
using Obj;
using UnityEngine;
using XYZFrameWork.Base;

namespace Mgr
{
    public class RoundMgr : BaseSingle<RoundMgr>
    {
        public static  int CurRound { get; private set; }
        
        public RoundMgr()
        {
            CurRound = 0;
        }
        public bool HasEnd()
        {
             return CurRound >= GameCfg.MaxRound;
        }

        // 是否是洗牌回合 
        public bool ShuffleRound()
        {
            return CurRound % GameCfg.ShuffleRoundTime == 0;
        }

        // 是否是玩家先手
        public bool RandomPlayerFirst()
        {
            return Random.Range(0f, 1.0f) > 0.5f;
        }
    }
    
}