# agentic-ai

A Python project template for agentic AI applications.

## Features
- MIT License
- Docker & Docker Compose support
- GitHub Actions CI for Docker image builds
- Environment variable management via `.env`
- Organized project structure: `src/`, `logs/`, `data/`, `uploads/`, `docs/`
- Dependency management with `uv` and `requirements.txt`
- Virtual environment managed by `uv`

## Project Structure
```
agentic-ai/
├── .env
├── .gitignore
├── Dockerfile
├── docker-compose.yml
├── LICENSE
├── README.md
├── requirements.txt
├── logs/
├── data/
├── uploads/
├── docs/
└── src/
    └── main.py
```

## Setup

### 1. Install [uv](https://github.com/astral-sh/uv)
```bash
pip install uv
```

### 2. Create a virtual environment
```bash
uv venv .venv
source .venv/bin/activate
```

### 3. Install dependencies
```bash
uv pip install -r requirements.txt
```

### 4. Run the application
```bash
python src/main.py
```

## Docker

Build and run with Docker Compose:
```bash
docker-compose up --build
```

## Configuration

All configuration and secrets should be placed in the `.env` file.

## License

MIT
