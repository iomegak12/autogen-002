from fastapi import FastAPI
from pydantic import BaseModel

from main import main

app = FastAPI()


class QueryRequest(BaseModel):
    query: str
    customer_id: str = "Guest"


@app.post("/chat")
async def chat(request: QueryRequest):
    """
    Endpoint to handle chat queries.
    """
    query = request.query
    customer_id = request.customer_id

    # Call the main function with the query
    result = await main(query)

    # Return the result as a response
    return {"response": result, "customer_id": customer_id}
