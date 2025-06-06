# Multi-Agent Chatbot for Customer Support

## Project Overview

The **Multi-Agent AI Chatbot for Customer Support** is a conversational AI system designed to streamline and automate customer interactions. Leveraging multiple specialized agents, the chatbot efficiently manages inquiries related to product information, order placement, order status, and complaint registrations. The system integrates a modern front-end with a robust back-end and utilizes AutoGen's multi-agent framework for intelligent task delegation and execution.

---

## Key Features

- **Product Inquiry:** Instantly provide customers with detailed product information.
- **Order Placement:** Seamlessly guide users through the order placement process.
- **Order Status Tracking:** Allow customers to check the status of their orders in real time.
- **Complaint Registration:** Enable users to register complaints and receive timely responses.
- **Multi-Agent Collaboration:**
  - Dynamic selection of specialized agents based on the inquiry type.
  - Intelligent routing and hand-offs between agents for efficient resolution.

---

## Technology Stack

- **Database:** In-memory lists and dictionaries for managing products, orders, and complaints.
- **Back-End:** FastAPI and Uvicorn for REST API development and hosting.
- **Front-End:** Streamlit for an interactive and user-friendly interface.
- **AI Model:** GPT-4o for natural language understanding and response generation.
- **Multi-Agent Framework:** AutoGen for orchestrating agent collaboration and task execution.

---

## Getting Started

1. **Clone the repository:**
   ```bash
   git clone <repository-url>
   cd agentic-ai
   ```

2. **Set up the environment using [uv](https://github.com/astral-sh/uv):**
   ```bash
   uv venv .venv
   source .venv/bin/activate
   uv pip install -r requirements.txt
   ```

3. **Configure environment variables:**
   - Copy `.env.example` to `.env` and update the values as needed.

4. **Run the application:**
   - **Back-End:**  
     ```bash
     uvicorn src.main:app --reload
     ```
   - **Front-End:**  
     ```bash
     streamlit run src/streamlit_app.py
     ```

---

## Project Structure

```
agentic-ai/
├── data/
├── docs/
├── logs/
├── uploads/
├── src/
│   ├── main.py
│   └── ...
├── .env
├── .gitignore
├── Dockerfile
├── docker-compose.yml
├── LICENSE
├── README.md
└── requirements.txt
```

---

## License

This project is licensed under the MIT License. See the [LICENSE](./LICENSE) file for details.

---

## Acknowledgements

- [FastAPI](https://fastapi.tiangolo.com/)
- [Streamlit](https://streamlit.io/)
- [AutoGen](https://github.com/microsoft/autogen)
- [OpenAI GPT-4o](https://platform.openai.com/docs/models/gpt-4o)