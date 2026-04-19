import axios from 'axios';
import type {
  CreateOrderRequest,
  OrderResponse,
  UpdateOrderStatusRequest,
} from '../types/Order';

/**
 * URL API (можно вынести в .env)
 */
const API_BASE_URL = 'http://localhost:5000/api';

/**
 * Для данного тестового задания используется базовая конфигурация axios.
 * 
 * В реальном проекте здесь целесообразно внедрить:
 * 1. Интерцепторы для централизованной обработки ошибок (401, 403, 500).
 * 2. Автоматическое прикрепление Auth-токенов.
 * 3. Логику Refresh Token.
 * В текущей реализации для чистоты кода и экономии времени логика ошибок вынесена в UI-слой/сторы.
 */
const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

/**
 * Для запросов к API заказов
 */
export const orderApi = {
  /**
   * Получение всех заказов
   */
  getAllOrders: async (): Promise<OrderResponse[]> => {
    const response = await apiClient.get<OrderResponse[]>('/orders');
    return response.data;
  },

  /**
   * Получение заказа по идентификатору
   */
  getOrderById: async (id: string): Promise<OrderResponse> => {
    const response = await apiClient.get<OrderResponse>(`/orders/${id}`);
    return response.data;
  },

  /**
   * Создание нового заказа
   */
  createOrder: async (request: CreateOrderRequest): Promise<OrderResponse> => {
    const response = await apiClient.post<OrderResponse>('/orders', request);
    return response.data;
  },

  /**
   * Обновление статуса заказа
   */
  updateOrderStatus: async (
    id: string,
    request: UpdateOrderStatusRequest,
  ): Promise<void> => {
    await apiClient.put(`/orders/${id}/status`, request);
  },

  /**
   * Удаление заказа
   */
  deleteOrder: async (id: string): Promise<void> => {
    await apiClient.delete(`/orders/${id}`);
  },
};

