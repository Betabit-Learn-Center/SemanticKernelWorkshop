using SimpleFeedReader;

namespace Azure_Semantic_Kernel_Workshop
{
  public class NewsPlugin(INewsService newsService, ILogger<NewsPlugin> logger)
  {
    private readonly INewsService _newsService;
    private readonly ILogger<NewsPlugin> _logger;

    public NewsPlugin(INewsService newsService, ILogger<NewsPlugin> logger)
    {
      _newsService = newsService;
      _logger = logger;
    }

    public async Task<List<FeedItem>> GetNewsAsync(string query, int count = 10)
    {
      throw new NotImplementedException("This method is not implemented yet");
    }
  }
}
