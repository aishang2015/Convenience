#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Convience.Applications/Convience.ManagentApi/Convience.ManagentApi.csproj", "Convience.Applications/Convience.ManagentApi/"]
COPY ["Convience.Applications/Convience.Service/Convience.Service.csproj", "Convience.Applications/Convience.Service/"]
COPY ["Convience.Applications/Convience.Model/Convience.Model.csproj", "Convience.Applications/Convience.Model/"]
COPY ["Convience.Applications/Convience.Repository/Convience.Repository.csproj", "Convience.Applications/Convience.Repository/"]
COPY ["Convience.Applications/Convience.Entity/Convience.Entity.csproj", "Convience.Applications/Convience.Entity/"]
COPY ["Convience.FrameWork/Convience.Core/Convience.Core.csproj", "Convience.FrameWork/Convience.Core/"]
COPY ["Convience.FrameWork/Convience.Taskscheduling/Convience.Background/Convience.Background.csproj", "Convience.FrameWork/Convience.Taskscheduling/Convience.Background/"]
COPY ["Convience.FrameWork/Convience.Util/Convience.Util.csproj", "Convience.FrameWork/Convience.Util/"]
COPY ["Convience.FrameWork/Convience.Dependency/Convience.Dependency.csproj", "Convience.FrameWork/Convience.Dependency/"]
COPY ["Convience.FrameWork/Convience.Filestorage/Convience.Filestorage.Filesystem/Convience.Filestorage.Filesystem.csproj", "Convience.FrameWork/Convience.Filestorage/Convience.Filestorage.Filesystem/"]
COPY ["Convience.FrameWork/Convience.Filestorage/Convience.Filestorage.Abstraction/Convience.Filestorage.Abstraction.csproj", "Convience.FrameWork/Convience.Filestorage/Convience.Filestorage.Abstraction/"]
COPY ["Convience.FrameWork/Convience.Swashbuckle/Convience.Swashbuckle.csproj", "Convience.FrameWork/Convience.Swashbuckle/"]
COPY ["Convience.FrameWork/Convience.Jwtauthentication/Convience.Jwtauthentication.csproj", "Convience.FrameWork/Convience.Jwtauthentication/"]
COPY ["Convience.FrameWork/Convience.Caches/Convience.Easycaching/Convience.Easycaching.csproj", "Convience.FrameWork/Convience.Caches/Convience.Easycaching/"]
COPY ["Convience.FrameWork/Convience.Taskscheduling/Convience.Hangfire/Convience.Hangfire.csproj", "Convience.FrameWork/Convience.Taskscheduling/Convience.Hangfire/"]
COPY ["Convience.FrameWork/Convience.Fluentvalidation/Convience.Fluentvalidation.csproj", "Convience.FrameWork/Convience.Fluentvalidation/"]
COPY ["Convience.FrameWork/Convience.Data/Convience.MongoDB/Convience.MongoDB.csproj", "Convience.FrameWork/Convience.Data/Convience.MongoDB/"]
COPY ["Convience.FrameWork/Convience.MessageQueue/Convience.MediatR/Convience.MediatRs.csproj", "Convience.FrameWork/Convience.MessageQueue/Convience.MediatR/"]
COPY ["Convience.FrameWork/Convience.Automapper/Convience.Automapper.csproj", "Convience.FrameWork/Convience.Automapper/"]
COPY ["Convience.FrameWork/Convience.Caches/Convience.Cache/Convience.Cache.csproj", "Convience.FrameWork/Convience.Caches/Convience.Cache/"]
COPY ["Convience.FrameWork/Convience.Data/Convience.EntityFrameWork/Convience.EntityFrameWork.csproj", "Convience.FrameWork/Convience.Data/Convience.EntityFrameWork/"]
COPY ["Convience.FrameWork/Convience.Comunication/Convience.HttpClients/Convience.HttpClients.csproj", "Convience.FrameWork/Convience.Comunication/Convience.HttpClients/"]
RUN dotnet restore "Convience.Applications/Convience.ManagentApi/Convience.ManagentApi.csproj"
COPY . .
WORKDIR "/src/Convience.Applications/Convience.ManagentApi"
RUN dotnet build "Convience.ManagentApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Convience.ManagentApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Convience.ManagentApi.dll"]