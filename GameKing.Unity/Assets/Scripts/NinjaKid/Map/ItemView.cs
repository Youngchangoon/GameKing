using GameKing.Shared.MessagePackObjects;
using UnityEngine;

namespace GameKing.Unity.NinjaKid
{
    public class ItemView : MonoBehaviour
    {
        [SerializeField] private float yOffset;
        public ItemType MyItemType { get; private set; }
        
        public void Init(Vector3 cellLocalPos, ItemType itemType)
        {
            cellLocalPos.y += yOffset;
            
            transform.localPosition = cellLocalPos;
            MyItemType = itemType;
        }
    }
}