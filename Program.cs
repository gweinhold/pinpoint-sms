using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon;
using Amazon.Pinpoint;
using Amazon.Pinpoint.Model;

namespace pinpoint_sms
{
    class Program
    {
        // The AWS Region that you want to use to send the message. For a list of
        // AWS Regions where the Amazon Pinpoint API is available, see
        // https://docs.aws.amazon.com/pinpoint/latest/apireference/
        private static readonly string region = "us-west-2";

        // The phone number or short code to send the message from. The phone number
        // or short code that you specify has to be associated with your Amazon Pinpoint
        // account. For best results, specify long codes in E.164 format.
        private static readonly string originationNumber = "+17145551234";

        // The recipient's phone number.  For best results, you should specify the
        // phone number in E.164 format.
        private static readonly string destinationNumber = "+17148675309";

        // The content of the SMS message.
        private static readonly string message = "This message was sent through Amazon Pinpoint"
                + "using the AWS SDK for .NET. Reply STOP to opt out.";

        // The Pinpoint project/application ID to use when you send this message.
        // Make sure that the SMS channel is enabled for the project or application
        // that you choose.
        private static readonly string appId = "0591d3f796b344db8152f1bf5207c631";

        // The type of SMS message that you want to send. If you plan to send
        // time-sensitive content, specify TRANSACTIONAL. If you plan to send
        // marketing-related content, specify PROMOTIONAL.
        private static readonly string messageType = "TRANSACTIONAL";

        // The registered keyword associated with the originating short code.
        private static readonly string registeredKeyword = "myKeyword";

        // The sender ID to use when sending the message. Support for sender ID
        // varies by country or region. For more information, see
        // https://docs.aws.amazon.com/pinpoint/latest/userguide/channels-sms-countries.html
        private static readonly string senderId = "mySenderId";

        static void Main(string[] args)
        {
            SendMessage().Wait();
        }

        private static async Task SendMessage()
        {
            using (AmazonPinpointClient client = new AmazonPinpointClient(RegionEndpoint.GetBySystemName(region)))
            {
                var sendRequest = new SendMessagesRequest
                {
                    ApplicationId = appId,
                    MessageRequest = new MessageRequest
                    {
                        Addresses = new Dictionary<string, AddressConfiguration>
                        {
                            {
                                destinationNumber,
                                new AddressConfiguration
                                {
                                    ChannelType = "SMS"
                                }
                            }
                        },
                        MessageConfiguration = new DirectMessageConfiguration
                        {
                            SMSMessage = new SMSMessage
                            {
                                Body = message,
                                MessageType = messageType,
                                OriginationNumber = originationNumber,
                                SenderId = senderId,
                                Keyword = registeredKeyword
                            }
                        }
                    }
                };
                try
                {
                    Console.WriteLine("Sending message...");
                    var response = await client.SendMessagesAsync(sendRequest).ConfigureAwait(false);
                    Console.WriteLine("Message sent!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("The message wasn't sent. Error message: " + ex.Message);
                }
            }

        }
    }
}
