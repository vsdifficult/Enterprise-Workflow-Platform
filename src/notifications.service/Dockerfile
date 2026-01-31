FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["notifications.api/notifications.api.csproj", "notifications.api/"]
COPY ["notifications.core/notifications.core.csproj", "notifications.core/"]
COPY ["notifications.infrastructure/notifications.infrastructure.csproj", "notifications.infrastructure/"]
COPY ["notifications.shared/notifications.shared.csproj", "notifications.shared/"]

RUN dotnet restore "notifications.api/notifications.api.csproj"
COPY . .
WORKDIR "/src/notifications.api"
RUN dotnet build "notifications.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "notifications.api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "notifications.api.dll"]