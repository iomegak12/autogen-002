# Use the official .NET 8.0 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy solution file and project files
COPY *.sln ./
COPY TrainingAgent/*.csproj ./TrainingAgent/
COPY TrainingAgentTests/*.csproj ./TrainingAgentTests/

# Restore dependencies
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish TrainingAgent -c Release -o out --no-restore

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app

# Create a non-root user
RUN groupadd -r appuser && useradd -r -g appuser appuser

# Copy the published app
COPY --from=build-env /app/out .

# Change ownership of the app directory
RUN chown -R appuser:appuser /app

# Switch to non-root user
USER appuser

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
  CMD dotnet TrainingAgent.dll --health-check || exit 1

# Entry point
ENTRYPOINT ["dotnet", "TrainingAgent.dll"]
