export type OrderStatus = 'Created' | 'Shipped' | 'Delivered' | 'Cancelled';

export const OrderStatusEnum = {
  Created: 'Created' as OrderStatus,
  Shipped: 'Shipped' as OrderStatus,
  Delivered: 'Delivered' as OrderStatus,
  Cancelled: 'Cancelled' as OrderStatus,
};

export interface OrderResponse {
  id: string;
  orderNumber: string;
  description: string;
  status: OrderStatus;
  createdAt: string;
  updatedAt: string;
}

export interface CreateOrderRequest {
  description: string;
}

export interface UpdateOrderStatusRequest {
  newStatus: OrderStatus;
}
