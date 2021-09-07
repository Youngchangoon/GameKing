using GameKing.Shared.MessagePackObjects;
using GameKing.Unity.NinjaKid.Messages;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace GameKing.Unity.NinjaKid
{
    public class ItemButton : MonoBehaviour
    {
        [SerializeField] private Text itemText;

        private ItemKind _itemKind;
        private ItemType _itemType;

        public void Init(ItemKind itemKind, ItemType itemType)
        {
            itemText.text = itemKind.ToString();

            _itemKind = itemKind;
            _itemType = itemType;
        }

        public void OnPressedItemButton()
        {
            MessageBroker.Default.Publish(new UseItemEvent
            {
                ItemKind = _itemKind,
                ItemType = _itemType,
                ItemButton = this
            });
        }
    }
}