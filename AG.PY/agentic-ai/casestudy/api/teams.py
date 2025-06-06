from autogen_agentchat.conditions import TextMentionTermination, MaxMessageTermination
from autogen_agentchat.teams import SelectorGroupChat
from autogen_core import CancellationToken

from agents import planning_agent, \
    product_inquiry_agent, order_placement_agent, \
    order_status_agent, complaint_registration_agent, response_agent
from extensions import customer_support_agent_selector_func
from models import azure_model_client as model_client

text_termination = TextMentionTermination("TERMINATE")
max_messages_termination = MaxMessageTermination(10)
termination = text_termination | max_messages_termination


team = SelectorGroupChat(
    [
        planning_agent,
        product_inquiry_agent,
        order_placement_agent,
        order_status_agent,
        complaint_registration_agent,
        response_agent
    ],
    model_client=model_client,
    termination_condition=termination,
    allow_repeated_speaker=True,
    selector_func=customer_support_agent_selector_func,
)
