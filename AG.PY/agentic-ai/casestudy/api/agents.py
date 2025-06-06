from autogen_agentchat.agents import AssistantAgent

from agent_tools import product_inquiry_tool, \
    order_placement_tool, order_status_tool, complaint_registration_tool

from models import azure_model_client as model_client

planning_agent_prompt = """
You are a planning agent. 
Your task is to create a detailed plan to accomplish a given task.
Your job is to identify customer rqeuests and delgates them to the appropriate agents.

Available Agents:
- ProductInquiryAgent: Handles inquiries about product information.
- OrderPlacementAgent: Manages order placements and stock checks.
- OrderStatusAgent: Provides order status updates.
- ComplaintRegistrationAgent: Registers and manages customer complaints.
- ResponseAgent: Responsible for responding to the user with the final response.

Once a task is completed by another agent, delegate the response to the **ResponseAgent**
to format and send the final response to the user.
"""

planning_agent = AssistantAgent(
    name="PlanningAgent",
    description="An agent that plans the steps to complete a task.",
    model_client=model_client,
    system_message=planning_agent_prompt,
)

product_inquiry_agent = AssistantAgent(
    name="ProductInquiryAgent",
    description="An agent that handles inquiries about product information.",
    model_client=model_client,
    tools=[product_inquiry_tool],
    system_message="""You are a product inquiry agent. 
    Your task is to provide detailed information about products.""",
)

order_placement_agent = AssistantAgent(
    name="OrderPlacementAgent",
    description="An agent that manages order placements and stock checks.",
    model_client=model_client,
    tools=[order_placement_tool],
    system_message="""You are an order placement agent. 
    Your task is to place orders and check product stock.""",
)

order_status_agent = AssistantAgent(
    name="OrderStatusAgent",
    description="An agent that provides order status updates.",
    model_client=model_client,
    tools=[order_status_tool],
    system_message="""You are an order status agent. 
    Your task is to provide updates on order statuses.""",
)

complaint_registration_agent = AssistantAgent(
    name="ComplaintRegistrationAgent",
    description="An agent that registers and manages customer complaints.",
    model_client=model_client,
    tools=[complaint_registration_tool],
    system_message="""You are a complaint registration agent. 
    Your task is to register and manage customer complaints.""",
)

response_agent = AssistantAgent(
    name="ResponseAgent",
    description="An agent that formats and sends the final response to the user.",
    model_client=model_client,
    system_message="""You are a response agent. 
    Your task is to format and send the final response to the user.
    Always end the conversation with 'TERMINATE'.
    
    Example:
    - If an order is placed successfully, confirm it and include the order ID:
    - If a complaint is registered, confirm it and include the complaint ID:
    - If an order status is requested, summarize the response.
    - If a product inquiry is made, summarize the product details.
    
    Always end your response with 'TERMINATE' to indicate the conversation is complete.
    """,
)
