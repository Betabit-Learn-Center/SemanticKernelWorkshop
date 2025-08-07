using Azure.AI.TextAnalytics;

namespace Azure_Semantic_Kernel_Workshop
{
    public class AzureTextAnalytics : ITextAnalysisService
    {
        private readonly TextAnalyticsClient _client;
        private readonly ILogger<AzureTextAnalytics> _logger;

        public AzureTextAnalytics(TextAnalyticsClient client, ILogger<AzureTextAnalytics> logger)
        {
            _client = client;
            _logger = logger;
        }
        public async Task<string> AnalyzeSentimentAsync(string text)
        {
            _logger.LogInformation("Analyzing sentiment for text of length: {TextLength}", text.Length);
            try
            {
                var response = await _client.AnalyzeSentimentAsync(text);
                var sentiment = response.Value.Sentiment.ToString();
                _logger.LogInformation("Sentiment analysis completed. Result: {Sentiment}", sentiment);
                return sentiment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error analyzing sentiment for text");
                throw;
            }
        }

        public async Task<string> DetectLanguageAsync(string text)
        {
            _logger.LogInformation("Detecting language for text of length: {TextLength}", text.Length);
            try
            {
                var response = await _client.DetectLanguageAsync(text);
                var language = response.Value.Iso6391Name;
                _logger.LogInformation("Language detection completed. Result: {Language}", language);
                return language;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error detecting language for text");
                throw;
            }
        }

        public async Task<string> ExtractKeyPhrasesAsync(string text)
        {
            _logger.LogInformation("Extracting key phrases for text of length: {TextLength}", text.Length);
            try
            {
                var response = await _client.ExtractKeyPhrasesAsync(text);
                var keyPhrases = string.Join(", ", response.Value);
                _logger.LogInformation("Key phrase extraction completed. Found {Count} phrases", response.Value.Count);
                return keyPhrases;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error extracting key phrases for text");
                throw;
            }
        }
    }
}
