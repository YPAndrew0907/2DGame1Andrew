public static class NotifyDefine
{
    private static class EventDefine
    {
        private static int _eventId;
        public static  int eventId => _eventId++;
    }

    #region 流程

    public static readonly int GAME_READY = EventDefine.eventId;
    public static readonly int GAME_START = EventDefine.eventId;

    public static readonly int ROUND_START = EventDefine.eventId;
    public static readonly int ROUND_END   = EventDefine.eventId;
    public static readonly int ROUND_NEXT  = EventDefine.eventId;

    public static readonly int GAME_END           = EventDefine.eventId;
    public static readonly int GAME_END_BACK_HOME = EventDefine.eventId;

    #endregion

    #region 特殊时刻

    public static readonly int SHUFFLE_START           = EventDefine.eventId;
    public static readonly int SHUFFLE_END             = EventDefine.eventId;
    
    public static readonly int SKILL_CARD_SELECT_START = EventDefine.eventId;
    public static readonly int SKILL_CARD_SELECT       = EventDefine.eventId;
    public static readonly int SKILL_CARD_SELECT_END   = EventDefine.eventId;
    
    public static readonly int MONEY_CHANGE            = EventDefine.eventId;
    public static readonly int ASK_CARD                = EventDefine.eventId;

    public static readonly int DEAL_CARD  = EventDefine.eventId;
    
    public static readonly int BET_CHIP   = EventDefine.eventId;
    public static readonly int NEXT_ROUND = EventDefine.eventId;
    #endregion

    #region  XAttachMachine

    public static readonly int X_ATTACH_MACHINE_ACTIVE_STATE = EventDefine.eventId;
    public static readonly int X_ATTACH_MACHINE_ENTER_STATE  = EventDefine.eventId;
    public static readonly int X_ATTACH_MACHINE_EXIT_STATE   = EventDefine.eventId;

    #endregion
}