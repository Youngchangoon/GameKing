using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GameKing.Unity.NinjaKid.Map
{
    public class MarkView : MonoBehaviour
    {
        [SerializeField] private Image markImage;
        [SerializeField] private Image hpGauge;

        public void Init(int playerIndex, Transform markRoot)
        {
            SetPlayerColor(playerIndex);
            transform.SetParent(markRoot);
            SetHp(100, 100);
        }

        public void SetPlayerColor(int playerIndex)
        {
            markImage.color = playerIndex == 0 ? Color.red : Color.blue;
        }

        public void SetPosition(Vector2 mapPosition)
        {
            transform.position = mapPosition;
        }

        public void SetHp(int curHp, int maxHp)
        {
            hpGauge.DOKill();
            hpGauge.DOFillAmount((float)curHp / (float)maxHp, 0.2f);
        }
    }
}