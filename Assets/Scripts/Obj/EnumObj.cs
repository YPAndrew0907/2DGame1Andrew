namespace Obj
{
    public enum GameEndCode
    {
        None,
        Win,
        Lose,
        GiveUp,
    }

    public enum PlayerType
    {
        None = 0,
        Public, // 已使用的牌堆。
        Player,
        AI,
    }
}