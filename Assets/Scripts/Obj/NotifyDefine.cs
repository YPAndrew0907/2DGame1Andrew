public static class NotifyDefine
{
    private static class EventDefine
    {
        private static int _eventId;
        public static  int eventId => _eventId++;
    }

    #region 流程
    public static readonly int GAME_START = EventDefine.eventId;
    
    public static readonly int ROUND_START = EventDefine.eventId;
    public static readonly int ROUND_END = EventDefine.eventId;
    public static readonly int ROUND_NEXT = EventDefine.eventId;
    
    public static readonly int GAME_END = EventDefine.eventId;
    #endregion
    
    #region 特殊时刻
    public static readonly int SHUFFLE = EventDefine.eventId;
    
    #endregion
    
}