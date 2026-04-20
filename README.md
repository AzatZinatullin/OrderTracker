# Order Tracking Application

Событийно-ориентированная микросервисная архитектура для отслеживания заказов, построенная на .NET 10, React, SignalR, RabbitMQ и PostgreSQL.

## Архитектура

- **OrderService**: Управляет основным доменом заказов с использованием EF Core и PostgreSQL. Публикует доменные события в RabbitMQ.
- **NotificationService**: Обрабатывает события из RabbitMQ и отправляет обновления в реальном времени подключенным клиентам через SignalR.
- **ApiGateway**: Центральный узел (YARP), который маршрутизирует трафик фронтенда в соответствующие микросервисы.
- **Frontend**: React SPA с использованием Vite, TailwindCSS и Zustand. Обновления в реальном времени доставляются через SignalR.
- **Observability**: Трассировка OpenTelemetry отправляется в Jaeger.
- **Clean Architecture**: Разделение на слои (Domain, Application, Infrastructure, API) для обеспечения тестируемости и независимости от внешних фреймворков.
- **Transactional Outbox (via MassTransit)**: Гарантированная доставка событий об изменении статуса заказа в RabbitMQ.
- **Real-time Updates**: Использование SignalR в NotificationService для мгновенного уведомления фронтенда об изменениях без перезагрузки страницы.
- **Validation & Error Handling**: Централизованная валидация через FluentValidation и глобальная обработка исключений (DomainException, NotFoundException).

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

# Потенциал развития и масштабируемости
Проект является гибким фундаментом, готовым к следующим улучшениям:
- Расширение экосистемы: Архитектура позволяет бесшовно внедрять новые микросервисы (например, Сервис доставки, Сервис управления пользователями, Складской учет).
- Безопасность: Интеграция систем аутентификации и авторизации (IdentityServer, Keycloak, JWT) на уровне API Gateway и отдельных сервисов.
- Бизнес-логика: Возможность усложнения доменных моделей и внедрения паттернов (например, Saga для распределенных транзакций или CQRS).
- Отказоустойчивость: Дальнейшая настройка политик повторов (Retry) и автоматизированного тестирования (Integration & Load testing).