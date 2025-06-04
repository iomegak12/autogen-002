# TrainingAgent

A C# .NET 8.0 console application designed for agent-based development purposes. This project provides a solid foundation for building intelligent agents with comprehensive testing, containerization, and CI/CD capabilities.

## 🚀 Features

- **Modern .NET 8.0**: Built on the latest .NET 8.0 SDK and runtime
- **Console Application**: Command-line interface for agent interactions
- **Comprehensive Testing**: xUnit test framework with dedicated test project
- **Containerization**: Docker support with multi-stage builds
- **CI/CD Ready**: GitHub Actions workflows for automated building and deployment
- **Configuration Management**: Centralized settings in `appsettings.json`
- **Production Ready**: Includes logging, error handling, and best practices

## 📋 Prerequisites

- .NET 8.0 SDK
- Docker (optional, for containerization)
- Visual Studio 2022 or Visual Studio Code (recommended)

## 🛠️ Installation

1. **Clone the repository**
   ```bash
   git clone <your-repository-url>
   cd firstagent
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the solution**
   ```bash
   dotnet build
   ```

## 🎯 Usage

### Running the Application

```bash
# Run from solution root
dotnet run --project TrainingAgent

# Or from the project directory
cd TrainingAgent
dotnet run
```

### Configuration

The application uses `appsettings.json` for configuration. Key settings include:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "Agent": {
    "Name": "TrainingAgent",
    "Version": "1.0.0",
    "EnableDebugMode": false
  }
}
```

### Environment-Specific Configuration

- `appsettings.json` - Base configuration
- `appsettings.Development.json` - Development overrides
- `appsettings.Production.json` - Production overrides

## 🧪 Testing

### Running Tests

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run tests in specific project
dotnet test TrainingAgentTests
```

### Test Structure

The test project (`TrainingAgentTests`) uses xUnit and follows these conventions:
- Test classes end with `Tests`
- Test methods use descriptive names
- Follows AAA pattern (Arrange, Act, Assert)

## 🐳 Docker

### Building Docker Image

```bash
# Build the image
docker build -t trainingagent:latest .

# Run the container
docker run --rm trainingagent:latest
```

### Using Docker Compose

```bash
# Start services
docker-compose up

# Build and start
docker-compose up --build

# Run in background
docker-compose up -d
```

## 🔄 CI/CD

The project includes GitHub Actions workflows:

### Build and Test Workflow
- Triggers on push and pull requests
- Runs on multiple OS (Windows, Linux, macOS)
- Executes tests and generates coverage reports

### Docker Build and Push Workflow
- Builds Docker images
- Pushes to container registry
- Tags with commit SHA and 'latest'

## 📁 Project Structure

```
firstagent/
├── .github/
│   └── workflows/
│       ├── build-and-test.yml
│       └── docker-build-push.yml
├── TrainingAgent/
│   ├── Program.cs
│   ├── TrainingAgent.csproj
│   └── appsettings.json
├── TrainingAgentTests/
│   ├── TrainingAgentTests.csproj
│   └── UnitTest1.cs
├── .gitignore
├── docker-compose.yml
├── Dockerfile
├── LICENSE
├── README.md
└── TrainingAgent.sln
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📊 Development Guidelines

### Code Style
- Follow Microsoft C# coding conventions
- Use meaningful variable and method names
- Add XML documentation for public APIs
- Keep methods focused and single-purpose

### Testing Guidelines
- Maintain high test coverage (>80%)
- Write unit tests for all public methods
- Use meaningful test names that describe the scenario
- Mock external dependencies

### Git Workflow
- Use descriptive commit messages
- Keep commits atomic and focused
- Rebase feature branches before merging
- Tag releases with semantic versioning

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🆘 Support

If you encounter any issues or have questions:

1. Check the [Issues](../../issues) page
2. Create a new issue with detailed information
3. Include system information and steps to reproduce

## 🗺️ Roadmap

- [ ] Add configuration validation
- [ ] Implement health checks
- [ ] Add metrics and monitoring
- [ ] Expand agent capabilities
- [ ] Add integration tests
- [ ] Performance optimization

## 🏷️ Versioning

We use [SemVer](http://semver.org/) for versioning. For available versions, see the [tags on this repository](../../tags).

---

**Happy Coding!** 🎉
