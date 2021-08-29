using System.Collections;
using System.Collections.Generic;
using MagicOnion;
using MessagePack;

namespace GameKing.Shared.Services
{
    public interface IChatService : IService<IChatService>
    {
        UnaryResult<Nil> GenerateException(string message);
        UnaryResult<Nil> SendReportAsync(string message);
    }
}