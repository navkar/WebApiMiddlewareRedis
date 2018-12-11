using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomHeaderMiddleware.Models
{
    public class ApiResponse<T>
    {
        public string Entity { get; set; }

        public T Result;
        public DateTimeOffset UTC { get; set; } = DateTimeOffset.UtcNow;
        public string Message { get; set; }
        public int StatusCode { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
