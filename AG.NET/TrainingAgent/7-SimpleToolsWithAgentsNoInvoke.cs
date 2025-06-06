using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;
using FluentAssertions;
using OpenAI.Chat;

namespace TrainingAgent;

public static class SimpleToolsWithAgentsNoInvoke
{
    public static async Task RunAsync()
    {
        var deploymentName = ConfigurationUtils.GetConfigurationValue("AzureOpenAI:DeploymentName");
        var client = LLMConfiguration.GetAzureOpenAIClient();
        var tools = new Tools();
        var toolsCallMiddleware = new FunctionCallMiddleware(
            functions:
            [
                tools.UpperCaseFunctionContract,
                tools.ConcatStringFunctionContract,
                tools.CalculateTaxFunctionContract
            ]
        );

        var agent = new OpenAIChatAgent(
            chatClient: client.GetChatClient(deploymentName),
            name: "ToolsAgent",
            systemMessage: "You are a helpful assistant.",
            temperature: 1.0f
        )
            .RegisterMessageConnector()
            .RegisterStreamingMiddleware(toolsCallMiddleware)
            .RegisterPrintMessage();

        var reply = await agent.SendAsync("calculate tax on 100 with a tax rate of 0.2");

        reply.Should().NotBeNull();
        reply.Should().BeOfType<ToolCallMessage>();

        Console.WriteLine($"Tax calculation result: {reply.GetContent()}");

        reply = await agent.SendAsync(
            "Calculate Tax on 100 with a tax rate of 0.35%; 2000 with a tax rate of 0.15%");

        reply.Should().NotBeNull();
        reply.Should().BeOfType<ToolCallMessage>();

        Console.WriteLine($"Tax calculation result: {reply.GetContent()}");
    }
}