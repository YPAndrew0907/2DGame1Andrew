using System.Collections.Generic;
using UI;

namespace Obj
{
    public class OperationData
    {
        public bool          IsAI;
        public List<CardObj> SelectCards;
    }

    public class ReplaceCardData
    {
        public bool          IsAI;
        public List<CardObj> SkillCard;
        public List<CardObj> TargetList;
    }

    public class InsertCardData
    {
        public bool          IsAI;
        public List<CardObj> ToTotalList;
        public List<CardObj> ToCollectList;
    }

    public class InsertMoveData
    {
        public CardObj  TargetCard;
        public CardZone FromZone;
        public CardZone ToZone;
        public int      FromIdx { get; set; }
        public int      ToIdx   { get; set; }
    }
}