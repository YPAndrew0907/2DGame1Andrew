public static class NotifyDefine
{
    private static class EventDefine
    {
        private static int _eventId;
        public static  int eventId => _eventId++;
    }
    
    #region 地图
    public static readonly int MAP_ENTER = EventDefine.eventId;
    #endregion
    
    
    #region 战斗
    public static readonly int BATTLE_START = EventDefine.eventId;
    
    public static readonly int BATTLE_ROUND_START = EventDefine.eventId;
    public static readonly int BATTLE_ROUND_END = EventDefine.eventId;
    public static readonly int BATTLE_NEXT_ROUND = EventDefine.eventId;
    public static readonly int BATTLE_QUEUE_CHANGE = EventDefine.eventId;
    public static readonly int BATTLE_END = EventDefine.eventId;
    
    #endregion
    
    
}