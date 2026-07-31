#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FaceWebServer/DeviceProtocolServer.csproj", "FaceWebServer/"]
COPY ["FaceAPI.DB/FaceWebServer.DB.csproj", "FaceAPI.DB/"]
COPY ["FaceWebServer.DTO/FaceWebServer.DTO.csproj", "FaceWebServer.DTO/"]
COPY ["FaceWebServer.Utility/FaceWebServer.Utility.csproj", "FaceWebServer.Utility/"]
COPY ["FaceWebServer.Language/FaceWebServer.Language.csproj", "FaceWebServer.Language/"]
COPY ["FaceWebServer.Interface/FaceWebServer.Interface.csproj", "FaceWebServer.Interface/"]
COPY ["FaceWebServer.Service/FaceWebServer.Service.csproj", "FaceWebServer.Service/"]
RUN dotnet restore "./FaceWebServer/DeviceProtocolServer.csproj"
COPY . .
WORKDIR "/src/FaceWebServer"
RUN dotnet build "./DeviceProtocolServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DeviceProtocolServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DeviceProtocolServer.dll"]