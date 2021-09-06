using System.Collections.Generic;
using GameKing.Shared.MessagePackObjects;

namespace GameKing.Server
{
    public static class NinjaKidGameLogic
    {
        public static MapModel GenerateMapModel(int size)
        {
            var mapModel = new MapModel();

            mapModel.list = new List<List<CellModel>>();
            for (var y = 0; y < size; ++y)
            {
                var addCellModelList = new List<CellModel>();
                for (var x = 0; x < size; ++x)
                    addCellModelList.Add(new CellModel
                    {
                        IsOpen = false,
                        ItemModel = new ItemModel { ItemKind = ItemKind.None, }
                    });

                mapModel.list.Add(addCellModelList);
            }

            return mapModel;
        }
    }
}