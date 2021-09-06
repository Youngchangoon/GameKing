using System.Threading.Tasks;
using GameKing.Shared.MessagePackObjects;
using MagicOnion;

namespace GameKing.Shared.Hubs
{
    // Client -> Serer 
    public interface INinjaKidHub : IStreamingHub<INinjaKidHub, INinjaKidReceiver>
    {
        /// <summary>
        /// 유저 입장
        /// </summary>
        /// <returns></returns>
        Task<int> JoinAsync(JoinRequest request);

        /// <summary>
        /// 유저 퇴장
        /// </summary>
        /// <returns></returns>
        Task LeaveAsync();

        /// <summary>
        /// 플레이어어 위치를 보낸다.
        /// </summary>
        /// <param name="playerIndex">플레이어 인덱스</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        Task PlaceMarkAsync(int playerIndex, int x, int y);

        /// <summary>
        /// 공격할 위치를 보낸다.
        /// </summary>
        /// <param name="playerIndex">플레이어 인덱스</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        Task AttackPosAsync(int playerIndex, int x, int y);

        /// <summary>
        /// 움직일 위치를 보낸다.
        /// </summary>
        /// <param name="playerIndex"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        Task MovePosAsync(int playerIndex, int x, int y);

        /// <summary>
        /// 아이템을 사용한다.
        /// </summary>
        /// <param name="playerIndex"></param>
        /// <param name="itemKind"></param>
        /// <returns></returns>
        Task UseItemAsync(ItemKind itemKind);
    }
}