using System;

namespace TrainingAgent
{
    public class Program
    {
        public static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== TrainingAgent Console Menu ===");
                Console.WriteLine("Please choose an option:");
                Console.WriteLine("1. Run SimpleAgentWithAssertions");
                Console.WriteLine("2. Run ConnectWithAzureOpenAI");
                Console.WriteLine("3. Run ConnectWithAzureOpenAIEx");
                Console.WriteLine("4. Run AgentWithMiddleware");
                Console.WriteLine("5. Run AgentWithMiddlewareEx");
                Console.WriteLine("6. Run ProfessorAndStudentAgent");
                Console.WriteLine("7. Run SimpleToolsWithAgents");
                Console.WriteLine("8. Run SimpleToolsWithAgentsNoInvoke");
                Console.WriteLine("9. Run StructuralOutputDemo");
                Console.WriteLine("10. Run MultimodalityModelDemo - Simple Background Image");
                Console.WriteLine("11. Run MultimodalityModelDemo - Visual Chart");
                Console.WriteLine("0. Exit");
                Console.Write("Enter your choice: ");

                var input = Console.ReadLine();

                try
                {
                    switch (input)
                    {
                        case "1":
                            SimpleAgentWithAssertions.RunAsync().GetAwaiter().GetResult();
                            break;
                        case "2":
                            ConnectWithAzureOpenAI.RunAsync().GetAwaiter().GetResult();
                            break;
                        case "3":
                            ConnectWithAzureOpenAIEx.RunAsync().GetAwaiter().GetResult();
                            break;
                        case "4":
                            AgentWithMiddleware.RunAsync().GetAwaiter().GetResult();
                            break;
                        case "5":
                            AgentWithMiddlewareEx.RunAsync().GetAwaiter().GetResult();
                            break;
                        case "6":
                            ProfessorAndStudentAgents.RunAsync().GetAwaiter().GetResult();
                            break;
                        case "7":
                            SimpleToolsWithAgents.RunAsync().GetAwaiter().GetResult();
                            break;
                        case "8":
                            SimpleToolsWithAgentsNoInvoke.RunAsync().GetAwaiter().GetResult();
                            break;
                        case "9":
                            StructuralOutputDemo.RunAsync().GetAwaiter().GetResult();
                            break;
                        case "10":
                            MultimodalityModelDemo.RunAsync1().GetAwaiter().GetResult();
                            break;
                        case "11":
                            MultimodalityModelDemo.RunAsync2().GetAwaiter().GetResult();
                            break;
                        case "0":
                            Console.WriteLine("Exiting...");
                            return;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }

                Console.WriteLine("\nPress any key to return to the menu...");
                Console.ReadKey();
            }
        }
    }
}