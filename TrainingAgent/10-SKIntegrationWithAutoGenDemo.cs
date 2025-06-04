namespace TrainingAgent;

using AutoGen.Core;
using AutoGen.SemanticKernel;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;

public static class SKIntegrationWithAutoGenDemo
{
    public static async Task RunAsync()
    {
        var kernel = Kernel.CreateBuilder()
            .AddAzureOpenAIChatCompletion(
                deploymentName: ConfigurationUtils.GetConfigurationValue("AzureOpenAI:DeploymentName"),
                endpoint: ConfigurationUtils.GetConfigurationValue("AzureOpenAI:Endpoint"),
                apiKey: ConfigurationUtils.GetConfigurationValue("AzureOpenAI:ApiKey"))
            .Build();

        var chatAgent = new ChatCompletionAgent()
        {
            Kernel = kernel,
            Name = "ChatAgent",
            Description = "An agent that can chat with users using Azure OpenAI."
        };

        var messageConnector = new SemanticKernelChatMessageContentConnector();
        var skAgent = new SemanticKernelChatCompletionAgent(chatAgent)
            .RegisterMiddleware(messageConnector)
            .RegisterPrintMessage();

        var reply = await skAgent.SendAsync("Hey, can you crack a long joke for me?");

        Console.WriteLine("Agent Reply:");
        Console.WriteLine(reply.GetContent());
    }
}