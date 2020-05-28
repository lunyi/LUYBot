using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using UGS.Infrastructures.Extensions;

namespace SkypeBot.Middleware
{
    public class BotLoggerMiddleware
    {
        private readonly RequestDelegate _next;
        
        private readonly string[] _bypassRouting = {"api/status", "api/messages"};
        
        private const string HttpForwardFor = "X-Forwarded-For";
        private const string UnknownIp = "0.0.0.0";

        private static readonly char[] Splitters = { ',', ';' };

        public BotLoggerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var requestPath = context.Request.Path.Value;
            if (!requestPath.IgnoreCaseContains("/api") ||
                _bypassRouting.Any(r => requestPath.IgnoreCaseContains(r)))
            {
                await _next(context);
                return;
            }

            var body = context.Response.Body;

            using (var requestBody = new MemoryStream())
            using (var responseBody = new MemoryStream())
            {
                string request;
                using (var requestReader = new StreamReader(context.Request.Body))
                {
                    request = await requestReader.ReadToEndAsync();

                    var requestData = Encoding.UTF8.GetBytes(request);
                    requestBody.Write(requestData, 0, requestData.Length);
                    context.Request.Body = requestBody;
                    context.Request.Body.Seek(0, SeekOrigin.Begin);
                }

                context.Response.Body = responseBody;

                var sw = Stopwatch.StartNew();
                await _next(context);
                sw.Stop();

                //get response
                string response;
                using (var responseReader = new StreamReader(context.Response.Body))
                {
                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                    response = await responseReader.ReadToEndAsync();
                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                    await responseBody.CopyToAsync(body);
                }
            }
        }

        private static string GetIp(HttpContext context)
        {
            if (context.Request.Headers == null)
            {
                return UnknownIp;
            }

            var remoteIp = context.Connection.RemoteIpAddress;
            var userHostAddress = remoteIp.IsIPv4MappedToIPv6 ?
                remoteIp.MapToIPv4().ToString() : remoteIp.ToString();

            if (!context.Request.Headers.ContainsKey(HttpForwardFor))
            {
                return userHostAddress;
            }

            var ip = context.Request.Headers[HttpForwardFor].FirstOrDefault();
            if (ip.IsNullOrEmpty())
            {
                return userHostAddress;
            }

            return ip.Split(Splitters)
                       .Select(e => e.Trim())
                       .FirstOrDefault() ?? userHostAddress;
        }
    }

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseBotLogger(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BotLoggerMiddleware>();
        }
    }
}