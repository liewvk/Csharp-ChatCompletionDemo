using System.Net.Http.Json;
using System.Text.Json;
using ChatCompletionDemo.Models;

namespace ChatCompletionDemo.Services;

public class OllamaChatService : IAiChatService, IDisposable
{
    private readonly HttpClient _client;

    private readonly string _modelName;

    public OllamaChatService(string modelName = "qwen3.5:9b")
    {
        _modelName = modelName;

        _client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:11434/"),
            Timeout = TimeSpan.FromMinutes(10)
        };
    }

    public async Task<ChatResult> GetResponseAsync(
        List<ChatMessage> messages,
        double temperature,
        int maxTokens)
    {
        var request = new
        {
            model = _modelName,
            messages = messages,
            stream = false,
            think = false,
            options = new
            {
                temperature = temperature,
                num_predict = maxTokens
            }
        };

        using HttpResponseMessage response =
            await _client.PostAsJsonAsync(
                "api/chat", request);

        string jsonText =
            await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Ollama API error {(int)response.StatusCode}: " +
                jsonText);
        }

        using JsonDocument json =
            JsonDocument.Parse(jsonText);

        JsonElement root = json.RootElement;

        string answer = root
            .GetProperty("message")
            .GetProperty("content")
            .GetString() ?? "";

        if (string.IsNullOrWhiteSpace(answer))
            throw new InvalidOperationException(
                "Ollama returned an empty answer.");

        int inputTokens =
            root.TryGetProperty("prompt_eval_count", out var input)
                ? input.GetInt32()
                : 0;

        int outputTokens =
            root.TryGetProperty("eval_count", out var output)
                ? output.GetInt32()
                : 0;

        long duration =
            root.TryGetProperty("total_duration", out var total)
                ? total.GetInt64()
                : 0;

        return new ChatResult
        {
            Answer = answer,
            InputTokens = inputTokens,
            OutputTokens = outputTokens,
            DurationSeconds = duration / 1_000_000_000.0
        };
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}
