FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
# Copy only project files first so the restore layer is cached
COPY gis.API/gis.API.csproj gis.API/
COPY Gis.Application/Gis.Application.csproj Gis.Application/
COPY Gis.Infrastructure/Gis.Infrastructure.csproj Gis.Infrastructure/
COPY gis.Domain/gis.Domain.csproj gis.Domain/
RUN dotnet restore gis.API/gis.API.csproj
COPY  . .

RUN dotnet publish gis.API/gis.API.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "gis.API.dll"]