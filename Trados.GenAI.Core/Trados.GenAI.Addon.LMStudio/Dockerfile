# This Dockerfile is used for debugging purposes to test how the App behaves when containerized. Run it under Linux docker.

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base

WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src
COPY ["Trados.GenAI.Addon.LMStudio/Trados.GenAI.Addon.LMStudio.csproj", "Trados.GenAI.Addon.LMStudio/"]
RUN dotnet restore "Trados.GenAI.Addon.LMStudio/Trados.GenAI.Addon.LMStudio.csproj"
COPY . .
WORKDIR "/src/Trados.GenAI.Addon.LMStudio"
RUN dotnet build "Trados.GenAI.Addon.LMStudio.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Trados.GenAI.Addon.LMStudio.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Trados.GenAI.Addon.LMStudio.dll"]