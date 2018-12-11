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
        private string PREFIX = "urn:profile:";
        private string C_PREFIX = "urn:profiles";
        public ProfileController(ISerializer serializer, IOptions<RedisConfiguration> config) : base(serializer, config)
        {
        }

        [HttpGet]
        public ActionResult<IEnumerable<Profile>> Get()
        {
            //var bundleId = HttpContext.Request.Headers["BundleId"];
            var response = Get<IEnumerable<Profile>>(C_PREFIX);
            if (response == null) return NotFound();
            return Ok(new ApiResponse<IEnumerable<Profile>>() { Result = response, Entity = "Profiles" });
        }

        [HttpGet("{id}")]
        public ActionResult<Profile> Get(string id)
        {
            var response = Get<Profile>(PREFIX + id);
            if (response == null) return NotFound();
            return Ok(new ApiResponse<Profile>() { Result = response, Entity = "Profile" } );
        }

        [HttpPost]
        public IActionResult Post([FromBody] Profile profile)
        {
            profile.Id = DateTime.UtcNow.Ticks.ToString();
            var key = PREFIX + profile.Id;
            bool added = Add(key, profile);

            SaveSortedProfileCollection();

            return Ok(new ApiResponse<string>() { Result = profile.Id, Entity = "Profile" });
        }

        private bool SaveSortedProfileCollection()
        {
            var ids = CacheClient.SearchKeys(PREFIX + "*");
            var profiles = new List<Profile>();

            foreach (string id in ids)
            {
                profiles.Add(Get<Profile>(id));
            }

            profiles.Sort();
            var key = C_PREFIX;
            return Add(key, profiles);
        }

    }
}
