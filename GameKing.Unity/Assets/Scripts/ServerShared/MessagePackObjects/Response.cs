using MessagePack;

namespace GameKing.Shared.MessagePackObjects
{
    [MessagePackObject]
    public struct GameStartResponse
    {
        [Key(0)] public string[] PlayerNames { get; set; }
    }
}