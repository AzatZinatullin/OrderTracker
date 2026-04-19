/**
 * Статусы заказа
 */
export type OrderStatus = 'Created' | 'Shipped' | 'Delivered' | 'Cancelled';

/**
 * Enum статусов заказа
 */
export const OrderStatusEnum = {
  Created: 'Created' as OrderStatus,
  Shipped: 'Shipped' as OrderStatus,
  Delivered: 'Delivered' as OrderStatus,
  Cancelled: 'Cancelled' as OrderStatus,
};

/**
 * Ответ при получении заказа
 */
export interface OrderResponse {
  id: string;
  orderNumber: string;
  description: string;
  status: OrderStatus;
  createdAt: string;
  updatedAt: string;
}

/**
 * Запрос на создание заказа
 */
export interface CreateOrderRequest {
  description: string;
}

/**
 * Запрос на обновление статуса заказа
 */
export interface UpdateOrderStatusRequest {
  newStatus: OrderStatus;
}
