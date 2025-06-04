using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;
using FluentAssertions;
using OpenAI.Chat;

namespace TrainingAgent;

public static class SimpleToolsWithAgents
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
            ],
            functionMap: new Dictionary<string, Func<string, Task<string>>>
            {
                { nameof(tools.UpperCase), tools.UpperCaseWrapper },
                { nameof(tools.ConcatString), tools.ConcatStringWrapper },
                { nameof(tools.CalculateTax), tools.CalculateTaxWrapper }
            }
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

        var reply = await agent.SendAsync("convert to upper case: hello world");

        reply.Should().NotBeNull();
        reply.Should().BeOfType<ToolCallAggregateMessage>();
        reply.GetContent().Should().Be("HELLO WORLD");

        reply = await agent.SendAsync("concatenate strings: Microsoft, AutoGen, and OpenAI");

        reply.Should().NotBeNull();
        reply.Should().BeOfType<ToolCallAggregateMessage>();
        reply.GetContent().Should().Be("Microsoft AutoGen OpenAI");
        
        reply = await agent.SendAsync("calculate tax on 100 with a tax rate of 0.2");   

        reply.Should().NotBeNull();
        reply.Should().BeOfType<ToolCallAggregateMessage>();
    }
}