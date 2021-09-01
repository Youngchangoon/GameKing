using GameKing.Shared.MessagePackObjects;
using MessagePack;

[MessagePackObject]
public class ItemPlacedInfo
{
    [Key(0)] public int x;
    [Key(1)] public int y;

    [Key(2)] public ItemType itemType;

    public ItemPlacedInfo(int x, int y, ItemType itemType)
    {
        this.x = x;
        this.y = y;
        this.itemType = itemType;
    }
}