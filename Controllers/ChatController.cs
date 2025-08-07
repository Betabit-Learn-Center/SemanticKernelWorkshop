using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace Azure_Semantic_Kernel_Workshop.Controllers
{
    [ApiController]
    [ApiKey]
    [RateLimit(100)]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly Kernel _kernel;
        private readonly IChatCompletionService _chatService;
        private static readonly ChatHistory _chatMessages = new();
        private IConfiguration _configuration;

        public ChatController(Kernel kernel, IChatCompletionService chatService, IConfiguration configuration)
        {
            _kernel = kernel;
            _chatService = chatService;
            _configuration = configuration;

            // Initialize system message if not already added
            if (_chatMessages.Count == 0)
            {
                var systemMessage = _configuration["SYSTEM_PROMPT"] ?? throw new InvalidOperationException("System message is not configured.");
                _chatMessages.AddSystemMessage(systemMessage);
            }
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] ChatRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
            {
                return BadRequest("Message cannot be empty");
            }

            try
            {
                _chatMessages.AddUserMessage(request.Message);

                var completion = _chatService.GetStreamingChatMessageContentsAsync(
                    _chatMessages,
                    new OpenAIPromptExecutionSettings
                    {
                        ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
                    },
                    _kernel);

                string fullMessage = "";
                await foreach (var content in completion)
                {
                    fullMessage += content.Content;
                }

                _chatMessages.AddAssistantMessage(fullMessage);

                return Ok(new ChatResponse { Message = fullMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("history")]
        public IActionResult GetChatHistory()
        {
            var history = _chatMessages
                .Where(msg => msg.Role != AuthorRole.System)
                .Select(msg => new { Role = msg.Role.ToString(), Content = msg.Content })
                .ToList();
            return Ok(history);
        }

        [HttpPost("clear")]
        public IActionResult ClearHistory()
        {
            // Keep only the system message
            var systemMessage = _chatMessages.FirstOrDefault(msg => msg.Role == AuthorRole.System);
            _chatMessages.Clear();
            if (systemMessage != null)
            {
                _chatMessages.Add(systemMessage);
            }
            return Ok();
        }
    }

    public class ChatRequest
    {
        public string Message { get; set; } = string.Empty;
    }

    public class ChatResponse
    {
        public string Message { get; set; } = string.Empty;
    }
}
