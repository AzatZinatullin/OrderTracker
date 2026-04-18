# Order Tracking Application

Event-driven microservices architecture for tracking orders, built with .NET 10, React, SignalR, RabbitMQ, and PostgreSQL.

## Architecture

- **OrderService**: Manages the core order domain using EF Core and PostgreSQL. Publishes domain events to RabbitMQ.
- **NotificationService**: Consumes events from RabbitMQ and pushes real-time updates to connected clients via SignalR.
- **ApiGateway**: Central piece (YARP) that routes frontend traffic to appropriate microservices.
- **Frontend**: React SPA using Vite, TailwindCSS, and Zustand. Real-time updates delivered smoothly via SignalR.
- **Observability**: OpenTelemetry tracing sent to Jaeger.

## Configuration & Tooling

To run this application locally, you'll need Docker.

## Run Locally (Docker)

```bash
docker-compose up --build
```

### URLs

- **Frontend Application**: http://localhost:3000
- **API Gateway**: http://localhost:5000
- **Jaeger Tracing UI**: http://localhost:16686
- **RabbitMQ Management**: http://localhost:15672 (guest / guest)
- **Order Service**: http://localhost:5001/swagger

## Project Details

- `/backend/src/OrderTracker.OrderService` - Order domain and infrastructure
- `/backend/src/OrderTracker.NotificationService` - RabbitMQ consumers and SignalR Hub
- `/backend/src/OrderTracker.ApiGateway` - YARP configuration
- `/backend/src/OrderTracker.Shared` - C# Records / Events 
- `/frontend` - React 18, Vite, Tailwind CSS, TypeScript
