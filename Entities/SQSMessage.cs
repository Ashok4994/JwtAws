using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtService.Entities
{
    public class SQSMessage
    {
        public Dictionary<string, string> Attributes { get; set; }     
        public string Body { get; set; }
    }
}
