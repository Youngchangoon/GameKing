using MessagePack;

namespace GameKing.Shared.MessagePackObjects
{
    [MessagePackObject]
    public class CellModel
    {
        [Key(0)] public bool IsOpen { get; set; }
        
        [Key(1)] public ItemModel ItemModel { get; set; }
    }
}