FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/OrderTracker.ApiGateway/OrderTracker.ApiGateway.csproj", "src/OrderTracker.ApiGateway/"]
RUN dotnet restore "src/OrderTracker.ApiGateway/OrderTracker.ApiGateway.csproj"
COPY . .
WORKDIR "/src/src/OrderTracker.ApiGateway"
RUN dotnet build "OrderTracker.ApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderTracker.ApiGateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderTracker.ApiGateway.dll"]
