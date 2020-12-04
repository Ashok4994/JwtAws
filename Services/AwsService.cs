using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JwtService.Helpers;
using JwtService.Entities;

namespace JwtService.Services
{

    public interface IAwsService
    {
        Task<bool> CreateQueue(string queuename);
        Task<string> SendMessageAsync(MessageRequest request);
        Task<List<string>> RecieveMessage(string queuename, string accountId);
    }
    public class AwsService : IAwsService
    {
        private readonly string awsAccessKeyId = "AKIAYZCL4NMT3IOJ6GVS";
        private readonly string awsSecretAccessKey = "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";
        private readonly IAmazonSQS _sqsClient;

        public AwsService()
        {
             _sqsClient = new AmazonSQSClient(awsAccessKeyId, awsSecretAccessKey, RegionEndpoint.USWest2);
        }

        public async Task<bool> CreateQueue(string queuename)
        {
                      
            var createRequest = new CreateQueueRequest()
            {
                QueueName = queuename
            };

            var createResponse = await _sqsClient.CreateQueueAsync(createRequest);

            if(createResponse != null)
            {
                return true;
            }
            return false;

        }

        public async Task<string> SendMessageAsync(MessageRequest sendrequest)
        {           
            //Get Queue url
            var queueUrl = GetQueueUrl(sendrequest.QueueName,sendrequest.QueueOwnerAWSAccountId);
          

            //Send message
            var sendMessageRequest = new SendMessageRequest()
            {
                QueueUrl = queueUrl.Result,
                MessageBody = sendrequest.MessageBody
            };

            var sendMessageResponse = await _sqsClient.SendMessageAsync(sendMessageRequest);

            if (sendMessageResponse != null)
            {
                return "success";
            };
            return "failure";

        }

        public async Task<List<string>> RecieveMessage(string queuename,string accountId)
        {
             List<string> messages = new List<string>();

            var queueUrl = GetQueueUrl(queuename, accountId);

            var receiveMessageRequest = new ReceiveMessageRequest()
            {
                QueueUrl = queueUrl.Result,             
            };

            var receiveMessageResponse = await _sqsClient.ReceiveMessageAsync(receiveMessageRequest);

            foreach(var message in receiveMessageResponse.Messages)
            {
                if (message.Body != null)
                    messages.Add(message.Body);
                    
            }
            return messages;
        }

        private async Task<string> GetQueueUrl(string queuename, string accountId)
        {
            var request = new GetQueueUrlRequest
            {
                QueueName = queuename,
                QueueOwnerAWSAccountId = accountId
            };

            var queueUrlresponse =  await _sqsClient.GetQueueUrlAsync(request);

            if(queueUrlresponse != null)
            {
                return queueUrlresponse.QueueUrl;
            }

            return null;
        }
    }
}
