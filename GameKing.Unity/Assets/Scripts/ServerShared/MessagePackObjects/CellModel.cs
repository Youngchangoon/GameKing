using MessagePack;

namespace GameKing.Shared.MessagePackObjects
{
    [MessagePackObject]
    public class CellModel
    {
        [Key(0)] public bool IsOpen { get; set; }
    }
}