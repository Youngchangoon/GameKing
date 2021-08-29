using MessagePack;

namespace GameKing.Shared.MessagePackObjects
{
    [MessagePackObject]
    public class MarkModel
    {
        [Key(0)] public int x;
        [Key(1)] public int y;

        [Key(2)] public int hp;
        [Key(3)] public int damage;
    }
}