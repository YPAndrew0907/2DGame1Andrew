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
    public static readonly int SKILL_CARD_SELECT_END   = EventDefine.eventId;
    public static readonly int MONEY_CHANGE            = EventDefine.eventId;

    #endregion
}