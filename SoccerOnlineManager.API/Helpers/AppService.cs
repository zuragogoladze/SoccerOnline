using Microsoft.AspNetCore.Http;
using SoccerOnlineManager.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SoccerOnlineManager.API.Helpers
{
    public class AppService : IAppService
    {
        private HttpContext _httpContext;

        public AppService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext.HttpContext ?? null;
        }

        public Guid UserId => GetUserIdFromContext();

        public bool IsAdmin => GetIsAdminFromContext();

        private Guid GetUserIdFromContext()
        {
            if (_httpContext == null && _httpContext.User.Identity.IsAuthenticated)
                throw new ApiException(HttpStatusCode.Unauthorized);

            var principal = _httpContext.User;
            if(principal == null)
                throw new ApiException(HttpStatusCode.Unauthorized);

            var sub = principal.Identity?.Name;

            if (sub == null)
                throw new ApiException(HttpStatusCode.Unauthorized);

            return Guid.Parse(sub);
        }

        private bool GetIsAdminFromContext()
        {
            if (_httpContext == null && _httpContext.User.Identity.IsAuthenticated)
                throw new ApiException(HttpStatusCode.Unauthorized);

            var principal = _httpContext.User;
            if (principal == null)
                throw new ApiException(HttpStatusCode.Unauthorized);

            return _httpContext.User.IsInRole("Admin");
        }
    }
}
