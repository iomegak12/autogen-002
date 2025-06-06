import asyncio
from autogen_agentchat.ui import Console
from teams import team


async def main(query):
    response = await team.run(task=query)
    result = response.messages[-1].content

    # for message in response.messages:
    #     print(f"***** {message.content} *****")

    # print(f"Final response: {result}")

    if result.endswith("TERMINATE"):
        result = result.removesuffix("TERMINATE").strip()

    return result

if __name__ == "__main__":

    query = "I want to order 2 units of Dell XPS 15 laptop."
    result = asyncio.run(main(query))

    print(result)  # Example usage, replace with actual query
