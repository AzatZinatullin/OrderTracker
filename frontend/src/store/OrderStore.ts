import { create } from 'zustand';
import { orderApi } from '../api/orderApi';
import type { OrderResponse, OrderStatus } from '../types/Order';
import toast from 'react-hot-toast';

interface OrderState {
  orders: OrderResponse[];
  currentOrder: OrderResponse | null;
  isLoading: boolean;
  error: string | null;
  fetchOrders: () => Promise<void>;
  fetchOrderById: (id: string) => Promise<void>;
  createOrder: (description: string) => Promise<void>;
  updateOrderStatus: (id: string, newStatus: OrderStatus) => Promise<void>;
  // Real-time integration methods
  handleOrderCreated: (order: OrderResponse) => void;
  handleOrderStatusUpdated: (payload: {
    orderId: string;
    newStatus: OrderStatus;
    updatedAt: string;
  }) => void;
  deleteOrder: (id: string) => Promise<void>;
  handleOrderDeleted: (orderId: string) => void;
}

export const useOrderStore = create<OrderState>((set) => ({
  orders: [],
  currentOrder: null,
  isLoading: false,
  error: null,

  fetchOrders: async () => {
    set({ isLoading: true, error: null });
    try {
      const orders = await orderApi.getAllOrders();
      set({ orders, isLoading: false });
    } catch (error: any) {
      set({
        error: error.message || 'Не удалось загрузить заказы',
        isLoading: false,
      });
    }
  },

  fetchOrderById: async (id: string) => {
    set({ isLoading: true, error: null });
    try {
      const order = await orderApi.getOrderById(id);
      set({ currentOrder: order, isLoading: false });
    } catch (error: any) {
      set({
        error: error.message || 'Не удалось загрузить заказ',
        isLoading: false,
      });
    }
  },

  createOrder: async (description: string) => {
    set({ isLoading: true, error: null });
    try {
      await orderApi.createOrder({ description });
      toast.success('Заказ успешно создан!');
      // Real-time will push it to the list
      set({ isLoading: false });
    } catch (error: any) {
      set({
        error: error.message || 'Не удалось создать заказ',
        isLoading: false,
      });
      toast.error('Не удалось создать заказ');
    }
  },

  updateOrderStatus: async (id: string, newStatus: OrderStatus) => {
    try {
      await orderApi.updateOrderStatus(id, { newStatus });
    } catch (error: any) {
      toast.error('Не удалось обновить статус');
      throw error;
    }
  },

  deleteOrder: async (id: string) => {
    try {
      await orderApi.deleteOrder(id);
      // Real-time will handle the list update
    } catch (error: any) {
      toast.error('Не удалось удалить заказ');
      throw error;
    }
  },

  handleOrderCreated: (newOrder: OrderResponse) => {
    set((state) => {
      // Avoid duplicates just in case
      if (state.orders.some((o) => o.id === newOrder.id)) return state;
      return { orders: [newOrder, ...state.orders] };
    });
  },

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

  handleOrderDeleted: (orderId: string) => {
    set((state) => ({
      orders: state.orders.filter((o) => o.id !== orderId),
      currentOrder: state.currentOrder?.id === orderId ? null : state.currentOrder,
    }));
  },
}));
