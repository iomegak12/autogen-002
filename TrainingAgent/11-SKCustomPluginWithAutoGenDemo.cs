using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;
using AutoGen.SemanticKernel;
using Microsoft.SemanticKernel;

namespace TrainingAgent;

public static class SKCustomPluginWithAutoGenDemo
{
    public static async Task RunAsync()
    {
        var kernel = Kernel.CreateBuilder()
            .Build();

        var getWeatherFunction = KernelFunctionFactory.CreateFromMethod(
            method: (string location) => $"The weather in {location} is sunny. Temperature is 25°C.",
            functionName: "GetWeather",
            description: "Get the current weather for a given location.");
        var getWeatherPlugin = kernel.CreatePluginFromFunctions("my_plugin", [getWeatherFunction]);

        var deploymentName = ConfigurationUtils.GetConfigurationValue("AzureOpenAI:DeploymentName");
        var client = LLMConfiguration.GetAzureOpenAIClient();
        var systemPrompt = @"You are a helpful assistant that can provide weather information using the GetWeather function from my_plugin.";
        var agent = new OpenAIChatAgent(
           chatClient: client.GetChatClient(deploymentName),
           name: "ToolsAgent",
           systemMessage: systemPrompt,
           temperature: 1.0f
        )
            .RegisterMessageConnector()
            .RegisterMiddleware(new KernelPluginMiddleware(kernel, getWeatherPlugin))
            .RegisterPrintMessage();

        var toolAggregateMessage = await agent.SendAsync("Tell me the weather in New York City.");

        Console.WriteLine("Tool Aggregate Message:");
        Console.WriteLine($"Role: {toolAggregateMessage.GetRole()}");
        Console.WriteLine($"Content: {toolAggregateMessage.GetContent()}");
    }
}