using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomHeaderMiddleware
{
    public interface IHeaders
    {
        string BundleId { get; set; }
    }
    public class CustomHeaders : IHeaders
    {
        public string BundleId { get; set; }
    }
}
