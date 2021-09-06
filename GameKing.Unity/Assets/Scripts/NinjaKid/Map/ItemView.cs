using GameKing.Shared.MessagePackObjects;
using UnityEngine;

namespace GameKing.Unity.NinjaKid
{
    public class ItemView : MonoBehaviour
    {
        [SerializeField] private float yOffset;
        public ItemKind MyItemKind { get; private set; }
        
        public void Init(Vector3 cellLocalPos, ItemKind itemKind)
        {
            cellLocalPos.y += yOffset;
            
            transform.localPosition = cellLocalPos;
            MyItemKind = itemKind;
        }
        
        
    }
}