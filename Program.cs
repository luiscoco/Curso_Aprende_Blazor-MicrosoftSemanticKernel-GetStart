using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.ComponentModel;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = Kernel.CreateBuilder();

builder.AddAzureOpenAIChatCompletion("gpt-4o",
    "https://luiscocoaiservice.openai.azure.com/",
    "",
    "gpt-4o");

var kernel = builder.Build();

// Example 1. Invoke the kernel with a prompt and display the result
Console.WriteLine(await kernel.InvokePromptAsync("What color is the sky?"));
Console.WriteLine();

// Example 2. Invoke the kernel with a templated prompt and display the result
KernelArguments arguments = new() { { "topic", "sea" } };
Console.WriteLine(await kernel.InvokePromptAsync("What color is the {{$topic}}?", arguments));
Console.WriteLine();

// Example 3. Invoke the kernel with a templated prompt and stream the results to the display
await foreach (var update in kernel.InvokePromptStreamingAsync("What color is the {{$topic}}? Provide a detailed explanation.", arguments))
{
    Console.Write(update);
}

Console.WriteLine(string.Empty);

// Example 4. Invoke the kernel with a templated prompt and execution settings
arguments = new(new OpenAIPromptExecutionSettings { MaxTokens = 500, Temperature = 0.5 }) { { "topic", "dogs" } };
Console.WriteLine(await kernel.InvokePromptAsync("Tell me a story about {{$topic}}", arguments));

// Example 5. Invoke the kernel with a templated prompt and execution settings configured to return JSON
#pragma warning disable SKEXP0010
arguments = new(new OpenAIPromptExecutionSettings { ResponseFormat = "json_object" }) { { "topic", "chocolate" } };
Console.WriteLine(await kernel.InvokePromptAsync("Create a recipe for a {{$topic}} cake in JSON format", arguments));