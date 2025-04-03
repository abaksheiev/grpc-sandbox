# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ["GrpcSandbox.Services/GrpcSandbox.Services.csproj", "./GrpcSandbox.Services/"]
RUN dotnet restore "GrpcSandbox.Services/GrpcSandbox.Services.csproj"

COPY . .

EXPOSE 5001

# This stage is used to publish the service project to be copied to the final stage
RUN dotnet publish "GrpcSandbox.Services/GrpcSandbox.Services.csproj" -c Release  -o /app/out

# Set environment variable for Kestrel to listen on all interfaces
ENV DOTNET_URLS=http://0.0.0.0:5001

ENTRYPOINT ["dotnet", "/app/out/GrpcSandbox.Services.dll"]