import { create } from 'zustand';
import type { OrderResponse, OrderStatus } from '../types/Order';
import toast from 'react-hot-toast';
import { orderApi } from '../api/orderApi';

/**
 * Состояние заказов
 */
interface OrderState {
  orders: OrderResponse[];
  currentOrder: OrderResponse | null;
  isLoading: boolean;
  error: string | null;
  fetchOrders: () => Promise<void>;
  fetchOrderById: (id: string) => Promise<void>;
  createOrder: (description: string) => Promise<void>;
  updateOrderStatus: (id: string, newStatus: OrderStatus) => Promise<void>;

  handleOrderCreated: (order: OrderResponse) => void;
  handleOrderStatusUpdated: (payload: {
    orderId: string;
    newStatus: OrderStatus;
    updatedAt: string;
  }) => void;
  deleteOrder: (id: string) => Promise<void>;
  handleOrderDeleted: (orderId: string) => void;
}

/**
 * Создание хранилища заказов
 */
export const useOrderStore = create<OrderState>((set) => ({
  orders: [],
  currentOrder: null,
  isLoading: false,
  error: null,

  /**
   * Загрузка всех заказов
   */
  fetchOrders: async () => {
    set({ isLoading: true, error: null });
    try {
      const orders = await orderApi.getAllOrders();
      set({ orders, isLoading: false });
    } catch (error: any) {
      set({
        error: error.response?.data || 'Не удалось создать заказ',
        isLoading: false,
      });
    }
  },

  /**
   * Загрузка заказа по идентификатору
   * @param id Идентификатор заказа
   */
  fetchOrderById: async (id: string) => {
    set({ isLoading: true, error: null });
    try {
      const order = await orderApi.getOrderById(id);
      set({ currentOrder: order, isLoading: false });
    } catch (error: any) {
      set({
        error: error.response?.data || 'Не удалось создать заказ',
        isLoading: false,
      });
    }
  },

  /**
   * Создание заказа
   * @param description Описание заказа
   */
  createOrder: async (description: string) => {
    set({ isLoading: true, error: null });
      console.log('createOrder')

    try {
      console.log('try creating order')
      await orderApi.createOrder({ description });
      toast.success('Заказ успешно создан!');
      
      set({ isLoading: false });
    } catch (error: any) {
      const message = error.response?.data || 'Не удалось создать заказ';

      set({
        error: message,
        isLoading: false,
      });
      toast.error(message);
    }
  },

  /**
   * Обновление статуса заказа
   * @param id Идентификатор заказа
   * @param newStatus Новый статус
   */
  updateOrderStatus: async (id: string, newStatus: OrderStatus) => {
    try {
      await orderApi.updateOrderStatus(id, { newStatus });
    } catch (error: any) {
      toast.error('Не удалось обновить статус');
      throw error;
    }
  },

  /**
   * Удаление заказа
   * @param id Идентификатор заказа
   */
  deleteOrder: async (id: string) => {
    try {
      await orderApi.deleteOrder(id);
    } catch (error: any) {
      toast.error('Не удалось удалить заказ');
      throw error;
    }
  },

  /**
   * Обработчик создания заказа
   * @param newOrder Новый заказ
   */
  handleOrderCreated: (newOrder: OrderResponse) => {
    set((state) => {
      if (state.orders.some((o) => o.id === newOrder.id)) return state;
      return { orders: [newOrder, ...state.orders] };
    });
  },

  /**
   * Обработчик обновления статуса заказа
   * @param payload Данные об обновлении статуса
   */
  handleOrderStatusUpdated: (payload) => {
    set((state) => {
      const updatedOrders = state.orders.map((o) =>
        o.id === payload.orderId
          ? { ...o, status: payload.newStatus, updatedAt: payload.updatedAt }
          : o,
      );

      const updatedCurrentOrder =
        state.currentOrder?.id === payload.orderId
          ? {
              ...state.currentOrder,
              status: payload.newStatus,
              updatedAt: payload.updatedAt,
            }
          : state.currentOrder;

      return { orders: updatedOrders, currentOrder: updatedCurrentOrder };
    });
  },

  /**
   * Обработчик удаления заказа
   * @param orderId Идентификатор заказа
   */
  handleOrderDeleted: (orderId: string) => {
    set((state) => ({
      orders: state.orders.filter((o) => o.id !== orderId),
      currentOrder: state.currentOrder?.id === orderId ? null : state.currentOrder,
    }));
  },
}));
