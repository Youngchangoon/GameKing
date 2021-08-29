using DG.Tweening;
using GameKing.Unity.NinjaKid.Messages;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace GameKing.Unity.NinjaKid.Map
{
    public class CellView : MonoBehaviour
    {
        [SerializeField] private Image canSelectMark;
        [SerializeField] private Button cellButton;
        [SerializeField] private RectTransform[] doorTrsArr;

        private Vector2Int _cellPos;

        public void Init(Vector2Int cellPos)
        {
            _cellPos = cellPos;
            
            UpdateCellCanSelect(false);
        }

        public void UpdateCellCanSelect(bool isCanSelect)
        {
            canSelectMark.enabled = isCanSelect;
            cellButton.interactable = isCanSelect;
        }

        public void SetOpen(bool isOpen)
        {
            for (var i = 0; i < 2; ++i)
            {
                doorTrsArr[i].DOKill();
                doorTrsArr[i].DOScaleX(isOpen ? 0f : 1f, 0.2f);
            }
        }
        
        public void OnPressedCellButton()
        {
            MessageBroker.Default.Publish(new SelectCellEvent{Pos = _cellPos});
        }
    }
}