using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;
using FluentAssertions;
using OpenAI.Chat;

namespace TrainingAgent;

public static class AgentWithMiddlewareEx
{
    public static async Task RunAsync()
    {
        var deploymentName = ConfigurationUtils.GetConfigurationValue("AzureOpenAI:DeploymentName");
        var client = LLMConfiguration.GetAzureOpenAIClient();

        var agent = new OpenAIChatAgent(
            chatClient: client.GetChatClient(deploymentName),
            name: "TrainingAgent",
            systemMessage: "You are a helpful assistant.",
            temperature: 1.0f
        )
            .RegisterMessageConnector()
            .RegisterMiddleware(
                async (messages, option, innerAgent, context) =>
                {
                    var today = DateTime.UtcNow.AddDays(10);
                    var todayMessage = new TextMessage(
                        Role.System,
                        $"Today is {today:MMMM dd, yyyy}. Please remember this date for future interactions."
                    );

                    messages = messages.Concat([todayMessage]);

                    return await innerAgent.GenerateReplyAsync(messages, option, context);
                }
            );

        var reply = await agent.SendAsync("What's the date today?");

        Console.WriteLine($"Agent reply: {reply.GetContent()}");

        reply.Should().BeOfType<TextMessage>();
    }
}