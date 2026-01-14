
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["auth.api/auth.api.csproj", "auth.api/"]
COPY ["auth.core/auth.core.csproj", "auth.core/"]
COPY ["auth.infrastructure/auth.infrastructure.csproj", "auth.infrastructure/"]
COPY ["auth.shared/auth.shared.csproj", "auth.shared/"]

RUN dotnet restore "auth.api/auth.api.csproj"
COPY . .
WORKDIR "/src/auth.api"
RUN dotnet build "auth.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "auth.api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "auth.api.dll"]
