using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace Azure_Semantic_Kernel_Workshop
{
  public class EmailPlugin(IEmailService emailService, ILogger<EmailPlugin> logger)
  {
    [KernelFunction("SendEmail")]
    [Description("Send an email to the user")]
    public async Task SendEmail(
        [Description("The subject of the email")] string subject,
        [Description("The body of the email, this should be in a nice html format with clean styling")] string body)
    {
      logger.LogInformation("Sending email with subject: {Subject}", subject);
      try
      {
        await emailService.SendEmailAsync(subject, body);
        logger.LogInformation("Email sent successfully with subject: {Subject}", subject);
      }
      catch (Exception ex)
      {
        logger.LogError(ex, "Failed to send email with subject: {Subject}", subject);
        throw;
      }
    }
  }
}
