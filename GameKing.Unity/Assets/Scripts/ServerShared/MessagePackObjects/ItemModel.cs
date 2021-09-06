using MessagePack;

namespace GameKing.Shared.MessagePackObjects
{
    [MessagePackObject]
    public class ItemModel
    {
        [Key(0)] public ItemKind ItemKind { get; set; }
    }
}