using System.Collections.Generic;
using MessagePack;

namespace GameKing.Shared.MessagePackObjects
{
    [MessagePackObject]
    public class MapModel
    {
        [Key(0)] public List<List<CellModel>> list { get; set; }
        
        public bool IsOutOfRange(int x, int y)
        {
            if (list.Count == 0)
                return true;
            
            if (x < 0 || y < 0 || x >= list[0].Count || y >= list.Count)
                return true;
            
            return false;
        }
    }
}