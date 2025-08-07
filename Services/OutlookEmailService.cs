using Microsoft.Graph;
using Microsoft.Graph.Me.SendMail;
using Microsoft.Graph.Models;

namespace Azure_Semantic_Kernel_Workshop
{
    public class OutlookEmailService : IEmailService
    {
        private readonly string toEmail = string.Empty;
        private readonly GraphServiceClient _graphClient;
        private readonly ILogger<OutlookEmailService> _logger;

        public OutlookEmailService(IConfiguration configuration, GraphServiceClient graphClient, ILogger<OutlookEmailService> logger)
        {
            toEmail = configuration["TO_EMAIL"] ?? throw new ArgumentNullException("TO_EMAIL");
            _graphClient = graphClient;
            _logger = logger;

            _logger.LogInformation("OutlookEmailService initialized with recipient: {ToEmail}", toEmail);
        }

        public async Task SendEmailAsync(string subject, string body)
        {
            _logger.LogInformation("Sending email to {Recipient} with subject: {Subject}", toEmail, subject);
            try
            {
                var message = new SendMailPostRequestBody
                {
                    Message = new Message
                    {
                        Subject = subject,
                        Body = new ItemBody
                        {
                            ContentType = BodyType.Html,
                            Content = body
                        },
                        ToRecipients = new List<Recipient>
                        {
                            new Recipient
                            {
                                EmailAddress = new EmailAddress
                                {
                                    Address = toEmail
                                }
                            }
                        }
                    },
                    SaveToSentItems = true
                };

                await _graphClient.Me.SendMail.PostAsync(message);
                _logger.LogInformation("Email sent successfully to {Recipient} with subject: {Subject}", toEmail, subject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Recipient} with subject: {Subject}", toEmail, subject);
                throw;
            }
        }
    }
}

