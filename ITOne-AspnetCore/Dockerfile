#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
RUN apt-get update -y
RUN apt-get install -y librdkafka-dev
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ITOne-AspnetCore.csproj", "ITOne-AspnetCore/"]
COPY ["Lazarus.Common/Lazarus.Common.csproj", "Lazarus.Common/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "ITOne-AspnetCore/ITOne-AspnetCore.csproj"
COPY . .
WORKDIR "/src/ITOne-AspnetCore"
RUN dotnet build "ITOne-AspnetCore.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ITOne-AspnetCore.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nexus.Api.User.dll"]