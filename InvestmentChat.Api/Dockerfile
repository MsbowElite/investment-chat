#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["InvestmentChat.Api/InvestmentChat.Api.csproj", "InvestmentChat.Api/"]
RUN dotnet restore "InvestmentChat.Api/InvestmentChat.Api.csproj"
COPY . .
WORKDIR "/src/InvestmentChat.Api"
RUN dotnet build "InvestmentChat.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InvestmentChat.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InvestmentChat.Api.dll"]