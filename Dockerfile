FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

LABEL author="duru100470"

# Copy main project
COPY src/*.csproj .
RUN dotnet restore

# Copy and publish app and libraries
COPY src/. .

# Run the project
ENTRYPOINT [ "dotnet", "run" ]