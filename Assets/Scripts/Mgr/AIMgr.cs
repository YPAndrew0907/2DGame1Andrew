namespace Mgr
{
    public static class AIMgr
    {
        public static bool AIAskCard(int curNum)
        {
            return curNum < 16;
        }

        public static bool AIIsLoss(int aiNum, int playerNum)
        {
            if (aiNum == 0)
                return true;
            if (playerNum == 0)
                return false;
            if (aiNum > 21)
            {
                if (playerNum <= 21)
                    return true;
                return playerNum < aiNum;
            }
            else
            {
                if (playerNum <= 21)
                    return playerNum >= aiNum;
                return false;
            }
        }
    }
}