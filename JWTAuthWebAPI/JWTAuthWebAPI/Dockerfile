# Dockerfile for JWTAuthWebAPI

# Use the official ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the official .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy the solution and restore dependencies
COPY JWTAuthWebAPI.sln ./
COPY JWTAuthWebAPI/JWTAuthWebAPI.csproj JWTAuthWebAPI/
RUN dotnet restore JWTAuthWebAPI/JWTAuthWebAPI.csproj

# Copy the rest of the application code and build it
COPY . .
WORKDIR /src/JWTAuthWebAPI
RUN dotnet build JWTAuthWebAPI.csproj -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish JWTAuthWebAPI.csproj -c Release -o /app/publish

# Use the runtime image for the final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JWTAuthWebAPI.dll"]
