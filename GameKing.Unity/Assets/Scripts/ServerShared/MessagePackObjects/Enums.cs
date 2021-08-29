namespace GameKing.Shared.MessagePackObjects
{
    public enum GameState
    {
        SelectMarkPos,  // 위치 선정 상태
        GamePlay,       // 게임 진행
        GameEnd,        // 게임 끝
    }

    public enum GameEndType
    {
        None,
        PlayerWin0,     // 플레이어 0 win
        PlayerWin1,     // 플레이어 1 win
        Draw,           // 무승부
    }
}