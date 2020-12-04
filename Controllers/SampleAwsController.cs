using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JwtService.Entities;
using JwtService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleAwsController : ControllerBase
    {
        public IAwsService _awsService;
        public SampleAwsController(IAwsService awsService)
        {
            _awsService = awsService;
        }

        [HttpPost]
        [Route("create-queue/{queuename}")]
        public async Task<bool> CreateQueue([FromRoute]string queuename)
        {
            var response = await _awsService.CreateQueue(queuename);
            return response;
        }

        [HttpPost]
        [Route("send-message")]
        public async Task<string> SendMessage([FromBody]MessageRequest request)
        {
            try
            {
             var response = await _awsService.SendMessageAsync(request);

             return response;

            }
            catch
            {
                return "Not created";
            }
        }
    
        [HttpGet]
        [Route("receive-message/{queuename}")]
        public async Task<List<string>> ReceiveMessage([FromRoute]string queuename, [FromBody]string AccountId)
        {
            var response = await _awsService.RecieveMessage(queuename, AccountId);
            return response;
        }

    }
}