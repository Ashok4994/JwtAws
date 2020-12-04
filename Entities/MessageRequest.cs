using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtService.Entities
{
    public class MessageRequest
    {
        public string QueueName { get; set; }
        public string QueueOwnerAWSAccountId { get; set; }
        public string MessageBody { get; set; }
    }
}
