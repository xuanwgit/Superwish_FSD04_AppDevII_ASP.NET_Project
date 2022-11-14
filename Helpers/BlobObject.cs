using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Superwish_FSD04_AppDevII_ASP.NET_Project.Helpers
{
    public class BlobObject
    {
        public Stream? Content { get; set; }
        public string? ContentType { get; set; }
    }
}