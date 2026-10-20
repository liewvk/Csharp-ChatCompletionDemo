# Csharp ChatCompletionDemo

A simple C# console application that demonstrates chat-completion-style interactions with an AI model using local Ollama integration. This project is part of the "C# AI Agent ME Chapter 6" learning exercise and shows how to build a lightweight conversational AI client in .NET.

## Overview

The app lets you:

- choose between a real Ollama-backed AI service and a mock AI service
- send prompts to the model from the console
- maintain a short conversational history
- adjust generation settings such as temperature and max tokens
- view response metadata, including token usage and timing

## Features

- Console-based chat interface
- Local AI inference via Ollama
- Mock service for testing without a model
- configurable conversation settings
- simple request/response model classes

## Project Structure

- `Program.cs` – console app entry point and interactive chat loop
- `Models/ChatMessage.cs` – chat message model
- `Models/named ChatResult.cs` – response result model
- `Services/IAiChatService.cs` – shared service contract
- `Services/Create OllamaChatService.cs` – Ollama HTTP client implementation
- `Services/MockAiChatService.cs` – mock implementation for offline/demo use
- `ChatCompletionDemo.csproj` – .NET project file

## Prerequisites

Before running the app, make sure you have:

- .NET 10 SDK installed
- Ollama installed and running locally
- the model `qwen3.5:9b` available

To pull the required model:

```bash
ollama pull qwen3.5:9b
```

If Ollama is not already running:

```bash
ollama serve
```

## Getting Started

1. Clone the repository
2. Restore dependencies:

```bash
dotnet restore
```

3. Run the project:

```bash
dotnet run
```

## Usage

When the app starts, it asks you to choose a service:

- `1` – Ollama (real AI)
- `2` – Mock AI

After that, you can type prompts into the console.

Available commands:

- `clear` – clears the conversation history
- `settings` – changes temperature and max token values
- `exit` – exits the app

## Example

```text
Select AI Service:
1. Ollama (Real AI)
2. Mock AI
Choice: 1

Service: Ollama / Qwen 3.5

You: Explain async/await in C#

Assistant:
async/await allows you to write asynchronous code...

API Statistics:
Input tokens: 43
Output tokens: 180
Duration: 1.27 seconds
```

## Notes

- The app uses the Ollama API at `http://localhost:11434/`
- `temperature` controls randomness in the model output
- `maxTokens` limits the length of the generated response
- Successful chat turns are saved to the in-memory conversation history

## License

This project does not currently include a license file. If you plan to share or distribute it publicly, consider adding an appropriate open-source license.
