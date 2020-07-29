#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Eggs.Core.Gateway/Eggs.Core.Gateway.csproj", "Eggs.Core.Gateway/"]
RUN dotnet restore "Eggs.Core.Gateway/Eggs.Core.Gateway.csproj"
COPY . .
WORKDIR "/src/Eggs.Core.Gateway"
RUN dotnet build "Eggs.Core.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Eggs.Core.Gateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Eggs.Core.Gateway.dll"]