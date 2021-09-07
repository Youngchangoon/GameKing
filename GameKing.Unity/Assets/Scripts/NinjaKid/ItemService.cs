using System;
using Cysharp.Threading.Tasks;
using GameKing.Shared.MessagePackObjects;
using GameKing.Unity.NinjaKid.Messages;
using UniRx;
using Zenject;

namespace GameKing.Unity.NinjaKid.Item
{
    public class ItemService : IInitializable
    {
        [Inject] private NinjaKidServerService _serverService;
        [Inject] private ItemListView _itemListView;

        public ActionType CurActionType { get; private set; }

        public void Initialize()
        {
            MessageBroker.Default.Receive<ActionType>().Subscribe(action => { CurActionType = action; });
            MessageBroker.Default.Receive<UseItemEvent>().Subscribe(itemEvent => { UseItem(itemEvent).Forget(); });
        }

        private async UniTaskVoid UseItem(UseItemEvent itemEvent)
        {
            // TODO: 만약 이미 공격을 한 상태라면 공격아이템을 사용하지 못하게, (방어아이템도 마찬가지)
            if (CurActionType == ActionType.None)
            {
                if (itemEvent.ItemType == ItemType.Attack)
                    MessageBroker.Default.Publish(ActionType.Attack);
                if (itemEvent.ItemType == ItemType.Defence)
                    MessageBroker.Default.Publish(ActionType.Move);
            }

            await _serverService.UseItemAsync(itemEvent.ItemKind);
            _itemListView.RemoveItem(itemEvent.ItemButton);
            await AdaptItem(itemEvent.ItemKind);
        }

        private async UniTask AdaptItem(ItemKind itemKind)
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
                    await _serverService.PlayerHeal(30);
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