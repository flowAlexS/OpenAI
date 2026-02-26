# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 8080

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

# Copy only the project file first, relative to Dockerfile
COPY ./Trados.GenAI.Addon.OpenAI.csproj ./
RUN dotnet restore "Trados.GenAI.Addon.OpenAI.csproj"

# Copy everything else (relative to Dockerfile)
COPY . . 

# Build the project
RUN dotnet build "Trados.GenAI.Addon.OpenAI.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "Trados.GenAI.Addon.OpenAI.csproj" -c Release -o /app/publish

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Trados.GenAI.Addon.OpenAI.dll"]