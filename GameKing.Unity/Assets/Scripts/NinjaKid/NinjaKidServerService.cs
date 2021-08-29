using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameKing.Shared.Hubs;
using GameKing.Shared.MessagePackObjects;
using GameKing.Shared.Services;
using GameKing.Unity.NinjaKid.Map;
using GameKing.Unity.NinjaKid.Messages;
using Grpc.Core;
using MagicOnion;
using MagicOnion.Client;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace GameKing.Unity.NinjaKid
{
    public class NinjaKidServerService : IInitializable, INinjaKidReceiver, IDisposable
    {
        [Inject] private MapService _mapService;
        [Inject] private MarkService _markService;
        [Inject] private InGameScreen _inGameScreen;

        private CancellationTokenSource _shutdownCancellation = new CancellationTokenSource();
        private ChannelBase _channel;
        private INinjaKidHub _streamingClient;
        private NinjaKidModel _ninjaKidModel;
        private int _myPlayerIndex;

        public int CurTurnPlayerIndex { get; private set; }

        public bool IsMyTurn() => _myPlayerIndex == CurTurnPlayerIndex;

        public void Initialize()
        {
            CurTurnPlayerIndex = -1;

            InitializeClientAsync().Forget();
        }

        void OnDestroy()
        {
            // Clean up Hub and channel
            _shutdownCancellation.Cancel();
        }

        public void Dispose()
        {
            _shutdownCancellation.Cancel();

            _shutdownCancellation?.Dispose();
            _channel?.ShutdownAsync();
        }


        public async UniTaskVoid InitializeClientAsync()
        {
            _channel = GrpcChannelx.ForAddress("http://localhost:5000");

            while (!_shutdownCancellation.IsCancellationRequested)
            {
                try
                {
                    Debug.Log($"Connecting to the server..");
                    _streamingClient = await StreamingHubClient.ConnectAsync<INinjaKidHub, INinjaKidReceiver>(_channel, this, cancellationToken: _shutdownCancellation.Token);
                    RegisterDisconnectEvent(_streamingClient);

                    JoinToRoom().Forget();
                    break;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }

                Debug.Log("Failed to connect to the server. retry after 5seconds..");
                await Task.Delay(5 * 1000);
            }
        }

        private async void RegisterDisconnectEvent(INinjaKidHub streamingClient)
        {
            try
            {
                await streamingClient.WaitForDisconnect();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            finally
            {
                Debug.Log("Disconnected from the server.");
            }
        }

        // ------- Send To Server ------
        public async UniTaskVoid JoinToRoom()
        {
            var request = new JoinRequest { RoomName = "TestRoom", UserName = $"Player_{Random.Range(0, 1000)}" };

            _myPlayerIndex = await _streamingClient.JoinAsync(request);
            Debug.Log("myPlayerIndex: " + _myPlayerIndex);
        }

        public async UniTaskVoid SendPlacedMark(int x, int y)
        {
            await _streamingClient.PlaceMarkAsync(_myPlayerIndex, x, y);
        }

        public async UniTaskVoid AttackPosAsync(int x, int y)
        {
            _inGameScreen.SetEnableButton(ActionType.Attack, false);
            await _streamingClient.AttackPosAsync(_myPlayerIndex, x, y);
        }

        public async UniTaskVoid MovePosAsync(int x, int y)
        {
            _inGameScreen.SetEnableButton(ActionType.Move, false);
            await _streamingClient.MovePosAsync(_myPlayerIndex, x, y);
        }


        // ------- Received Server -----

        public void OnJoin(string userName)
        {
            Debug.Log("ON join...!: " + userName);
        }

        public void OnLeave(string leaveUerName)
        {

        }

        public void OnGameStart(NinjaKidModel ninjaKidModel)
        {
            _ninjaKidModel = ninjaKidModel;
            _mapService.GenerateMapObjects(ninjaKidModel.MapModel);
        }

        public void OnGameState(GameState gameState, GameEndType gameEndType)
        {
            Debug.Log("GAME STATE!!: " + gameState);
            switch (gameState)
            {
                case GameState.SelectMarkPos:
                    _mapService.UpdateMapState(MapState.SelectPos);
                    break;
                case GameState.GameEnd:
                    _inGameScreen.ShowResult(gameEndType, _myPlayerIndex == (int)gameEndType - 1);
                    break;
            }
        }

        public void OnPlacedMark(MarkModel[] markModels)
        {
            for (var playerIndex = 0; playerIndex < markModels.Length; playerIndex++)
            {
                var markModel = markModels[playerIndex];
                Debug.Log("MARK POS: " + markModel.x + ", " + markModel.y);
                _markService.SetMarkPos(markModel, playerIndex);
            }
        }

        public void OnStartTurn(int curTurnPlayerIndex)
        {
            Debug.Log("CUR TURN PlayerINDEX: " + curTurnPlayerIndex + ", myIDX: " + _myPlayerIndex);
            CurTurnPlayerIndex = curTurnPlayerIndex;

            if (_myPlayerIndex == curTurnPlayerIndex)
            {
                // My Turn!!
                _inGameScreen.SetEnableAllButton(true);
            }
            else
            {
                // Opposite turn!
            }
        }

        public void OnAttackedCell(int damage, int x, int y)
        {
            _mapService.SetOpen(x, y, true);
            _markService.DamagePos(damage, x, y);
        }

        public void OnMovedCell(MarkModel[] markModels)
        {
            for (var i = 0; i < markModels.Length; ++i)
                _markService.SetMarkPos(markModels[i], i);
        }
    }
}