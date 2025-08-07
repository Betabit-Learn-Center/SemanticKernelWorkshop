namespace Azure_Semantic_Kernel_Workshop
{
  public class TextAnalysisPlugin(ITextAnalysisService textAnalysisService, ILogger<TextAnalysisPlugin> logger)
  {
    private readonly ITextAnalysisService _textAnalysisService;
    private readonly ILogger<TextAnalysisPlugin> _logger;

    public TextAnalysisPlugin(ITextAnalysisService textAnalysisService, ILogger<TextAnalysisPlugin> logger)
    {
      _textAnalysisService = textAnalysisService;
      _logger = logger;
    }

    public async Task<string> AnalyzeSentimentAsync()
    {
      throw new NotImplementedException("This method is not implemented yet");
    }

    public async Task<string> ExtractKeyPhrasesAsync()
    {
      throw new NotImplementedException("This method is not implemented yet");
    }

    public async Task<string> DetectLanguageAsync()
    {
      throw new NotImplementedException("This method is not implemented yet");
    }

  }
}
