namespace Obj
{
    public enum PlayerSkill
    {
        None,
        NoInit= 1 << 1,
        
        /// <summary>
        /// 集中观察（猜测手牌大致范围）
        /// 被动
        /// </summary>
        Guess = 1 << 2,
        
        /// <summary>
        /// 记忆（洗牌时记忆几张牌的位置）
        /// 洗牌主动，后续被动
        /// </summary>
        Remember = 1 << 3,
        GuessOrRemember = Guess | Remember | NoInit,
        
        /// <summary>
        /// 开局带一张牌
        /// 主动
        /// </summary>
        Copy = 1 << 4,
        
        /// <summary>
        /// 自己回合进行更换
        /// 主动
        /// </summary>
        Switch = 1 << 5 ,
        
        // 神速伸缩。开局带一张牌，自己回合可以进行更换。
        CopyAndSwitch = Copy | Switch | NoInit ,
        
        /// <summary>
        /// 自然谎言
        /// 主动
        /// </summary> 
        Lie = 1 << 6,
        
        /// <summary>
        /// 灵活之指（洗牌时偷牌或者放入）
        /// 主动
        /// </summary>
        StealAndInsert = 1 << 7,
        
        /// <summary>
        /// 识破（当对手出千时，有几率获得微小的提示）
        /// 被动
        /// </summary>
        /// 当对手出千时，你有几率获得一个微小的提示。若你成功发现并点击，你成功识破对手的千术，并使其承受出千惩罚。
        Detect = 1<<8
    }
}