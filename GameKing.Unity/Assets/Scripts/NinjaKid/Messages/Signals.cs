using GameKing.Shared.MessagePackObjects;
using UnityEngine;

namespace GameKing.Unity.NinjaKid.Messages
{
    public class CreatedMapSignal
    {
        public MapModel MapModel { get; set; }
    }

    public class SelectCellEvent
    {
        public Vector2Int Pos { get; set; }
    }

    public class SelectMoveEvent
    {
        public Vector2Int MovePos { get; set; }
    }

    public class SelectAttackEvent
    {
        public Vector2Int AttackPos { get; set; }
    }

    public class SelectActionTypeEvent
    {
        public ActionType ActionType { get; set; }
    }

    public class UseItemEvent
    {
        public ItemKind ItemKind { get; set; }
        public ItemType ItemType { get; set; }
        
        public ItemButton ItemButton { get; set; }
    }
}