using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;
using FluentAssertions;
using OpenAI.Chat;

namespace TrainingAgent;

public static class AgentWithMiddleware
{
    public static async Task RunAsync()
    {
        var deploymentName = ConfigurationUtils.GetConfigurationValue("AzureOpenAI:DeploymentName");
        var client = LLMConfiguration.GetAzureOpenAIClient();
        var totalTokenCount = 0;

        var agent = new OpenAIChatAgent(
            chatClient: client.GetChatClient(deploymentName),
            name: "TrainingAgent",
            systemMessage: "You are a helpful assistant.",
            temperature: 1.0f
        )
            .RegisterMiddleware(
                async (messages, option, innerAgent, context) =>
                {
                    var reply = await innerAgent.GenerateReplyAsync(messages, option, context);

                    if (reply is MessageEnvelope<ChatCompletion> chatCompletions)
                    {
                        var tokenCount = chatCompletions.Content.Usage?.TotalTokenCount ?? 0;

                        totalTokenCount += tokenCount;

                        Console.WriteLine($"Total token count: {totalTokenCount}");
                    }

                    return reply;
                }
            )
            .RegisterMiddleware(new OpenAIChatRequestMessageConnector());

        var reply = await agent.SendAsync("Can you crack a joke about Batman Joker?");

        Console.WriteLine($"Agent reply: {reply.GetContent()}");
        Console.WriteLine($"Total token count after first reply: {totalTokenCount}");

        reply.Should().BeOfType<TextMessage>();
        totalTokenCount.Should().BeGreaterThan(0);
    }
}