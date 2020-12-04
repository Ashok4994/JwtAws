using Amazon;
using Amazon.SQS;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtService.Helpers
{
    public abstract class AwsClient
    {

        public IOptionsMonitor<AwsCredentials> _awsCredentials;

        private static IAmazonSQS sqsclient;

        static string awsAccessKeyId = "AKIAYZCL4NMT3IOJ6GVS";
        static string awsSecretAccessKey = "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";
        //IAmazonSQS sqsClient = new AmazonSQSClient(awsAccessKeyId, awsSecretAccessKey, RegionEndpoint.USWest2);

        public AwsClient(IOptionsMonitor<AwsCredentials> awsCredentials)
        {
            _awsCredentials = awsCredentials;
            sqsclient = new AmazonSQSClient(_awsCredentials.CurrentValue.AccessKeyId, _awsCredentials.CurrentValue.SecretAccessKey, RegionEndpoint.USWest2);
        }

        protected AwsClient()
        {
        }
    }
}
