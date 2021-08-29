using System.Threading.Tasks;
using GameKing.Shared.MessagePackObjects;

namespace GameKing.Shared.Hubs
{
    // Server -> Client
    public interface INinjaKidReceiver
    {
        /// <summary>
        /// 게임방에 들어왔을때
        /// </summary>
        void OnJoin(string userName);

        void OnLeave(string leaveUerName);
        
        /// <summary>
        /// 게임 시작
        /// </summary>
        void OnGameStart(NinjaKidModel ninjaKidModel);

        /// <summary>
        /// 게임 상태 Updated
        /// </summary>
        void OnGameState(GameState gameState);
        
        /// <summary>
        /// 모든 말이 놓여졌을때
        /// </summary>
        /// <param name="markModels">말 모델들</param>
        void OnPlacedMark(MarkModel[] markModels);

        /// <summary>
        /// 유저의 턴 시작 알림
        /// </summary>
        /// <param name="curTurnPlayerIndex">턴 시작하는 유저 인덱스</param>
        void OnStartTurn(int curTurnPlayerIndex);

        void OnAttackedCell(int damage, int x, int y);
        
        void OnMovedCell(MarkModel[] markModels);
    }
}