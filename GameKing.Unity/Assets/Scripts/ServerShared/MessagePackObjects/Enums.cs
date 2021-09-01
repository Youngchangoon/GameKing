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

    public enum ItemType
    {
        None,
        Shield,     // (방어) 자신을 방어하는 실드 생성
        MoveUp,     // (방어) 이동을 한번 더함
        Teleport,   // (방어) 어디로든지 이동할 수 있다.
        Energy,     // (방어) 자신의 에너지를 채운다.
        SeeThrough, // (공격) 안보이는 곳을 한곳 볼 수 있음
        DamageUp,   // (공격) 데미지 1.5배
        Lock,       // (공격) 상대방을 한턴 묶는다.
        Double,     // (공격) 한 곳을 두번 공격한다.
        Multi,      // (공격) 2곳을 동시에 공격한다.
        SeeThroughAll, // (공격) 전체 셀을 투시한다.
    }
}