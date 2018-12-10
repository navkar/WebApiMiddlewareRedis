using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomHeaderMiddleware.Models
{
    public class ApiResponse<T>
    {
        public T Result;
        public DateTimeOffset UTC { get; set; } = DateTimeOffset.UtcNow;
        public string Message { get; set; }
    }
}
