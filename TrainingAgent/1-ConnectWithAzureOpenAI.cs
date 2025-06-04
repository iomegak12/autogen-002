namespace TrainingAgent;

using System;
using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;
using Azure.AI.OpenAI;

public static class ConnectWithAzureOpenAI
{
    public async static Task RunAsync()
    {
        // Initialize the configuration
        var endpointUrl = ConfigurationUtils.GetConfigurationValue("AzureOpenAI:Endpoint");

        if (string.IsNullOrEmpty(endpointUrl))
        {
            Console.WriteLine("Azure OpenAI Endpoint is not configured.");
            return;
        }

        var deploymentName = ConfigurationUtils.GetConfigurationValue("AzureOpenAI:DeploymentName");

        if (string.IsNullOrEmpty(deploymentName))
        {
            Console.WriteLine("Azure OpenAI Deployment Name is not configured.");
            return;
        }

        Console.WriteLine("Deployment Name: " + deploymentName);

        var apiKey = ConfigurationUtils.GetConfigurationValue("AzureOpenAI:ApiKey");

        if (string.IsNullOrEmpty(apiKey))
        {
            Console.WriteLine("Azure OpenAI API Key is not configured.");
            return;
        }

        var openAIClient = new AzureOpenAIClient(
            new Uri(endpointUrl),
            new System.ClientModel.ApiKeyCredential(apiKey)
        );

        var agent = new OpenAIChatAgent(
            chatClient: openAIClient.GetChatClient(deploymentName),
            name: "TrainingAgent",
            systemMessage: "You are a helpful assistant.",
            temperature: 1.0f
        )
            .RegisterMessageConnector()
            .RegisterPrintMessage();

        await agent.SendAsync("Just Generate Code. Can you write a piece of C# code to calculate 100th fibonacci number?");
    }
}