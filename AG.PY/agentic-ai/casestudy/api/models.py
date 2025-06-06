import os

from dotenv import load_dotenv
from autogen_ext.models.openai import AzureOpenAIChatCompletionClient, OpenAIChatCompletionClient

load_dotenv()

azure_model_client = AzureOpenAIChatCompletionClient(
    azure_deployment=os.getenv("deployment_name"),
    model=os.getenv("model_name"),
    api_version=os.getenv("api_version"),
    azure_endpoint=os.getenv("azure_endpoint"),
    api_key=os.getenv("api_key")
)


