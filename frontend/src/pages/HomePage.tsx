import { useEffect, useState } from 'react';
import { useOrderStore } from '../store/OrderStore';
import { OrderStatusBadge } from '../components/OrderStatusBadge';
import { Link } from 'react-router-dom';
import { Plus, ChevronRight, Package, Calendar, Trash2 } from 'lucide-react';

export default function HomePage() {
  const { orders, fetchOrders, createOrder, deleteOrder, isLoading } = useOrderStore();
  const [description, setDescription] = useState('');
  const [isCreating, setIsCreating] = useState(false);

  useEffect(() => {
    fetchOrders();
  }, [fetchOrders]);

  const handleCreate = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!description.trim()) return;
    setIsCreating(true);
    await createOrder(description);
    setDescription('');
    setIsCreating(false);
  };

  const handleDelete = async (id: string, e: React.MouseEvent) => {
    e.preventDefault();
    e.stopPropagation();
    if (confirm('Вы уверены, что хотите удалить этот заказ?')) {
      await deleteOrder(id);
    }
  };

  return (
    <div className='space-y-8 animate-in fade-in slide-in-from-bottom-4 duration-500'>
      <div className='flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4'>
        <div>
          <h1 className='text-3xl font-bold tracking-tight text-slate-900'>
            Обзор заказов
          </h1>
          <p className='text-slate-500 mt-1'>
            Управляйте и отслеживайте все ваши активные доставки в реальном времени.
          </p>
        </div>

        <form
          onSubmit={handleCreate}
          className='flex items-center gap-2 w-full sm:w-auto'
        >
          <input
            type='text'
            placeholder='Описание нового заказа...'
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            disabled={isCreating}
            className='flex-1 sm:w-64 px-4 py-2 rounded-xl border border-slate-200 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent shadow-sm transition-all bg-white'
          />
          <button
            type='submit'
            disabled={isCreating || !description.trim()}
            className='inline-flex items-center gap-2 bg-indigo-600 text-white px-4 py-2 rounded-xl shadow-md hover:bg-indigo-700 focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2 disabled:opacity-50 disabled:cursor-not-allowed transition-all active:scale-95'
          >
            <Plus size={18} />
            <span className='hidden sm:inline'>Создать</span>
          </button>
        </form>
      </div>

      <div className='bg-white rounded-2xl shadow-sm border border-slate-200 overflow-hidden'>
        {isLoading && orders.length === 0 ? (
          <div className='p-12 text-center text-slate-500'>
            <div className='animate-spin h-8 w-8 border-4 border-indigo-500 border-t-transparent rounded-full mx-auto mb-4'></div>
            Загрузка заказов...
          </div>
        ) : orders.length === 0 ? (
          <div className='p-12 text-center text-slate-500'>
            <Package size={48} className='mx-auto text-slate-300 mb-4' />
            <p className='text-lg font-medium text-slate-900'>
              Заказы не найдены
            </p>
            <p>Создайте свой первый заказ выше, чтобы начать.</p>
          </div>
        ) : (
          <ul className='divide-y divide-slate-100'>
            {orders.map((order) => (
              <li
                key={order.id}
                className='group hover:bg-slate-50 transition-colors'
              >
                <Link
                  to={`/orders/${order.id}`}
                  className='flex items-center px-6 py-5'
                >
                  <div className='min-w-0 flex-1 flex items-center gap-4'>
                    <div className='bg-indigo-50 text-indigo-700 p-3 rounded-xl shadow-inner group-hover:bg-indigo-100 transition-colors'>
                      <Package size={24} />
                    </div>
                    <div>
                      <div className='flex items-center gap-2'>
                        <p className='text-sm font-semibold text-slate-900 group-hover:text-indigo-600 transition-colors'>
                          {order.orderNumber}
                        </p>
                        <OrderStatusBadge status={order.status} />
                      </div>
                      <p className='mt-1 text-sm text-slate-500 truncate max-w-md'>
                        {order.description}
                      </p>
                      <div className='flex items-center gap-1 mt-1 text-xs text-slate-400'>
                        <Calendar size={12} />
                        <span>
                          Создан{' '}
                          {new Intl.DateTimeFormat('ru-RU', {
                            month: 'short',
                            day: 'numeric',
                            hour: '2-digit',
                            minute: '2-digit',
                          }).format(new Date(order.createdAt))}
                        </span>
                      </div>
                    </div>
                  </div>
                  <div className='ml-5 flex-shrink-0 flex items-center gap-4'>
                    <button
                      onClick={(e) => handleDelete(order.id, e)}
                      className='p-2 text-slate-300 hover:text-red-500 hover:bg-red-50 rounded-lg transition-all opacity-0 group-hover:opacity-100 focus:opacity-100'
                      title='Удалить заказ'
                    >
                      <Trash2 size={18} />
                    </button>
                    <ChevronRight className='text-slate-400 group-hover:text-indigo-500 transition-colors transform group-hover:translate-x-1' />
                  </div>
                </Link>
              </li>
            ))}
          </ul>
        )}
      </div>
    </div>
  );
}
