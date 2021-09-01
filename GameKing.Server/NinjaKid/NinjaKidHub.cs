using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudStructures.Structures;
using GameKing.Shared.Hubs;
using GameKing.Shared.MessagePackObjects;
using MagicOnion.Server.Hubs;
using MagicOnion.Server.Redis;
using StackExchange.Redis;

namespace GameKing.Server
{
    [GroupConfiguration(typeof(RedisGroupRepositoryFactory))]
    public class NinjaKidHub : StreamingHubBase<INinjaKidHub, INinjaKidReceiver>, INinjaKidHub
    {
        private IGroup _room;
        private string _userName;
        private int _playerIndex;
        private RedisString<NinjaKidModel> _gameModelRedis;

        /// <summary>
        /// 유저가 입장할때 들어오는 함수
        /// </summary>
        /// <param name="request"></param>
        public async Task<int> JoinAsync(JoinRequest request)
        {
            //Test
            Console.WriteLine("REQ~~: " + request.RoomName + ", name: " + request.UserName);

            // 유저 추가
            _room = await Group.AddAsync(request.RoomName);
            _userName = request.UserName;
            _playerIndex = await _room.GetMemberCountAsync() - 1;

            // Redis
            var key = _room.GroupName;
            _gameModelRedis = new RedisString<NinjaKidModel>(RedisServer.Connection, new RedisKey(key), TimeSpan.FromDays(1));

            Broadcast(_room).OnJoin(request.UserName);
            var memberCount = await _room.GetMemberCountAsync();
            Console.WriteLine("memberCount: " + memberCount);

            // 멤버수 2명이면 방 게임 시작
            if (memberCount >= 2)
            {
                await _gameModelRedis.SetAsync(new NinjaKidModel(NinjaKidGameLogic.GenerateMapModel(5)));
                var gameModel = await _gameModelRedis.GetAsync();

                Broadcast(_room).OnGameStart(gameModel.Value);
                Broadcast(_room).OnGameState(GameState.SelectMarkPos);
            }

            return _playerIndex;
        }


        /// <summary>
        /// 유저가 퇴장할때 들어오는 함수
        /// </summary>
        public async Task LeaveAsync()
        {
            Console.WriteLine("LEAVE ASYNC~");
            await _room.RemoveAsync(Context);
            Broadcast(_room).OnLeave(_userName);
        }

        /// <summary>
        /// 유저가 말을 놓는다.
        /// </summary>
        /// <param name="x">x 위치</param>
        /// <param name="y">y 위치</param>
        public async Task PlaceMarkAsync(int playerIndex, int x, int y)
        {
            Console.WriteLine($"PlayerIndex: {playerIndex}, x: {x}, y: {y}");
            var gameModel = await _gameModelRedis.GetAsync();
            var curMarkModel = gameModel.Value.MarkModels[playerIndex];

            curMarkModel.x = x;
            curMarkModel.y = y;

            await _gameModelRedis.SetAsync(gameModel.Value);

            if (gameModel.Value.IsAllPlacedMark())
            {
                Broadcast(_room).OnPlacedMark(gameModel.Value.MarkModels);
                Broadcast(_room).OnStartTurn(gameModel.Value.CurTurnPlayerIndex);
            }
        }

        public async Task AttackPosAsync(int playerIndex, int x, int y)
        {
            Console.WriteLine($"Attacker Index: {playerIndex}, x: {x}, y: {y}");

            var gameModelResult = await _gameModelRedis.GetAsync();
            var gameModel = gameModelResult.Value;
            var damage = gameModel.Attack(playerIndex, x, y);

            var nextTurnIndex = gameModel.CheckTurnEnd();
            var isNextTurn = nextTurnIndex != -1;
            var gameEndType = gameModel.GetGameEndState();
            ItemPlacedInfo[] itemPlacedInfoArr = null;

            if (isNextTurn && nextTurnIndex == 0)
                itemPlacedInfoArr = gameModel.GenerateRandomItem(2);

            await _gameModelRedis.SetAsync(gameModel);

            Broadcast(_room).OnAttackedCell(damage, x, y);

            if (gameEndType != GameEndType.None)
                Broadcast(_room).OnGameState(GameState.GameEnd, gameEndType);
            else if (isNextTurn)
            {
                if (itemPlacedInfoArr != null)
                    Broadcast(_room).OnPlacedItem(itemPlacedInfoArr);

                Broadcast(_room).OnStartTurn(nextTurnIndex);
            }
        }

        public async Task MovePosAsync(int playerIndex, int x, int y)
        {
            var gameModelResult = await _gameModelRedis.GetAsync();
            var gameModel = gameModelResult.Value;
            gameModel.Move(playerIndex, x, y);

            var nextTurnIndex = gameModel.CheckTurnEnd();
            var isNextTurn = nextTurnIndex != -1;
            ItemPlacedInfo[] itemPlacedInfoArr = null;

            if (isNextTurn && nextTurnIndex == 0)
                itemPlacedInfoArr = gameModel.GenerateRandomItem(2);

            await _gameModelRedis.SetAsync(gameModel);

            Broadcast(_room).OnMovedCell(gameModel.MarkModels);

            if (isNextTurn)
            {
                if (itemPlacedInfoArr != null)
                    Broadcast(_room).OnPlacedItem(itemPlacedInfoArr);

                Broadcast(_room).OnStartTurn(nextTurnIndex);
            }
        }

        public async Task GetItemAsync(int playerIndex, ItemType itemType, int x, int y)
        {
            var gameModelResult = await _gameModelRedis.GetAsync();
            var gameModel = gameModelResult.Value;
            var willGetMark = gameModel.MarkModels[playerIndex];

            willGetMark.items.Add(itemType);

            await _gameModelRedis.SetAsync(gameModel);

            Broadcast(_room).OnGetItem(playerIndex, itemType, x, y);
        }
    }
}