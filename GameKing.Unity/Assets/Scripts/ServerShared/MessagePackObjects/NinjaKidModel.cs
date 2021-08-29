using System;
using System.Collections.Generic;
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
                new MarkModel() { x = -1, y = -1, hp = 100, damage = 100},
                new MarkModel() { x = -1, y = -1, hp = 100, damage = 100},
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

            IsAttacked = true;
            return damage;
        }
        
        public void Move(int playerIndex, int x, int y)
        {
            var markModel = MarkModels[playerIndex];

            markModel.x = x;
            markModel.y = y;

            IsMoved = true;
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
    }
}