using ChatCompletionDemo.Models;
using ChatCompletionDemo.Services;

Console.Title = "C# Chat Completion Demo";

Console.WriteLine("==================================");
Console.WriteLine("       CHAT COMPLETION DEMO");
Console.WriteLine("       Visual Studio 2026");
Console.WriteLine("       Ollama + Qwen 3.5");
Console.WriteLine("==================================");

Console.WriteLine("Select AI Service:");
Console.WriteLine("1. Ollama (Real AI)");
Console.WriteLine("2. Mock AI");
Console.Write("Choice: ");

string? choice = Console.ReadLine();

bool useMock = choice == "2";

using var ollamaService = new OllamaChatService();

IAiChatService chatService = useMock
    ? new MockAiChatService()
    : ollamaService;

Console.WriteLine();
Console.WriteLine(useMock
    ? "Service: Mock AI"
    : "Service: Ollama / Qwen 3.5");

double temperature = 0.4;
int maxTokens = 800;

var history = new List<ChatMessage>
{
    new ChatMessage(
        "system",
        "You are a helpful C# programming assistant. " +
        "Explain concepts clearly with examples.")
};

Console.WriteLine("Commands: clear, settings, exit");

while (true)
{
    Console.WriteLine();
    Console.Write("You: ");

    string? input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input))
        continue;

    if (input.Equals("exit",
        StringComparison.OrdinalIgnoreCase))
        break;

    if (input.Equals("clear",
        StringComparison.OrdinalIgnoreCase))
    {
        history.RemoveRange(1, history.Count - 1);
        Console.WriteLine("Conversation cleared.");
        continue;
    }

    if (input.Equals("settings",
        StringComparison.OrdinalIgnoreCase))
    {
        Console.Write("Temperature (0 to 1): ");

        if (double.TryParse(
            Console.ReadLine(), out double temp) &&
            double.IsFinite(temp) &&
            temp >= 0 && temp <= 1)
        {
            temperature = temp;
        }

        Console.Write("Max tokens (100 to 4096): ");

        if (int.TryParse(
            Console.ReadLine(), out int tokens) &&
            tokens >= 100 && tokens <= 4096)
        {
            maxTokens = tokens;
        }

        Console.WriteLine(
            $"Settings: {temperature}, {maxTokens}");

        continue;
    }

    try
    {
        // Prepare the request without changing
        // the stored conversation yet.
        var messages = new List<ChatMessage>(history)
        {
            new ChatMessage("user", input)
        };

        Console.WriteLine("Processing...");

        ChatResult result =
            await chatService.GetResponseAsync(
                messages, temperature, maxTokens);

        Console.WriteLine();
        Console.WriteLine("Assistant:");
        Console.WriteLine(result.Answer);

        Console.WriteLine();
        Console.WriteLine("API Statistics:");
        Console.WriteLine(
            $"Input tokens: {result.InputTokens}");
        Console.WriteLine(
            $"Output tokens: {result.OutputTokens}");
        Console.WriteLine(
            $"Duration: {result.DurationSeconds:F2} seconds");

        // Save only successful conversations.
        history.Add(new ChatMessage("user", input));
        history.Add(new ChatMessage("assistant", result.Answer));
    }
    catch (HttpRequestException ex)
    {
        Console.WriteLine("API Error: " + ex.Message);
    }
    catch (TaskCanceledException)
    {
        Console.WriteLine("Request timed out.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error: " + ex.Message);
    }
}

Console.WriteLine("Goodbye!");

