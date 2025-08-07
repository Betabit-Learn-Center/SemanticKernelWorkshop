using System.ComponentModel;
using Microsoft.SemanticKernel;
using SimpleFeedReader;

namespace Azure_Semantic_Kernel_Workshop
{
  public class NewsPlugin(INewsService newsService, ILogger<NewsPlugin> logger)
  {
    [KernelFunction("GetNews")]
    [Description("Retrieves a list of the latest news articles matching the specified search query. Returns up to the specified number of articles.")]
    [return: Description("List of news articles")]
    public async Task<List<FeedItem>> GetNewsAsync([Description("Search term to find relevant news articles")]string query, int count = 10)
    {
      logger.LogDebug("Retrieving news articles for query: {Query} with count: {Count}", query, count);
      return await newsService.GetNewsAsync(query, count);
    }
  }
}
