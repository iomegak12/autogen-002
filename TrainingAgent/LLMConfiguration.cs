using Azure.AI.OpenAI;

namespace TrainingAgent;

public static class LLMConfiguration
{
    public static string EndpointUrl =>
        ConfigurationUtils.GetConfigurationValue("AzureOpenAI:Endpoint") ??
        throw new InvalidOperationException("Azure OpenAI Endpoint is not configured.");

    public static string DeploymentName =>
        ConfigurationUtils.GetConfigurationValue("AzureOpenAI:DeploymentName") ??
        throw new InvalidOperationException("Azure OpenAI Deployment Name is not configured.");

    public static string ApiKey =>
        ConfigurationUtils.GetConfigurationValue("AzureOpenAI:ApiKey") ??
        throw new InvalidOperationException("Azure OpenAI API Key is not configured.");

    public static AzureOpenAIClient GetAzureOpenAIClient()
    {
        return new AzureOpenAIClient(
            new Uri(EndpointUrl),
            new System.ClientModel.ApiKeyCredential(ApiKey)
        );
    }
}