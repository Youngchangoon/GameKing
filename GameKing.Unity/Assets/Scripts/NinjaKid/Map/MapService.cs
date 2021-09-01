using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameKing.Shared.MessagePackObjects;
using GameKing.Unity.NinjaKid.Messages;
using UniRx;
using UnityEngine;
using Zenject;

namespace GameKing.Unity.NinjaKid.Map
{
    public class MapService : IInitializable
    {
        [Inject] private NinjaKidServerService _ninjaKidServerService;
        [Inject] private MarkService _markService;

        [Inject] private MapView _mapView;

        private MapModel _mapModel;
        private MapState _curMapState;

        public void Initialize()
        {
            MessageBroker.Default.Receive<SelectCellEvent>().Subscribe(e =>
            {
                switch (_curMapState)
                {
                    case MapState.SelectPos:
                        _ninjaKidServerService.SendPlacedMark(e.Pos.x, e.Pos.y).Forget();
                        break;
                    case MapState.AttackPos:
                        _ninjaKidServerService.AttackPosAsync(e.Pos.x, e.Pos.y).Forget();
                        break;
                    case MapState.MovePos:
                        _ninjaKidServerService.MovePosAsync(e.Pos.x, e.Pos.y).Forget();
                        break;
                }

                UpdateMapState(MapState.None);
            });

            MessageBroker.Default.Receive<ActionType>().Subscribe(actionType =>
            {
                switch (actionType)
                {
                    case ActionType.None:
                        UpdateMapState(MapState.None);
                        break;
                    case ActionType.Attack:
                        UpdateMapState(MapState.AttackPos);
                        break;
                    case ActionType.Move:
                        UpdateMapState(MapState.MovePos);
                        break;
                }
            });
        }

        public void GenerateMapObjects(MapModel mapModel)
        {
            _mapModel = mapModel;
            _mapView.CreateMapView(mapModel);
        }

        public void SetOpen(int x, int y, bool isOpen)
        {
            _mapView.SetOpen(x, y, isOpen);
        }

        public void SetCelAlphaAll(float alpha)
        {
            _mapView.SetCellAlphaAll(alpha);
        }

        public void SetCellAlpha(int x, int y, float alpha)
        {
            _mapView.SetCellAlpha(x, y, alpha);
        }

        public void UpdateMapState(MapState mapState)
        {
            _curMapState = mapState;

            switch (_curMapState)
            {
                case MapState.None:
                    _mapView.SetAllCanSelect(false);
                    break;
                case MapState.SelectPos:
                case MapState.AttackPos:
                    _mapView.SetAllCanSelect(true);
                    break;
                case MapState.MovePos:
                    _mapView.SetCanSelectNearByPos(_markService.GetMarkPos(_ninjaKidServerService.CurTurnPlayerIndex));
                    break;
            }
        }

        public void CheckAllGetItem()
        {
            // _markService
            // _mapView.CheckAllGetItem()
        }

        public Vector2 GetMapPosition(int x, int y)
        {
            return _mapView.GetPosition(x, y);
        }

        public void AddItem(ItemPlacedInfo itemsPlacedInfo)
        {
            _mapView.AddItemInMap(itemsPlacedInfo);
        }
    }
}