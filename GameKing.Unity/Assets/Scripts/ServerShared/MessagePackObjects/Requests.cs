using MessagePack;

namespace GameKing.Shared.MessagePackObjects
{
    /// <summary>
    /// 방 입장 Request
    /// </summary>
    [MessagePackObject]
    public struct JoinRequest
    {
        [Key(0)] public string RoomName { get; set; }
        [Key(1)] public string UserName { get; set; }
    }
}