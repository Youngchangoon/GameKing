using GameKing.Shared.MessagePackObjects;
using UnityEngine;
using UnityEngine.UI;

namespace GameKing.Unity.NinjaKid
{
    public class GameResultPopup : MonoBehaviour
    {
        [SerializeField] private Text resultText;
        
        public void Init(GameEndType gameEndType, bool isPlayerWin)
        {
            gameObject.SetActive(true);

            if (isPlayerWin)
                resultText.text = "You Win!!";
            else
            {
                if (gameEndType == GameEndType.Draw)
                    resultText.text = "DRAW!!";
                else
                    resultText.text = "You Lose..";
            }
        }
    }
}