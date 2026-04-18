import axios from 'axios';
import type {
  CreateOrderRequest,
  OrderResponse,
  UpdateOrderStatusRequest,
} from '../types/Order';

const API_BASE_URL = 'http://localhost:5000/api';

const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

export const orderApi = {
  getAllOrders: async (): Promise<OrderResponse[]> => {
    const response = await apiClient.get<OrderResponse[]>('/orders');
    return response.data;
  },

  getOrderById: async (id: string): Promise<OrderResponse> => {
    const response = await apiClient.get<OrderResponse>(`/orders/${id}`);
    return response.data;
  },

  createOrder: async (request: CreateOrderRequest): Promise<OrderResponse> => {
    const response = await apiClient.post<OrderResponse>('/orders', request);
    return response.data;
  },

  updateOrderStatus: async (
    id: string,
    request: UpdateOrderStatusRequest,
  ): Promise<void> => {
    await apiClient.put(`/orders/${id}/status`, request);
  },

  deleteOrder: async (id: string): Promise<void> => {
    await apiClient.delete(`/orders/${id}`);
  },
};

