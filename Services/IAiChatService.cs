using ChatCompletionDemo.Models;

namespace ChatCompletionDemo.Services;

public interface IAiChatService
{
    Task<ChatResult> GetResponseAsync(
        List<ChatMessage> messages,
        double temperature,
        int maxTokens);
}
