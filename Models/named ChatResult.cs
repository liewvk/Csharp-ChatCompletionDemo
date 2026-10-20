namespace ChatCompletionDemo.Models;

public class ChatResult
{
    public string Answer { get; set; } = "";

    public int InputTokens { get; set; }

    public int OutputTokens { get; set; }

    public double DurationSeconds { get; set; }
}

