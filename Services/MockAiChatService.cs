using ChatCompletionDemo.Models;

namespace ChatCompletionDemo.Services;

public class MockAiChatService : IAiChatService
{
    public Task<ChatResult> GetResponseAsync(
        List<ChatMessage> messages,
        double temperature,
        int maxTokens)
    {
        string question = messages
            .LastOrDefault(m => m.Role == "user")?
            .Content ?? "";

        var result = new ChatResult
        {
            Answer = $"Mock response: You asked '{question}'.",
            InputTokens = 0,
            OutputTokens = 0,
            DurationSeconds = 0
        };

        return Task.FromResult(result);
    }
}

