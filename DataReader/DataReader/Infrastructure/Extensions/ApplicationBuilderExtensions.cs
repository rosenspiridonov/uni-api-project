using DataReader.Infrastructure.Middlewares;

namespace DataReader.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseIPFilter(this IApplicationBuilder builder, string allowedIp)
        {
            return builder.UseMiddleware<IPFilterMiddleware>(allowedIp);
        }
    }
}
