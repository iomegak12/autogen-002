using System.Text.Json;
using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;
using FluentAssertions;
using Grpc.Core;
using Json.Schema;
using Json.Schema.Generation;

namespace TrainingAgent;

public static class StructuralOutputDemo
{
    public static async Task RunAsync()
    {
        var deploymentName = ConfigurationUtils.GetConfigurationValue("AzureOpenAI:DeploymentName");
        var client = LLMConfiguration.GetAzureOpenAIClient();

        var schemaBuilder = new JsonSchemaBuilder().FromType<Person>();
        var schema = schemaBuilder.Build();
        var systemPrompt = @"You are a helpful assistant that generates structured output based on the provided schema.";

        var agent = new OpenAIChatAgent(
           chatClient: client.GetChatClient(deploymentName),
           name: "ToolsAgent",
           systemMessage: systemPrompt,
           temperature: 1.0f
        )
            .RegisterMessageConnector()
            .RegisterPrintMessage();

        var prompt = new TextMessage(
            Role.User,
            """
            My name is Ramkumar, I am 50 Years Old, I manage a firm named REDIVAC.
            I live in Chennai, India. My hobbies include reading, writing, and traveling, along with Managing.
            """
        );

        var reply = await agent.GenerateReplyAsync(
            messages: [prompt],
            options: new GenerateReplyOptions
            {
                OutputSchema = schema
            });

        var person = JsonSerializer.Deserialize<Person>(reply.GetContent());

        if (person != null)
        {
            Console.WriteLine($"Name: {person.Name}");
            Console.WriteLine($"Age: {person.Age}");
            Console.WriteLine($"Occupation: {person.Occupation}");
            Console.WriteLine($"City: {person.City}");
            Console.WriteLine($"Address: {person.Address}");
            Console.WriteLine($"Hobbies: {string.Join(", ", person.Hobbies ?? new List<string>())}");
        }
        else
        {
            Console.WriteLine("Failed to parse the structured output.");
        }

        person.Name.Should().Be("Ramkumar");
        person.Age.Should().Be(50);
        person.Occupation.Should().Be("Manager");
        person.City.Should().Be("Chennai");
        person.Hobbies.Should().NotBeNull();
    }
}