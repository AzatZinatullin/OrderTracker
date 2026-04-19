import { useEffect, useState } from 'react';
import * as signalR from '@microsoft/signalr';
import { useOrderStore } from '../store/OrderStore';
import toast from 'react-hot-toast';

/**
 * URL хаба SignalR (можно вынести в .env)
 */
const HUB_URL = 'http://localhost:5000/hub/order-tracking';

/**
 * Хук для работы с SignalR
 * @param orderIdScope - ID заказа для подписки на обновления
 * @param enableToasts - включить/выключить тосты
 */
export function useSignalR(orderIdScope?: string, enableToasts: boolean = true) {
  const [connection, setConnection] = useState<signalR.HubConnection | null>(
    null,
  );

  const {
    handleOrderCreated,
    handleOrderStatusUpdated,
    handleOrderDeleted,
  } = useOrderStore();

  /**
   * Создание соединения с SignalR
   */
  useEffect(() => {
    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl(HUB_URL)
      .withAutomaticReconnect()
      .build();

    setConnection(newConnection);
  }, []);

  /**
   * Обработка событий SignalR
   */
  useEffect(() => {
    if (!connection) return;

    connection
      .start()
      .then(() => {
        console.log('SignalR Connected.');

        if (orderIdScope) {
          connection.invoke('JoinOrderGroup', orderIdScope);
        }

        /**
         * Обработчик создания заказа
         */
        connection.on('OrderCreated', (event) => {
          console.log('SignalR: OrderCreated received', event);
          handleOrderCreated({
            id: event.orderId,
            orderNumber: event.orderNumber,
            description: event.description,
            status: event.status,
            createdAt: event.createdAt,
            updatedAt: event.createdAt,
          });
          if (enableToasts) {
            toast.success(`Новый заказ: ${event.orderNumber}`);
          }
        });

        /**
         * Обработчик обновления статуса заказа
         */
        connection.on('OrderStatusUpdatedAll', (event) => {
          console.log('SignalR: OrderStatusUpdatedAll received', event);
          handleOrderStatusUpdated({
            orderId: event.orderId,
            newStatus: event.newStatus,
            updatedAt: event.updatedAt,
          });

          if (enableToasts) {
            toast(`Заказ ${event.orderNumber} изменил статус на ${event.newStatus}`, {
              icon: '🔄',
            });
          }
        });

        /**
         * Обработчик обновления статуса заказа
         */
        connection.on('OrderStatusUpdated', (event) => {
          console.log('SignalR: OrderStatusUpdated (Group) received', event);
          handleOrderStatusUpdated({
            orderId: event.orderId,
            newStatus: event.newStatus,
            updatedAt: event.updatedAt,
          });
        });

        /**
         * Обработчик удаления заказа
         */
        connection.on('OrderDeleted', (event) => {
          console.log('SignalR: OrderDeleted received', event);
          handleOrderDeleted(event.orderId);
          if (enableToasts) {
            toast.error(`Заказ ${event.orderNumber} был удален`, {
              icon: '🗑️',
            });
          }
        });
      })
      .catch((err) => console.error('SignalR Connection Error: ', err));

    return () => {
      if (connection.state === signalR.HubConnectionState.Connected) {
        if (orderIdScope) {
          connection.invoke('LeaveOrderGroup', orderIdScope);
        }
        connection.stop();
      }
    };
  }, [
    connection,
    orderIdScope,
    handleOrderCreated,
    handleOrderStatusUpdated,
    enableToasts,
  ]);

  return connection;
}
