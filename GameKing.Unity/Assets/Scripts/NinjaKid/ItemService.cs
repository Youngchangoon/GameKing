using System;
using Cysharp.Threading.Tasks;
using GameKing.Shared.MessagePackObjects;
using GameKing.Unity.NinjaKid.Map;
using GameKing.Unity.NinjaKid.Messages;
using UniRx;
using UnityEngine;
using Zenject;

namespace GameKing.Unity.NinjaKid.Item
{
    public class ItemService : IInitializable
    {
        [Inject] private NinjaKidServerService _serverService;
        [Inject] private MapService _mapService;

        public ActionType CurActionType { get; private set; }

        public void Initialize()
        {
            MessageBroker.Default.Receive<ActionType>().Subscribe(action => { CurActionType = action; });
            MessageBroker.Default.Receive<UseItemEvent>().Subscribe(itemEvent => { UseItem(itemEvent.ItemKind, itemEvent.ItemType).Forget(); });
        }

        private async UniTaskVoid UseItem(ItemKind itemKind, ItemType itemType)
        {
            Debug.Log("CurActionType: " + CurActionType);

            if (CurActionType == ActionType.None)
            {
                if (itemType == ItemType.Attack)
                    MessageBroker.Default.Publish(ActionType.Attack);
                if (itemType == ItemType.Defence)
                    MessageBroker.Default.Publish(ActionType.Move);
            }
            else
            {
                await _serverService.UseItemAsync(itemKind);
                AdaptItem(itemKind);
            }
        }

        private void AdaptItem(ItemKind itemKind)
        {
            switch (itemKind)
            {
                case ItemKind.None:
                    break;
                case ItemKind.Shield:
                    break;
                case ItemKind.MoveUp:
                    break;
                case ItemKind.Teleport:
                    break;
                case ItemKind.Energy:
                    break;
                case ItemKind.SeeThrough:
                    break;
                case ItemKind.DamageUp:
                    break;
                case ItemKind.Lock:
                    break;
                case ItemKind.Double:
                    break;
                case ItemKind.Multi:
                    break;
                case ItemKind.SeeThroughAll:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(itemKind), itemKind, null);
            }
        }
    }
}