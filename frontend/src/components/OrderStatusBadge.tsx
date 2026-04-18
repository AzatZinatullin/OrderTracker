import type { OrderStatus } from '../types/Order';
import clsx from 'clsx';
import { CheckCircle2, Clock, Package, XCircle } from 'lucide-react';

export function OrderStatusBadge({ status }: { status: OrderStatus }) {
  const isCreated = status === 'Created';
  const isShipped = status === 'Shipped';
  const isDelivered = status === 'Delivered';
  const isCancelled = status === 'Cancelled';

  const Icon = isCreated
    ? Clock
    : isShipped
      ? Package
      : isDelivered
        ? CheckCircle2
        : XCircle;

  const statusLabels: Record<OrderStatus, string> = {
    Created: 'Создан',
    Shipped: 'Отправлен',
    Delivered: 'Доставлен',
    Cancelled: 'Отменен',
  };

  return (
    <span
      className={clsx(
        'inline-flex items-center gap-1.5 px-3 py-1 rounded-full text-sm font-medium border shadow-sm transition-colors',
        {
          'bg-blue-50 text-blue-700 border-blue-200': isCreated,
          'bg-amber-50 text-amber-700 border-amber-200': isShipped,
          'bg-emerald-50 text-emerald-700 border-emerald-200': isDelivered,
          'bg-red-50 text-red-700 border-red-200': isCancelled,
        },
      )}
    >
      <Icon size={16} className={clsx({ 'animate-pulse': isShipped })} />
      {statusLabels[status]}
    </span>
  );
}
