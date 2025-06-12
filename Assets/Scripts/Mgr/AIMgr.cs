using System;
using System.Collections.Generic;
using System.Linq;
using Obj;
using UnityEngine;
using Random = UnityEngine.Random;

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

        public static int AIRandomGuessOrRemember()
        {
            return (int)(Random.Range(0, 1.0f) > 0.5 ? PlayerSkill.Guess : PlayerSkill.Remember);
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="count">偷几张</param>
        /// <returns>返回索引列表</returns>
        public static List<int> AIRandomStealCard(IReadOnlyList<CardObj> cards, int count)
        {
            var list = new List<int>();
            while (list.Count < count)
            {
                var index = Random.Range(0, cards.Count);
                if (!list.Contains(index))
                    list.Add(index);
            }

            return list;
        }

        public static List<int> AIRandomCopyCard(IReadOnlyList<CardObj> cards, int count)
        {
            var list = new List<int>();
            while (list.Count < count)
            {
                var index = Random.Range(0, cards.Count);
                if (!list.Contains(index))
                    list.Add(index);
            }

            return list;
        }

        //  AI 替换 逻辑。每次只替换一个的前提下。
        public static (List<CardObj> curCardObjs, List<CardObj> repCardObjs) AIReplaceCard(
            List<CardObj> curCards, List<CardObj> skillCards, int replaceNum)
        {
            int originalTotal = curCards.Sum(card => (int)card.Value + 1);
            int originalDelta = originalTotal > 21 ? int.MaxValue : Math.Abs(21 - originalTotal);

            int   bestDelta      = originalDelta;
            int   minReplaceUsed = 0;
            int[] bestCur        = null;
            int[] bestRep        = null;

            for (int n = 1; n <= replaceNum; n++)
            {
                if (n > curCards.Count || n > skillCards.Count)
                    break;

                var curCombos = GetCombinations(n, curCards.Count);
                var repCombos = GetCombinations(n, skillCards.Count);

                foreach (var curIdxArr in curCombos)
                {
                    foreach (var repIdxArr in repCombos)
                    {
                        var tempList = new List<CardObj>(curCards);
                        for (int k = 0; k < n; k++)
                        {
                            tempList[curIdxArr[k]] = skillCards[repIdxArr[k]];
                        }

                        int total = tempList.Sum(card => (int)card.Value + 1);

                        if (total > 21)
                            continue;

                        int delta = Math.Abs(21 - total);

                        bool isBetter = delta < bestDelta
                                        || (delta == bestDelta && n < minReplaceUsed);

                        if (isBetter)
                        {
                            bestDelta      = delta;
                            minReplaceUsed = n;
                            bestCur        = (int[])curIdxArr.Clone();
                            bestRep        = (int[])repIdxArr.Clone();
                        }
                    }
                }
            }

            if (bestCur == null || bestRep == null || minReplaceUsed == 0)
                return (null, null);

            var curResult = bestCur.Select(i => curCards[i]).ToList();
            var repResult = bestRep.Select(i => skillCards[i]).ToList();

            return (curResult, repResult);
        }


        private static List<int[]> GetCombinations(int n, int max)
        {
            var result = new List<int[]>();

            void Combine(int[] arr, int start, int depth)
            {
                if (depth == n)
                {
                    result.Add((int[])arr.Clone());
                    return;
                }

                for (int i = start; i < max; i++)
                {
                    arr[depth] = i;
                    Combine(arr, i + 1, depth + 1);
                }
            }

            Combine(new int[n], 0, 0);
            return result;
        }

        public static bool IsCanDoSkill(float rate)
        {
            return Random.Range(0, 1.0f) < rate;
        }

        public static int RandomBet(int curChip, int minBet, int maxBet)
        {
            if (curChip < minBet)
                return curChip; // 筹码不足最小押注，直接全押。

            int actualMaxBet = Math.Min(curChip, maxBet);

            // 筹码紧张程度：剩余筹码与最大下注的比例
            float pressure = 1f - (float)curChip / (maxBet * 5f); // 假设5倍maxBet是舒适区，越接近0越轻松，越接近1越紧张
            pressure = Mathf.Clamp01(pressure);

            int bet;

            int randomFactor = UnityEngine.Random.Range(0, 100);

            if (pressure < 0.3f) // 筹码很充裕，倾向保守
            {
                if (randomFactor < 70)
                    bet = UnityEngine.Random.Range(minBet, (minBet + actualMaxBet) / 2 + 1);
                else if (randomFactor < 95)
                    bet = UnityEngine.Random.Range((minBet + actualMaxBet) / 2, actualMaxBet + 1);
                else
                    bet = actualMaxBet;
            }
            else if (pressure < 0.7f) // 筹码适中，平衡下注
            {
                if (randomFactor < 50)
                    bet = UnityEngine.Random.Range(minBet, (minBet + actualMaxBet) / 2 + 1);
                else if (randomFactor < 85)
                    bet = UnityEngine.Random.Range((minBet + actualMaxBet) / 2, actualMaxBet + 1);
                else
                    bet = actualMaxBet;
            }
            else // 筹码紧张，All-in的倾向增大
            {
                if (randomFactor < 30)
                    bet = Random.Range(minBet, (minBet + actualMaxBet) / 2 + 1);
                else if (randomFactor < 60)
                    bet = Random.Range((minBet + actualMaxBet) / 2, actualMaxBet + 1);
                else
                    bet = actualMaxBet;
            }

            // 调整为10的倍数
            bet = (bet / 10) * 10;

            // 保证至少是最小下注且不超过当前最大可下注
            bet = Mathf.Clamp(bet, minBet, actualMaxBet);

            return bet;
        }


    }
}