using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomHeaderMiddleware.Models
{
    public class Profile : IComparable<Profile>
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public long JoinedOn { get; set; } = DateTimeOffset.UtcNow.Ticks;

        public int CompareTo(Profile other)
        {
            return other.JoinedOn.CompareTo(this.JoinedOn);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
