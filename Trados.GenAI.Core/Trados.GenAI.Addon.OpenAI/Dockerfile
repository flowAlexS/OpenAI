# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 8080

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

# Copy the entire repo so that all project references are available
COPY . .

# Restore dependencies for Addon.OpenAI project (or solution)
RUN dotnet restore "Trados.GenAI.Core/Trados.GenAI.Addon.OpenAI/Trados.GenAI.Addon.OpenAI.csproj"

# Build the Addon.OpenAI project
RUN dotnet build "Trados.GenAI.Core/Trados.GenAI.Addon.OpenAI/Trados.GenAI.Addon.OpenAI.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "Trados.GenAI.Core/Trados.GenAI.Addon.OpenAI/Trados.GenAI.Addon.OpenAI.csproj" -c Release -o /app/publish

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Trados.GenAI.Addon.OpenAI.dll"]