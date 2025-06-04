using AutoGen;
using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;
using AutoGen.SemanticKernel;
using AutoGen.SemanticKernel.Extension;
using Google.Apis.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Plugins.Web;

namespace TrainingAgent;

#pragma warning disable SKEXP0050 // Missing XML comment for publicly visible type or member

public class RoundRobinGroupChatDemo
{
    public static async Task<IAgent> CreateSearchAgentAsync()
    {
        var configurationSettings = Configuration.ConfigureAppSettings();
        var openAISettings = new OpenAIOptions();
        var pluginSettings = new PluginOptions();

        configurationSettings.GetSection(OpenAIOptions.OpenAI).Bind(openAISettings);
        configurationSettings.GetSection(PluginOptions.PluginConfig).Bind(pluginSettings);

        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Warning);
            builder.AddConfiguration(configurationSettings);
            builder.AddConsole();
        });

        var kernelBuilder = Kernel.CreateBuilder();

        kernelBuilder.Services.AddSingleton(loggerFactory);
        kernelBuilder.AddChatCompletionService(openAISettings);
        kernelBuilder.AddBraveConnector(pluginSettings, ApiLoggingLevel.ResponseAndRequest);
        kernelBuilder.Plugins.AddFromType<WebSearchEnginePlugin>();

        var kernel = kernelBuilder.Build();
        var systemPrompt = @"You are a helpful assistant that can search the web using Brave Search.
            You will be given a search query, and you should return the top results from the Brave Search engine.
            You put the original search results between ``` Brave and ``` tags.

            For example, if the user asks for the weather, you might respond with:

            ```Brave
            Search query: weather
            Here are the top results for 'weather':
            1. Weather.com - Current weather conditions and forecasts
            2. AccuWeather - Local weather forecasts
            3. BBC Weather - Latest weather updates
            ```
            ";
        var skAgent = new SemanticKernelAgent(
            kernel: kernel,
            name: "BraveSearchPluginInAutoGenDemo",
            systemMessage: systemPrompt
        )
            .RegisterMessageConnector()
            .RegisterPrintMessage();

        return skAgent;
    }

    public static async Task<IAgent> CreateSummarizeAgentAsync()
    {
        var deploymentName = ConfigurationUtils.GetConfigurationValue("AzureOpenAI:DeploymentName");
        var client = LLMConfiguration.GetAzureOpenAIClient();
        var systemPrompt =
            @"You are a helpful assistant that can summarize web search results.
            You will be given a set of search results.
            
            The search results will be provided in a code block with the tag ```Brave.
            For example, if the user asks for the weather, you might receive:

            ```Brave
            Search query: weather
            Here are the top results for 'weather':
            1. Weather.com - Current weather conditions and forecasts
            2. AccuWeather - Local weather forecasts
            3. BBC Weather - Latest weather updates
            ```
            You should summarize the results in a concise manner, focusing on the most relevant information.";

        var agent = new OpenAIChatAgent(
           chatClient: client.GetChatClient(deploymentName),
           name: "ToolsAgent",
           systemMessage: systemPrompt,
           temperature: 1.0f
        )
            .RegisterMessageConnector()
            .RegisterPrintMessage();

        return agent;
    }

    public static async Task RunAsync()
    {
        var userProxyAgent = new UserProxyAgent(
            name: "user",
            humanInputMode: HumanInputMode.ALWAYS)
            .RegisterPrintMessage();

        var searchAgent = await CreateSearchAgentAsync();
        var summarizeAgent = await CreateSummarizeAgentAsync();
        var groupChat = new RoundRobinGroupChat(
            agents: [userProxyAgent, searchAgent, summarizeAgent]);
        var groupChatManager = new GroupChatManager(groupChat);

        var history = await userProxyAgent.InitiateChatAsync(
            receiver: groupChatManager,
            message: "How do you deploy an OpenAI resource in Azure? ",
            maxRound: 10);

        Console.WriteLine("Chat History:");
        Console.WriteLine("========================================");

        foreach (var chatMessage in history)
        {
            Console.WriteLine($"{chatMessage.From}: {chatMessage.GetContent()}");
        }
    }
}