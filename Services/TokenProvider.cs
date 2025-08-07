using Microsoft.Graph;
using Microsoft.Identity.Web;
using Microsoft.Kiota.Abstractions.Authentication;

namespace Azure_Semantic_Kernel_Workshop
{
    public class TokenProvider : IAccessTokenProvider
    {
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly string[] _scopes;

        public TokenProvider(ITokenAcquisition tokenAcquisition, IConfiguration configuration)
        {
            _tokenAcquisition = tokenAcquisition;

            // Get scopes from configuration, fallback to default if not configured
            var configuredScopes = configuration["Graph:Scopes"];
            if (!string.IsNullOrWhiteSpace(configuredScopes))
            {
                // Split space-separated scopes and convert to full Graph API URLs
                _scopes = configuredScopes.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                       .Select(scope => $"https://graph.microsoft.com/{scope}")
                                       .ToArray();
            }
            else
            {
                _scopes = ["https://graph.microsoft.com/.default"];
            }
        }

        public Task<string> GetAuthorizationTokenAsync(Uri uri, Dictionary<string, object>? additionalAuthenticationContext = null, CancellationToken cancellationToken = default)
        {
            return _tokenAcquisition.GetAccessTokenForUserAsync(_scopes);
        }

        public AllowedHostsValidator AllowedHostsValidator { get; } = new AllowedHostsValidator();
    }
}
