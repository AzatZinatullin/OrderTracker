import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import { Package2 } from 'lucide-react';
import HomePage from './pages/HomePage';
import OrderPage from './pages/OrderPage';
import { useSignalR } from './hooks/useSignalR';

/**
 * Основной компонент приложения
 */
function App() {
  useSignalR();
  
  return (
    <Router>
      <div className="min-h-screen bg-slate-50 text-slate-900 font-sans selection:bg-indigo-200">
        <header className="bg-white border-b border-slate-200 sticky top-0 z-10 backdrop-blur-sm bg-white/80">
          <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 h-16 flex items-center justify-between">
            <Link to="/" className="flex items-center gap-2 group">
              <div className="bg-indigo-600 text-white p-2 rounded-xl group-hover:scale-105 transition-transform shadow-lg shadow-indigo-200">
                <Package2 size={24} />
              </div>
              <span className="text-xl font-bold tracking-tight bg-gradient-to-r from-indigo-700 to-violet-500 bg-clip-text text-transparent">
                Трекер заказов
              </span>
            </Link>
          </div>
        </header>

        <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
          <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/orders/:id" element={<OrderPage />} />
          </Routes>
        </main>
      </div>
    </Router>
  );
}

export default App;
