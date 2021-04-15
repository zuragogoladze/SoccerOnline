using System;

namespace SoccerOnlineManager.API.Helpers
{
    public interface IAppService
    {
        Guid UserId { get; }
        bool IsAdmin { get; }
    }
}
