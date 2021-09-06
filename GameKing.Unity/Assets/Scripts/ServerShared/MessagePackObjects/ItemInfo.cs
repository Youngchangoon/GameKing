using System;
using GameKing.Shared.MessagePackObjects;
using MessagePack;

[MessagePackObject]
public class ItemInfo
{
    [Key(0)] public int x;
    [Key(1)] public int y;

    [Key(2)] public ItemKind ItemKind;

    public ItemInfo(int x, int y, ItemKind itemKind)
    {
        this.x = x;
        this.y = y;
        this.ItemKind = itemKind;
    }

    public ItemType GetItemType() => GetItemType(ItemKind);

    private ItemType GetItemType(ItemKind itemKind)
    {
        switch (itemKind)
        {
            case ItemKind.None:
                break;
            case ItemKind.Shield:
            case ItemKind.MoveUp:
            case ItemKind.Teleport:
            case ItemKind.Energy:
                return ItemType.Defence;
            case ItemKind.SeeThrough:
            case ItemKind.DamageUp:
            case ItemKind.Lock:
            case ItemKind.Double:
            case ItemKind.Multi:
            case ItemKind.SeeThroughAll:
                return ItemType.Attack;
            default:
                throw new ArgumentOutOfRangeException(nameof(itemKind), itemKind, null);
        }
        
        throw new ArgumentOutOfRangeException(nameof(itemKind), itemKind, null); 
    }
}