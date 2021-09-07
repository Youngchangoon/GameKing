using System;
using System.Collections.Generic;
using System.Numerics;
using MessagePack;

namespace GameKing.Shared.MessagePackObjects
{
    [MessagePackObject]
    public class NinjaKidModel
    {
        [Key(0)] public MapModel MapModel { get; set; }
        [Key(1)] public MarkModel[] MarkModels { get; set; }
        [Key(2)] public int CurTurnPlayerIndex { get; set; }
        [Key(4)] public bool IsMoved { get; set; }
        [Key(5)] public bool IsAttacked { get; set; }


        public NinjaKidModel(MapModel map)
        {
            MapModel = map;
            CurTurnPlayerIndex = 0;
            IsMoved = false;
            IsAttacked = false;

            MarkModels = new MarkModel[2]
            {
                new MarkModel() { x = -1, y = -1, hp = 100, damage = 20, items = new List<ItemKind>() },
                new MarkModel() { x = -1, y = -1, hp = 100, damage = 20, items = new List<ItemKind>() },
            };
        }

        /// <summary>
        /// 모든 말이 올려져 있는지 체크
        /// </summary>
        /// <returns></returns>
        public bool IsAllPlacedMark()
        {
            foreach (var mark in MarkModels)
            {
                if (mark.x == -1 || mark.y == -1)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Cell을 선택해 공격한다.
        /// </summary>
        /// <param name="attackerIndex"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public int Attack(int attackerIndex, int x, int y)
        {
            IsAttacked = true;
            
            if (MapModel.IsOutOfRange(x, y))
                return 0;
            
            OpenCell(x, y);

            var damage = MarkModels[attackerIndex].damage;

            for (var i = 0; i < MarkModels.Length; ++i)
            {
                var curModel = MarkModels[i];
                if (curModel.x != x || curModel.y != y)
                    continue;

                curModel.hp -= damage;
                curModel.hp = Math.Max(0, curModel.hp);
            }

            
            return damage;
        }

        public void Move(int playerIndex, int x, int y)
        {
            IsMoved = true;

            if (MapModel.IsOutOfRange(x, y))
                return;

            var markModel = MarkModels[playerIndex];
            markModel.x = x;
            markModel.y = y;
        }

        public int CheckTurnEnd()
        {
            if (IsMoved && IsAttacked)
                return SetNextPlayerIndex();

            return -1;
        }

        public int SetNextPlayerIndex()
        {
            CurTurnPlayerIndex = ++CurTurnPlayerIndex >= 2 ? 0 : CurTurnPlayerIndex;
            IsMoved = false;
            IsAttacked = false;

            return CurTurnPlayerIndex;
        }

        private void OpenCell(int x, int y)
        {
            MapModel.list[y][x].IsOpen = true;
        }

        private MarkModel GetOppositeMarkModel(int playerIndex)
        {
            var nextIndex = playerIndex + 1;
            return MarkModels[nextIndex < MarkModels.Length ? nextIndex : 0];
        }

        private int GetOppositeIndex(int playerIndex)
        {
            var nextIndex = playerIndex + 1;
            return nextIndex < MarkModels.Length ? nextIndex : 0;
        }

        public GameEndType GetGameEndState()
        {
            var player0Dead = false;
            var player1Dead = false;

            if (MarkModels[0].hp <= 0)
                player0Dead = true;

            if (MarkModels[1].hp <= 0)
                player1Dead = true;

            if (player0Dead && player1Dead)
                return GameEndType.Draw;

            if (player0Dead)
                return GameEndType.PlayerWin1;
            if (player1Dead)
                return GameEndType.PlayerWin0;

            return GameEndType.None;
        }

        public ItemInfo[] GenerateRandomItem(int generateItemCount)
        {
            var itemPlacedInfoArr = new ItemInfo[generateItemCount];
            
            for (var i = 0; i < generateItemCount; ++i)
            {
                var items = Enum.GetValues(typeof(ItemKind));
                var newItemType = (ItemKind)items.GetValue(new Random().Next(1, items.Length)); // 0은 None이니 제외
                var mapArray = MapModel.list;
                var nonePosList = new List<Tuple<int, int>>();

                for (var y = 0; y < mapArray.Count; ++y)
                {
                    for (var x = 0; x < mapArray[y].Count; ++x)
                    {
                        if (mapArray[y][x].ItemModel.ItemKind != ItemKind.None)
                            continue;

                        nonePosList.Add(new Tuple<int, int>(x, y));
                    }
                }

                if (nonePosList.Count == 0)
                    return null;

                var (posX, posY) = nonePosList[new Random().Next(0, nonePosList.Count)];
                mapArray[posY][posX].ItemModel.ItemKind = newItemType;

                itemPlacedInfoArr[i] = new ItemInfo(posX, posY, newItemType);
            }

            return itemPlacedInfoArr;
        }

        /// <summary>
        /// 해당 위치의 아이템이 있다면 먹는다.
        /// </summary>
        public ItemInfo CheckAndGetItemOrNull(int playerIndex, int x, int y)
        {
            if (MapModel.IsOutOfRange(x, y))
                return null;
            
            var curCellItemModel = MapModel.list[y][x].ItemModel;
            var cellItemType = curCellItemModel.ItemKind;

            if (cellItemType != ItemKind.None)
            {
                MarkModels[playerIndex].items.Add(cellItemType);
                curCellItemModel.ItemKind = ItemKind.None;
            }

            return cellItemType != ItemKind.None ? new ItemInfo(x, y, cellItemType) : null;
        }

        /// <summary>
        /// 아이템을 사용한다.
        /// </summary>
        /// <param name="playerIndex"></param>
        /// <param name="itemKind"></param>
        public void UseItem(int playerIndex, ItemKind itemKind)
        {
            MarkModels[playerIndex].items.Remove(itemKind);
        }

        public void HealPlayer(int playerIndex, int addHp)
        {
            var curHp = MarkModels[playerIndex].hp;

            MarkModels[playerIndex].hp = Math.Min(curHp + addHp, 100);
        }
    }
}