using Baz.RequestManager;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace Baz.KisiServisApi
{
    /// <summary>
    /// Http requestlerinin headrını dolduran class
    /// </summary>
    public class RequestManagerHeaderHelperForHttp
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Http requestlerinin headrını dolduran classın yapıcı methodu.
        /// </summary>
        /// <param name="serviceProvider"></param>
        public RequestManagerHeaderHelperForHttp(IServiceProvider serviceProvider)
        {
            _httpContextAccessor = (IHttpContextAccessor)serviceProvider.GetService(typeof(IHttpContextAccessor));
        }

        /// <summary>
        /// Varsayılan Header atama methodu.
        /// </summary>
        /// <returns></returns>
        public RequestHelperHeader SetDefaultHeader()
        {
            var headers = new RequestHelperHeader();
            if (_httpContextAccessor.HttpContext.Request.Headers["sessionid"].Any())
            {
                var sessionId = _httpContextAccessor.HttpContext.Request.Headers["sessionid"][0];
                if (!string.IsNullOrEmpty(sessionId))
                {
                    headers.Add("sessionId", sessionId);
                }
            }

            return headers;
        }
    }
}