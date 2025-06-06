namespace TrainingAgent;

using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;

public static class ConnectWithAzureOpenAIEx
{
    public async static Task RunAsync()
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
            .RegisterPrintMessage();

        await agent.SendAsync("Just Generate Code. Can you write a piece of C# code to calculate 100th fibonacci number?");
    }
}