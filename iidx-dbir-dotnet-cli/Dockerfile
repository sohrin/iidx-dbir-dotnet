#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0.102-ca-patch-buster-slim AS build
WORKDIR /src
COPY [".",  "iidx-dbir-dotnet-cli/"]
RUN dotnet restore "iidx-dbir-dotnet-cli/iidx-dbir-dotnet-cli.csproj"
WORKDIR "/src/iidx-dbir-dotnet-cli"
RUN dotnet build "iidx-dbir-dotnet-cli.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "iidx-dbir-dotnet-cli.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "iidx-dbir-dotnet-cli.dll"]