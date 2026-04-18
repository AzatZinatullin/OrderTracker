FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/OrderTracker.NotificationService/OrderTracker.NotificationService.csproj", "src/OrderTracker.NotificationService/"]
COPY ["src/OrderTracker.Shared/OrderTracker.Shared.csproj", "src/OrderTracker.Shared/"]
RUN dotnet restore "src/OrderTracker.NotificationService/OrderTracker.NotificationService.csproj"
COPY . .
WORKDIR "/src/src/OrderTracker.NotificationService"
RUN dotnet build "OrderTracker.NotificationService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderTracker.NotificationService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderTracker.NotificationService.dll"]
