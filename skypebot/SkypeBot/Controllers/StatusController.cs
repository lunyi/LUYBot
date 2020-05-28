using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SkypeBot.Models;

namespace SkypeBot.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/status")]
    public class StatusController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public StatusController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet("env")]
        public ServiceStatus GetEnvironment()
        {
            return new ServiceStatus
            {
                processor_count = Environment.ProcessorCount,
                current_server_time = DateTimeOffset.Now,
                server_name = Environment.MachineName,
                location = AppDomain.CurrentDomain.BaseDirectory,
                version = GetType().Assembly.GetName().Version.ToString(),
                last_built = new FileInfo(GetType().Assembly.Location).LastWriteTime,
                environment = _env.EnvironmentName
            };
        }
    }
}