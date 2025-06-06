using AutoGen.Core;
using AutoGen.SemanticKernel;
using AutoGen.SemanticKernel.Extension;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Plugins.Web;

namespace TrainingAgent;

#pragma warning disable SKEXP0050 // Missing XML comment for publicly visible type or member

public static class BraveSearchPluginInAutoGenDemo
{
    public static async Task RunAsync()
    {
        var configurationSettings = Configuration.ConfigureAppSettings();
        var openAISettings = new OpenAIOptions();
        var pluginSettings = new PluginOptions();

        configurationSettings.GetSection(OpenAIOptions.OpenAI).Bind(openAISettings);
        configurationSettings.GetSection(PluginOptions.PluginConfig).Bind(pluginSettings);

        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Warning);
            builder.AddConfiguration(configurationSettings);
            builder.AddConsole();
        });

        var kernelBuilder = Kernel.CreateBuilder();

        kernelBuilder.Services.AddSingleton(loggerFactory);
        kernelBuilder.AddChatCompletionService(openAISettings);
        kernelBuilder.AddBraveConnector(pluginSettings, ApiLoggingLevel.ResponseAndRequest);
        kernelBuilder.Plugins.AddFromType<WebSearchEnginePlugin>();

        var kernel = kernelBuilder.Build();

        var skAgent = new SemanticKernelAgent(
            kernel: kernel,
            name: "BraveSearchPluginInAutoGenDemo",
            systemMessage: "You are a helpful assistant that can search the web using Brave Search."
        )
            .RegisterMessageConnector()
            .RegisterPrintMessage();

        var reply = await skAgent.SendAsync("Tell me about the latest news in Solar Energy in 2025.");

        Console.WriteLine($"Reply: {reply.GetContent()}");
    }
}