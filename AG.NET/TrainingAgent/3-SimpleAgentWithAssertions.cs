using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;
using FluentAssertions;

namespace TrainingAgent;

public static class SimpleAgentWithAssertions
{
    public static async Task RunAsync()
    {
        var deploymentName = ConfigurationUtils.GetConfigurationValue("AzureOpenAI:DeploymentName");
        var client = LLMConfiguration.GetAzureOpenAIClient();

        var agent = new OpenAIChatAgent(
            chatClient: client.GetChatClient(deploymentName),
            name: "TrainingAgent",
            systemMessage: "You are a helpful assistant. You convert user said into capital letters.",
            temperature: 1.0f
        )
            .RegisterMessageConnector()
            .RegisterPrintMessage();

        var reply = await agent.SendAsync("Hello World");

        Console.WriteLine($"Agent reply: {reply.GetContent()}");

        reply.Should().BeOfType<TextMessage>();
        reply.GetContent().Should().Be("HELLO WORLD");

        var conversationHistory = new List<IMessage>
        {
            new TextMessage(Role.User, "Hello World"),
            reply
        };

        reply = await agent.SendAsync("What did I just say?", conversationHistory);

        Console.WriteLine($"Agent reply: {reply.GetContent()}");
    }
}