FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DiplomaDictionary/DiplomaDictionary.csproj", "DiplomaDictionary/"]
COPY ["DiplomaDictionary.Data/DiplomaDictionary.Data.csproj", "DiplomaDictionary.Data/"]
COPY ["DiplomaDictionary.Domain/DiplomaDictionary.Domain.csproj", "DiplomaDictionary.Domain/"]
RUN dotnet restore "DiplomaDictionary/DiplomaDictionary.csproj"
COPY . .
WORKDIR "/src/DiplomaDictionary"
RUN dotnet build "./DiplomaDictionary.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DiplomaDictionary.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DiplomaDictionary.dll"]
