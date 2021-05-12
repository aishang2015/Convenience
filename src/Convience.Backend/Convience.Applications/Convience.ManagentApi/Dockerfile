#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 44

RUN apt-get update
RUN apt-get install libgdiplus -y && ln -s libgdiplus.so gdiplus.dll

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

WORKDIR /src
COPY ["Convience.Applications/Convience.ManagentApi/Convience.ManagentApi.csproj", "Convience.Applications/Convience.ManagentApi/"]
COPY ["Convience.Applications/Convience.Service/Convience.Service.csproj", "Convience.Applications/Convience.Service/"]
COPY ["Convience.Applications/Convience.Caching/Convience.Caching.csproj", "Convience.Applications/Convience.Caching/"]
COPY ["Convience.Applications/Convience.Model/Convience.Model.csproj", "Convience.Applications/Convience.Model/"]
COPY ["Convience.Applications/Convience.Entity/Convience.Entity.csproj", "Convience.Applications/Convience.Entity/"]
COPY ["Convience.FrameWork/Convience.Core/Convience.Core.csproj", "Convience.FrameWork/Convience.Core/"]
COPY ["Convience.FrameWork/Convience.Taskscheduling/Convience.Background/Convience.Background.csproj", "Convience.FrameWork/Convience.Taskscheduling/Convience.Background/"]
COPY ["Convience.FrameWork/Convience.Util/Convience.Util.csproj", "Convience.FrameWork/Convience.Util/"]
COPY ["Convience.FrameWork/Convience.Dependency/Convience.Dependency.csproj", "Convience.FrameWork/Convience.Dependency/"]
COPY ["Convience.FrameWork/Convience.Filestorage/Convience.Filestorage.MongoDB/Convience.Filestorage.MongoDB.csproj", "Convience.FrameWork/Convience.Filestorage/Convience.Filestorage.MongoDB/"]
COPY ["Convience.FrameWork/Convience.Filestorage/Convience.Filestorage.Abstraction/Convience.Filestorage.Abstraction.csproj", "Convience.FrameWork/Convience.Filestorage/Convience.Filestorage.Abstraction/"]
COPY ["Convience.FrameWork/Convience.Data/Convience.MongoDB/Convience.MongoDB.csproj", "Convience.FrameWork/Convience.Data/Convience.MongoDB/"]
COPY ["Convience.FrameWork/Convience.Filestorage/Convience.Filestorage.Filesystem/Convience.Filestorage.Filesystem.csproj", "Convience.FrameWork/Convience.Filestorage/Convience.Filestorage.Filesystem/"]
COPY ["Convience.FrameWork/Convience.Taskscheduling/Convience.Quartz/Convience.Quartz.csproj", "Convience.FrameWork/Convience.Taskscheduling/Convience.Quartz/"]
COPY ["Convience.FrameWork/Convience.Jwtauthentication/Convience.JwtAuthentication.csproj", "Convience.FrameWork/Convience.Jwtauthentication/"]
COPY ["Convience.FrameWork/Convience.Taskscheduling/Convience.Hangfire/Convience.Hangfire.csproj", "Convience.FrameWork/Convience.Taskscheduling/Convience.Hangfire/"]
COPY ["Convience.FrameWork/Convience.Comunication/Convience.SignalRs/Convience.SignalRs.csproj", "Convience.FrameWork/Convience.Comunication/Convience.SignalRs/"]
COPY ["Convience.FrameWork/Convience.Fluentvalidation/Convience.Fluentvalidation.csproj", "Convience.FrameWork/Convience.Fluentvalidation/"]
COPY ["Convience.FrameWork/Convience.Comunication/Convience.WebSocket/Convience.WebSockets.csproj", "Convience.FrameWork/Convience.Comunication/Convience.WebSocket/"]
COPY ["Convience.FrameWork/Convience.Data/Convience.EntityFrameWork/Convience.EntityFrameWork.csproj", "Convience.FrameWork/Convience.Data/Convience.EntityFrameWork/"]
COPY ["Convience.FrameWork/Convience.Comunication/Convience.Mail/Convience.Mail.csproj", "Convience.FrameWork/Convience.Comunication/Convience.Mail/"]
COPY ["Convience.FrameWork/Convience.Injection/Convience.Injection.csproj", "Convience.FrameWork/Convience.Injection/"]
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