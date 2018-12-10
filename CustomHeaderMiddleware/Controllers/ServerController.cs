using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Configuration;
using System.Collections.Generic;

namespace CustomHeaderMiddleware.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerController : BaseController
    {
        public ServerController(ISerializer serializer, IOptions<RedisConfiguration> config) : base(serializer, config)
        {
        }

        [HttpGet("")]
        public ActionResult<Dictionary<string,string>> Get(string id)
        {
            return CacheClient.GetInfo();
        }
    }
}