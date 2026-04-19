import { useEffect } from 'react';
import { useParams, Link } from 'react-router-dom';
import { useOrderStore } from '../store/OrderStore';
import { OrderStatusBadge } from '../components/OrderStatusBadge';
import type { OrderStatus } from '../types/Order';
import { ArrowLeft, Check, Truck, XCircle, Clock } from 'lucide-react';
import clsx from 'clsx';

/**
 * Шаги отслеживания заказа
 */
const steps = [
  { status: 'Created' as OrderStatus, label: 'Создан', icon: Clock },
  { status: 'Shipped' as OrderStatus, label: 'Отправлен', icon: Truck },
  { status: 'Delivered' as OrderStatus, label: 'Доставлен', icon: Check },
];

/**
 * Страница заказа
 */
export default function OrderPage() {
  const { id } = useParams<{ id: string }>();
  const { currentOrder, fetchOrderById, updateOrderStatus, isLoading } =
    useOrderStore();

  useEffect(() => {
    if (id) fetchOrderById(id);
  }, [id, fetchOrderById]);

  if (isLoading && !currentOrder) {
    return (
      <div className='p-12 text-center text-slate-500'>
        Загрузка деталей заказа...
      </div>
    );
  }

  if (!currentOrder) {
    return (
      <div className='text-center p-12'>
        <h2 className='text-2xl font-bold text-slate-900'>Заказ не найден</h2>
        <Link
          to='/'
          className='text-indigo-600 hover:underline mt-4 inline-block'
        >
          Назад к заказам
        </Link>
      </div>
    );
  }

  /**
   * Индекс текущего шага
   */
  const currentStepIndex = steps.findIndex(
    (s) => s.status === currentOrder.status,
  );

  /**
   * Флаг отмененного заказа
   */
  const isCancelled = currentOrder.status === 'Cancelled';

  const handleUpdateStatus = async (status: OrderStatus) => {
    if (id) await updateOrderStatus(id, status);
  };

  return (
    <div className='max-w-3xl mx-auto animate-in fade-in slide-in-from-bottom-4 duration-500'>
      <Link
        to='/'
        className='inline-flex items-center gap-2 text-slate-500 hover:text-slate-900 transition-colors mb-6'
      >
        <ArrowLeft size={16} />
        Назад ко всем заказам
      </Link>

      <div className='bg-white rounded-2xl shadow-sm border border-slate-200 overflow-hidden'>
        <div className='p-6 sm:p-8 border-b border-slate-100 flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4'>
          <div>
            <div className='flex items-center gap-3'>
              <h1 className='text-2xl font-bold text-slate-900'>
                {currentOrder.orderNumber}
              </h1>
              <OrderStatusBadge status={currentOrder.status} />
            </div>
            <p className='text-slate-500 mt-1'>{currentOrder.description}</p>
          </div>

          {/* Кнопки для имитации изменения состояния */}
          <div className='flex flex-wrap gap-2 relative z-10'>
            {currentOrder.status === 'Created' && (
              <>
                <button
                  onClick={() => handleUpdateStatus('Shipped')}
                  className='px-4 py-2 text-sm font-medium bg-amber-500 text-white rounded-lg shadow hover:bg-amber-600 transition-colors'
                >
                  Отправить
                </button>
                <button
                  onClick={() => handleUpdateStatus('Cancelled')}
                  className='px-4 py-2 text-sm font-medium bg-slate-100 hover:bg-red-50 hover:text-red-600 text-slate-700 rounded-lg transition-colors'
                >
                  Отменить
                </button>
              </>
            )}
            {currentOrder.status === 'Shipped' && (
              <button
                onClick={() => handleUpdateStatus('Delivered')}
                className='px-4 py-2 text-sm font-medium bg-emerald-500 text-white rounded-lg shadow hover:bg-emerald-600 transition-colors'
              >
                Доставить
              </button>
            )}
          </div>
        </div>

        <div className='p-6 sm:p-8 bg-slate-50'>
          <h3 className='text-sm font-semibold text-slate-900 uppercase tracking-wider mb-6'>
            История отслеживания
          </h3>

          <div className='relative'>
            {isCancelled ? (
              <div className='flex items-center gap-4 text-red-600 bg-red-50 p-4 rounded-xl border border-red-100'>
                <XCircle size={24} />
                <div>
                  <p className='font-medium'>Заказ отменен</p>
                  <p className='text-sm opacity-80'>
                    Этот заказ больше не будет выполнен.
                  </p>
                </div>
              </div>
            ) : (
              <div className='space-y-8 relative before:absolute before:inset-0 before:ml-5 before:-translate-x-px md:before:mx-auto md:before:translate-x-0 before:h-full before:w-0.5 before:bg-gradient-to-b before:from-indigo-500 before:via-slate-200 before:to-slate-200'>
                {steps.map((step, idx) => {
                  const isCompleted = currentStepIndex >= idx;
                  const isCurrent = currentStepIndex === idx;
                  const Icon = step.icon;

                  return (
                    <div
                      key={step.status}
                      className='relative flex items-center justify-between md:justify-normal md:odd:flex-row-reverse group is-active'
                    >
                      <div
                        className={clsx(
                          'flex items-center justify-center w-10 h-10 rounded-full border-4 border-white shadow shrink-0 md:order-1 md:group-odd:-translate-x-1/2 md:group-even:translate-x-1/2 transition-colors duration-500',
                          isCompleted
                            ? 'bg-indigo-500 text-white'
                            : 'bg-slate-200 text-slate-400',
                        )}
                      >
                        <Icon
                          size={16}
                          className={clsx(isCurrent && 'animate-pulse')}
                        />
                      </div>
                      <div className='w-[calc(100%-4rem)] md:w-[calc(50%-2.5rem)] p-4 rounded-xl border border-slate-200 bg-white shadow-sm hover:shadow-md transition-shadow'>
                        <div className='flex items-center justify-between mb-1'>
                          <h4
                            className={clsx(
                              'font-semibold',
                              isCompleted ? 'text-slate-900' : 'text-slate-400',
                            )}
                          >
                            {step.label}
                          </h4>
                          {isCurrent && (
                            <span className='text-xs font-medium px-2 py-0.5 rounded-full bg-indigo-50 text-indigo-700 backdrop-blur'>
                              Текущий
                            </span>
                          )}
                        </div>
                        <p className='text-sm text-slate-500'>
                          {isCurrent
                            ? `Обновлено ${new Intl.DateTimeFormat('ru-RU', { timeStyle: 'short', dateStyle: 'medium' }).format(new Date(currentOrder.updatedAt))}`
                            : isCompleted
                              ? 'Завершено'
                              : 'Ожидает завершения'}
                        </p>
                      </div>
                    </div>
                  );
                })}
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}
