using System.Collections.Generic;
using MessagePack;

namespace GameKing.Shared.MessagePackObjects
{
    [MessagePackObject]
    public class MapModel
    {
        [Key(0)] public List<List<CellModel>> list { get; set; }
    }
}