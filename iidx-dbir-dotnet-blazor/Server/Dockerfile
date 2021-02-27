#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["iidx-dbir-dotnet-blazor/Server/iidx-dbir-dotnet-blazor.Server.csproj", "iidx-dbir-dotnet-blazor/Server/"]
COPY ["iidx-dbir-dotnet-lib/iidx-dbir-dotnet-lib.csproj", "iidx-dbir-dotnet-lib/"]
COPY ["iidx-dbir-dotnet-blazor/Client/iidx-dbir-dotnet-blazor.Client.csproj", "iidx-dbir-dotnet-blazor/Client/"]
RUN dotnet restore "iidx-dbir-dotnet-blazor/Server/iidx-dbir-dotnet-blazor.Server.csproj"
COPY . .
WORKDIR "/src/iidx-dbir-dotnet-blazor/Server"
RUN dotnet build "iidx-dbir-dotnet-blazor.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "iidx-dbir-dotnet-blazor.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS http://*:5000

ENV DOCKERIZE_VERSION v0.6.1
RUN apt-get update && apt-get install -y wget \
 && wget https://github.com/jwilder/dockerize/releases/download/$DOCKERIZE_VERSION/dockerize-linux-amd64-$DOCKERIZE_VERSION.tar.gz \
 && tar -C /usr/local/bin -xzvf dockerize-linux-amd64-$DOCKERIZE_VERSION.tar.gz \
 && rm dockerize-linux-amd64-$DOCKERIZE_VERSION.tar.gz

ENTRYPOINT ["dotnet", "iidx-dbir-dotnet-blazor.Server.dll"]