using System.Net;

namespace DataReader.Infrastructure.Middlewares
{
    public class IPFilterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IPAddress _allowedIpV4;
        private readonly IPAddress _allowedIpV6;

        public IPFilterMiddleware(RequestDelegate next, string allowedIp)
        {
            _next = next;
            _allowedIpV4 = IPAddress.Parse(allowedIp);
            _allowedIpV6 = IPAddress.Parse("::1");
        }

        public async Task Invoke(HttpContext context)
        {
            var remoteIp = context.Connection.RemoteIpAddress;
            if (remoteIp == null || (!remoteIp.Equals(_allowedIpV4) && !remoteIp.Equals(_allowedIpV6)))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }

            await _next.Invoke(context);
        }
    }
}
