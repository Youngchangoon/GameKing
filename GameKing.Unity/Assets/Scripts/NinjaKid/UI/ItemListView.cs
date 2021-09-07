using System.Collections.Generic;
using GameKing.Shared.MessagePackObjects;
using GameKing.Unity.Utils;
using UnityEngine;

namespace GameKing.Unity.NinjaKid
{
    public class ItemListView : MonoBehaviour
    {
        [SerializeField] private ItemButton buttonPrefab;
        [SerializeField] private Transform buttonPoolRoot;

        private ObjectPool<ItemButton> _itemButtonPool;
        private Dictionary<ItemType, Transform> _itemButtonRootDic;

        public void PreInit()
        {
            _itemButtonPool = new ObjectPool<ItemButton>(buttonPrefab, "ItemButton", buttonPoolRoot, 10, 5);
            _itemButtonRootDic = new Dictionary<ItemType, Transform>
            {
                { ItemType.Attack, transform.Find("AttackItems/AttackButtonRoot") },
                { ItemType.Defence, transform.Find("DefenceItems/DefenceButtonRoot") },
            };
        }

        public void AddItem(ItemKind itemKind, ItemType itemType)
        {
            var newItemButton = _itemButtonPool.Pop();
            var newItemButtonRoot = _itemButtonRootDic[itemType];
            
            newItemButton.transform.SetParent(newItemButtonRoot);
            newItemButton.Init(itemKind, itemType);
        }

        public void RemoveItem(ItemButton itemButton)
        {
            _itemButtonPool.Push(itemButton);
        }
    }
}