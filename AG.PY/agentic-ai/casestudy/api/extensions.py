from typing import Sequence
from autogen_agentchat.messages import AgentEvent, ChatMessage

from agents import planning_agent

def customer_support_agent_selector_func(messages: Sequence[AgentEvent | ChatMessage]) -> str | None:
    if messages[-1].source != planning_agent.name:
        return planning_agent.name
    return None
