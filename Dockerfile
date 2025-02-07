# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy all files and build
COPY . ./
RUN dotnet build -c Release -o /app/build

# Publish Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS publish
WORKDIR /app
COPY --from=build /app .  
RUN dotnet publish DrinkConnect.csproj -c Release -o /app/publish

# Run Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/publish .  
EXPOSE 5037
ENTRYPOINT [ "dotnet", "DrinkConnect.dll" ]
