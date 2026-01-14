
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["users.api/users.api.csproj", "users.api/"]
COPY ["users.core/users.core.csproj", "users.core/"]
COPY ["users.infrastructure/users.infrastructure.csproj", "users.infrastructure/"]
COPY ["users.shared/users.shared.csproj", "users.shared/"]

RUN dotnet restore "users.api/users.api.csproj"
COPY . .
WORKDIR "/src/users.api"
RUN dotnet build "users.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "users.api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "users.api.dll"]
