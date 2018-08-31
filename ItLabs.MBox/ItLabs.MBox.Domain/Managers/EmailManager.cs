using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

namespace ItLabs.MBox.Domain.Managers
{
    public class EmailManager
    {
        public async Task<bool> SendEmail(string to, string subject, string html)
        {
            // might want to provide credentials
            var sendTo = new List<string>{ to };
            using (var ses = new AmazonSimpleEmailServiceClient(RegionEndpoint.EUCentral1))

            {
                var sendResult = await ses.SendEmailAsync(new SendEmailRequest
                {
                    Source = "ENTER SOURCE EMAIL HERE WHEN CREATED",
                    Destination = new Destination(sendTo),
                    Message = new Message
                    {
                        Subject = new Content(subject),
                        Body = new Body
                        {
                            Html = new Content(html)
                        }
                    }
                });
                return sendResult.HttpStatusCode == HttpStatusCode.OK;
            }
        }
    }
}
