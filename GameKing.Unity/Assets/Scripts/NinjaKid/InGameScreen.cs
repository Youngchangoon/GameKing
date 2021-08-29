using GameKing.Unity.NinjaKid.Messages;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameKing.Unity.NinjaKid
{
    public enum ActionType
    {
        None,
        Move,
        Attack,
    }

    public class InGameScreen : MonoBehaviour
    {
        [Inject] private NinjaKidServerService _serverService;

        [SerializeField] private Button attackButton;
        [SerializeField] private Button moveButton;

        public void SetEnableAllButton(bool enable)
        {
            SetEnableButton(ActionType.Attack, enable);
            SetEnableButton(ActionType.Move, enable);
        }

        public void SetEnableButton(ActionType actionType, bool enable)
        {
            switch (actionType)
            {
                case ActionType.Move:
                    moveButton.interactable = enable;
                    break;
                case ActionType.Attack:
                    attackButton.interactable = enable;
                    break;
            }
        }

        public void OnPressedAttackButton()
        {
            if (_serverService.IsMyTurn() == false)
                return;

            MessageBroker.Default.Publish(ActionType.Attack);
        }

        public void OnPressedMoveButton()
        {
            if (_serverService.IsMyTurn() == false)
                return;

            MessageBroker.Default.Publish(ActionType.Move);
        }
    }
}