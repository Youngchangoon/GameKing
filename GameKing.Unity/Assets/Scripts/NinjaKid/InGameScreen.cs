using GameKing.Shared.MessagePackObjects;
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
        [SerializeField] private Text attackButtonText;
        [SerializeField] private Text moveButtonText;
        [SerializeField] private GameResultPopup resultPopup;

        private ActionType _curActionType;

        public void Awake()
        {
            MessageBroker.Default.Receive<ActionType>().Subscribe(actionType => _curActionType = actionType);
        }

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
                    moveButtonText.text = "Move";
                    break;
                case ActionType.Attack:
                    attackButton.interactable = enable;
                    attackButtonText.text = "Attack";
                    break;
            }
        }

        public void ShowResult(GameEndType gameEndType, bool isPlayerWin)
        {
            resultPopup.Init(gameEndType, isPlayerWin);
        }

        public void OnPressedAttackButton()
        {
            if (_serverService.IsMyTurn() == false)
                return;
            
            switch (_curActionType)
            {
                case ActionType.None:
                    MessageBroker.Default.Publish(ActionType.Attack);
                    attackButtonText.text = "Attack\nSkip";
                    break;
                case ActionType.Attack:
                    MessageBroker.Default.Publish(new SelectCellEvent{Pos = new Vector2Int(-1, -1)});
                    break;
                default:
                    return;
            }
        }

        public void OnPressedMoveButton()
        {
            if (_serverService.IsMyTurn() == false)
                return;

            switch (_curActionType)
            {
                case ActionType.None:
                    MessageBroker.Default.Publish(ActionType.Move);
                    moveButtonText.text = "Move\nSkip";
                    break;
                case ActionType.Move:
                    MessageBroker.Default.Publish(new SelectCellEvent{Pos = new Vector2Int(-1, -1)});
                    break;
                default:
                    return;
            }
        }
    }
}