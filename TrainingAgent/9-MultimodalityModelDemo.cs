using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;
using FluentAssertions;

namespace TrainingAgent;

public static class MultimodalityModelDemo
{
    public static async Task RunAsync(string imageFileName)
    {
        var deploymentName = ConfigurationUtils.GetConfigurationValue("AzureOpenAI:DeploymentName");
        var client = LLMConfiguration.GetAzureOpenAIClient();
        var systemPrompt =
            @"You are a helpful assistant that can process multimodal inputs, including text and images.
              You will receive a text prompt and an image, and you should generate a response based on both.";

        var agent = new OpenAIChatAgent(
           chatClient: client.GetChatClient(deploymentName),
           name: "ToolsAgent",
           systemMessage: systemPrompt,
           temperature: 1.0f
        )
            .RegisterMessageConnector()
            .RegisterPrintMessage();

        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "resources", "images", imageFileName);

        if (!File.Exists(imagePath))
        {
            Console.WriteLine($"Image file '{imageFileName}' not found in the resources/images directory.");

            return;
        }

        var imageBytes = await File.ReadAllBytesAsync(imagePath);
        var imageMessage = new ImageMessage(
            Role.User,
            BinaryData.FromBytes(imageBytes, "image/png"));
        var textPrompt = new TextMessage(
            Role.User,
            "What can you tell me about this image?"
        );
        var multimodalMessage = new MultiModalMessage(
            Role.User,
            [textPrompt, imageMessage]
        );

        var reply = await agent.SendAsync(multimodalMessage);

        reply.Should().NotBeNull();
        reply.GetContent().Should().NotBeNullOrEmpty();
        reply.Should().BeOfType<TextMessage>();

        Console.WriteLine("Response from the multimodal model Processed!");
        // Console.WriteLine(reply.GetContent());
    }

    public static async Task RunAsync1()
    {
        await RunAsync("background.png");
    }

    public static async Task RunAsync2()
    {
        await RunAsync("USMortgageRate.png");
    }
}