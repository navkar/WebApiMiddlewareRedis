using CustomHeaderMiddleware.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Configuration;
using System;
using System.Collections.Generic;

namespace CustomHeaderMiddleware.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : BaseController
    {
        private string PREFIX = "Profile::";
        public ProfileController(ISerializer serializer, IOptions<RedisConfiguration> config) : base(serializer, config)
        {
        }

        [HttpGet]
        public ActionResult<IEnumerable<Profile>> Get()
        {
            //var bundleId = HttpContext.Request.Headers["BundleId"];
            var keys = CacheClient.SearchKeys(PREFIX + "*");
            var profiles = new List<Profile>();

            foreach(string key in keys)
            {
                profiles.Add(Get<Profile>(key));
            }

            return Ok(new ApiResponse<IEnumerable<Profile>>() { Result = profiles });
        }

        [HttpGet("{id}")]
        public ActionResult<Profile> Get(string id)
        {
            var response = Get<Profile>(PREFIX + id);
            if (response == null) return NotFound();
            return Ok(new ApiResponse<Profile>() { Result = response } );
        }

        [HttpPost]
        public IActionResult Post([FromBody] Profile profile)
        {
            profile.Id = DateTime.UtcNow.Ticks.ToString();
            var key = PREFIX + profile.Id;
            bool added = Add(key, profile, DateTimeOffset.Now.AddMinutes(10));
            return Ok(new ApiResponse<string>() { Result = profile.Id });
        }

    }
}
