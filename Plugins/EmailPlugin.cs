namespace Azure_Semantic_Kernel_Workshop
{
  public class EmailPlugin(IEmailService emailService, ILogger<EmailPlugin> logger)
  {
    private readonly IEmailService _emailService;
    private readonly ILogger<EmailPlugin> _logger;

    public EmailPlugin(IEmailService emailService, ILogger<EmailPlugin> logger)
    {
      _emailService = emailService;
      _logger = logger;
    }

    public async Task SendEmail()
    {
      throw new NotImplementedException("This method is not implemented yet");
    }
  }
}
