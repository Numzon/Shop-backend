#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Shop.WebApi/Shop.WebApi.csproj", "Shop.WebApi/"]
COPY ["Shop.Application/Shop.Application.csproj", "Shop.Application/"]
COPY ["Shop.Core/Shop.Domain.csproj", "Shop.Core/"]
COPY ["Shop.Infrastructure/Shop.Infrastructure.csproj", "Shop.Infrastructure/"]
RUN dotnet restore "Shop.WebApi/Shop.WebApi.csproj"
COPY . .
WORKDIR "/src/Shop.WebApi"
RUN dotnet build "Shop.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Shop.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Shop.WebApi.dll"]