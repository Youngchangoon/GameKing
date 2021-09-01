using MessagePack;

namespace GameKing.Shared.MessagePackObjects
{
    [MessagePackObject]
    public class ItemModel
    {
        [Key(0)] public ItemType ItemType { get; set; }
    }
}