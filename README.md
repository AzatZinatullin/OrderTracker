# Order Tracking Application

Событийно-ориентированная микросервисная архитектура для отслеживания заказов, построенная на .NET 10, React, SignalR, RabbitMQ и PostgreSQL.

## Архитектура

- **OrderService**: Управляет основным доменом заказов с использованием EF Core и PostgreSQL. Публикует доменные события в RabbitMQ.
- **NotificationService**: Обрабатывает события из RabbitMQ и отправляет обновления в реальном времени подключенным клиентам через SignalR.
- **ApiGateway**: Центральный узел (YARP), который маршрутизирует трафик фронтенда в соответствующие микросервисы.
- **Frontend**: React SPA с использованием Vite, TailwindCSS и Zustand. Обновления в реальном времени доставляются через SignalR.
- **Observability**: Трассировка OpenTelemetry отправляется в Jaeger.

## Конфигурация и инструменты

Для запуска приложения локально вам понадобится Docker.

## Запуск локально (Docker)

```bash
docker-compose up --build
```

### URLs

- **Frontend Application**: http://localhost:3000
- **Jaeger Tracing UI**: http://localhost:16686
- **RabbitMQ Management**: http://localhost:15672
- **Order Service**: http://localhost:5001/swagger

## Структура проекта

- `/frontend` - React 18, Vite, Tailwind CSS, TypeScript
- `/backend/src/OrderTracker.OrderService` - Домен и инфраструктура заказов
- `/backend/src/OrderTracker.NotificationService` - Обработчики RabbitMQ и SignalR Hub
- `/backend/src/OrderTracker.ApiGateway` - Конфигурация YARP
- `/backend/src/OrderTracker.Shared` - Общие сущности (используются в сервисах, в тестах: Events / Exceptions / Constants и тд.)
- `/backend/tests/OrderTracker.OrderService.Tests` - Тесты для OrderService
- `/backend/tests/OrderTracker.NotificationService.Tests` - Тесты для NotificationService
