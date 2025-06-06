using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;
using FluentAssertions;
using OpenAI.Chat;

namespace TrainingAgent;

public static class ProfessorAndStudentAgents
{
    public static async Task RunAsync()
    {
        var deploymentName = ConfigurationUtils.GetConfigurationValue("AzureOpenAI:DeploymentName");
        var client = LLMConfiguration.GetAzureOpenAIClient();
        var superProfessorSystemMessage =
        @"
            You are a super professor, an expert in mathematics subject. 
            You are very helpful and knowledgeable. 
            You are required to ask questions for students to answer,
            and you will provide feedback on their answers.

            If the answer is correct, you will say 'Correct!'. You stop the conversation by saying [COMPLETE].
            If the answer is incorrect, you will say 'Incorrect!', and ask the student to try again to fix it.            
        ";

        var professorAgent = new OpenAIChatAgent(
                    chatClient: client.GetChatClient(deploymentName),
                    name: "SuperProfessorAgent",
                    systemMessage: superProfessorSystemMessage,
                    temperature: 1.0f
                )
                .RegisterMessageConnector()
                .RegisterMiddleware(
                    async (messages, option, agent, context) =>
                    {
                        var reply = await agent.GenerateReplyAsync(messages, option);

                        if (reply.GetContent()?.ToLower().Contains("complete") == true)
                        {
                            Console.WriteLine("The conversation is complete. Terminating the chat.");

                            return new TextMessage(
                                Role.Assistant,
                                GroupChatExtension.TERMINATE,
                                from: reply.From
                            );
                        }

                        return reply;
                    }
                )
                .RegisterPrintMessage();

        var studentSystemMessage = @"
            You are a student, and you are trying to answer the questions asked by the professor.
            You will try your best to answer the questions, and you will ask for help if you don't know the answer.
            If you don't know the answer, you will say 'I don't know'.";

        var studentAgent = new OpenAIChatAgent(
            chatClient: client.GetChatClient(deploymentName),
            name: "StudentAgent",
            systemMessage: studentSystemMessage,
            temperature: 0.7f
        )
            .RegisterMessageConnector()
            .RegisterPrintMessage();

        var conversation = await studentAgent.InitiateChatAsync(
            receiver: professorAgent,
            message: "Hello Professor, I am ready to learn from you. Please ask me a question.",
            maxRound: 10
        );

        conversation.Count().Should().BeLessThan(10, "the conversation should not exceed 10 rounds");
        conversation.Last().IsGroupChatTerminateMessage().Should().BeTrue("the conversation should end with a terminate message");
    }
}