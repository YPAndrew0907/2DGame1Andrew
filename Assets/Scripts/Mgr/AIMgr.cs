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

        /// <summary>
        /// ai 输了返回 1，赢了返回 -1，平局返回 0
        /// </summary>
        /// <param name="aiNum">AI分数</param>
        /// <param name="playerNum">玩家分数</param>
        /// <returns></returns>
        public static int AIIsLoss(int aiNum, int playerNum)
        {
            if (aiNum < 0) return 1;
            if (playerNum < 0) return -1;
            
            // 0视为爆牌/特殊情况，优先判负
            if (aiNum == 0 && playerNum != 0) return 1;  // AI爆/弃
            if (playerNum == 0 && aiNum != 0) return -1; // 玩家爆/弃
            if (aiNum == 0 && playerNum == 0) return 0;  // 都弃/爆

            bool aiBust     = aiNum > 21;
            bool playerBust = playerNum > 21;

            // 双方都未爆
            if (!aiBust && !playerBust)
            {
                if (aiNum > playerNum) return -1; // AI赢
                if (aiNum < playerNum) return 1;  // AI输
                return 0;                         // 平局
            }
            // AI爆，玩家未爆
            if (aiBust && !playerBust) return 1;        // AI输
            // 玩家爆，AI未爆
            if (!aiBust && playerBust) return -1;       // AI赢

            // 双方都爆，分数小的赢
            if (aiNum < playerNum) return -1; // AI赢（爆得少）
            if (aiNum > playerNum) return 1;  // AI输
            return 0;                         // 平局
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

        /// <summary>
        /// 随机选择偷还是插入
        /// </summary>
        /// <param name="count">当前收藏的牌的数量</param>
        /// <returns>false 是偷，true 是插入</returns>
        public static bool RandomStealOrInsert(int count)
        {
            if (count == 0)
                return false;
            return Random.Range(0, 1.0f) > 0.5;
        }


        public static (List<CardObj> ToCollect, List<CardObj> ToTotal)
            SelectCards(IReadOnlyList<CardObj> totalCards, List<CardObj> collectCard, int moveCount)
        {
            var toCollect = new List<CardObj>();
            var toTotal   = new List<CardObj>();
            // 合并所有牌用于评分（确保评分时能看到全部牌面）
            var allCards  = totalCards.Concat(collectCard).ToList();
            var allPoints = allCards.Select(x => GetCardPoint(x.Value)).ToList();

            // 作弊贡献评分
            int ScoreCard(CardObj card)
            {
                int myPoint = GetCardPoint(card.Value);
                int score   = 0;
                for (int i = 0; i < allPoints.Count; i++)
                {
                    for (int j = 0; j < allPoints.Count; j++)
                    {
                        if (i == j)
                            continue;
                        int origin = allPoints[i] + allPoints[j];
                        if (origin >= 21)
                            continue; // 已爆，不考虑
                        int withCheat = Math.Max(origin, origin + myPoint);
                        // 拉低爆牌风险 or 提高低分
                        if (origin < 17 && withCheat >= 17 && withCheat <= 21)
                            score++;
                        else if (origin >= 19 && withCheat <= 21)
                            score++;
                    }
                }

                return score;
            }

            // 评分
            var totalScores = totalCards
                              .Select(card => (card, score: ScoreCard(card)))
                              .ToList();
            var collectScores = collectCard
                                .Select(card => (card, score: ScoreCard(card)))
                                .ToList();

            // 排序
            totalScores   = totalScores.OrderByDescending(c => c.score).ToList();
            collectScores = collectScores.OrderBy(c => c.score).ToList();

            int move = 0, iTotal = 0, iCollect = 0;
            // 用moveCount次机会，每次把提升最大的牌换进collect，换出最差的
            while (move < moveCount && iTotal < totalScores.Count)
            {
                // 如果collect满了且有比collect最差还高分的牌，执行交换
                if (iCollect < collectScores.Count && totalScores[iTotal].score > collectScores[iCollect].score)
                {
                    toCollect.Add(totalScores[iTotal].card);
                    toTotal.Add(collectScores[iCollect].card);
                    iCollect++;
                    iTotal++;
                }
                // 如果collect没满，只加牌
                else if (collectCard.Count + toCollect.Count < totalCards.Count)
                {
                    toCollect.Add(totalScores[iTotal].card);
                    iTotal++;
                }
                else
                    break;

                move++;
            }

            return (toCollect, toTotal);
        }

        // 牌点数换算
        private static int GetCardPoint(CardValue val)
        {
            if (val == CardValue.A)
                return 1;
            if (val >= CardValue.Two && val <= CardValue.Ten)
                return (int)val + 1;
            if (val >= CardValue.J && val <= CardValue.K)
                return 10;
            return 0;
        }
    }
}