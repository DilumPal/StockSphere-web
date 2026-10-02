"use client";

import { useEffect, useState } from "react";
import { Package, AlertTriangle, TrendingUp, DollarSign } from "lucide-react";
import api from "@/lib/axios";
import { AreaChart, Area, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer } from 'recharts';

export default function DashboardPage() {
  const [summary, setSummary] = useState({
    totalProducts: 0,
    lowStockAlerts: 0,
    activeOrders: 0,
    totalStockValue: 0
  });

  const [movements, setMovements] = useState([]);

  useEffect(() => {
    const fetchDashboardData = async () => {
      try {
        const [summaryRes, movementsRes] = await Promise.all([
          api.get("/analytics/summary"),
          api.get("/analytics/stock-movements")
        ]);
        setSummary(summaryRes.data);
        setMovements(movementsRes.data);
      } catch (err) {
        console.error("Failed to load dashboard data", err);
      }
    };
    fetchDashboardData();
  }, []);

  const monthlyData = [
    { name: 'Jan', stockValue: 4000, orders: 24 },
    { name: 'Feb', stockValue: 3000, orders: 13 },
    { name: 'Mar', stockValue: 2000, orders: 48 },
    { name: 'Apr', stockValue: 2780, orders: 39 },
    { name: 'May', stockValue: 1890, orders: 48 },
    { name: 'Jun', stockValue: 2390, orders: 38 },
    { name: 'Jul', stockValue: 3490, orders: 43 },
  ];

  return (
    <div className="space-y-6 animate-in fade-in slide-in-from-bottom-4 duration-500">
      <div className="flex justify-between items-center mb-8">
        <div>
          <h1 className="text-3xl font-bold tracking-tight bg-clip-text text-transparent bg-gradient-to-r from-blue-400 to-purple-500">
            Analytics Overview
          </h1>
          <p className="text-slate-400 mt-1">Here is what's happening with your inventory today.</p>
        </div>
      </div>

      {/* KPI Cards */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        <KPICard 
          title="Total Products" 
          value={summary.totalProducts.toString()} 
          icon={<Package className="h-6 w-6 text-blue-400" />} 
          trend="+12%" 
          bgClass="bg-blue-500/10"
        />
        <KPICard 
          title="Stock Value" 
          value={`$${summary.totalStockValue.toLocaleString()}`} 
          icon={<DollarSign className="h-6 w-6 text-emerald-400" />} 
          trend="+5%" 
          bgClass="bg-emerald-500/10"
        />
        <KPICard 
          title="Active Orders" 
          value={summary.activeOrders.toString()} 
          icon={<TrendingUp className="h-6 w-6 text-purple-400" />} 
          trend="+18%" 
          bgClass="bg-purple-500/10"
        />
        <KPICard 
          title="Low Stock Alerts" 
          value={summary.lowStockAlerts.toString()} 
          icon={<AlertTriangle className="h-6 w-6 text-red-400" />} 
          trend="-2" 
          bgClass="bg-red-500/10"
        />
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6 mt-6">
        {/* Main Chart */}
        <div className="lg:col-span-2 bg-slate-900 border border-slate-800 rounded-xl p-6 shadow-xl relative overflow-hidden group">
          <div className="absolute inset-0 bg-gradient-to-br from-blue-500/5 to-purple-500/5 opacity-0 group-hover:opacity-100 transition-opacity duration-500" />
          <h3 className="text-lg font-semibold text-slate-200 mb-6">Inventory Valuation Trend</h3>
          <div className="h-72 w-full">
            <ResponsiveContainer width="100%" height="100%">
              <AreaChart data={monthlyData} margin={{ top: 10, right: 30, left: 0, bottom: 0 }}>
                <defs>
                  <linearGradient id="colorValue" x1="0" y1="0" x2="0" y2="1">
                    <stop offset="5%" stopColor="#3b82f6" stopOpacity={0.3}/>
                    <stop offset="95%" stopColor="#3b82f6" stopOpacity={0}/>
                  </linearGradient>
                </defs>
                <CartesianGrid strokeDasharray="3 3" stroke="#1e293b" vertical={false} />
                <XAxis dataKey="name" stroke="#64748b" tickLine={false} axisLine={false} />
                <YAxis stroke="#64748b" tickLine={false} axisLine={false} tickFormatter={(value) => `$${value}`} />
                <Tooltip 
                  contentStyle={{ backgroundColor: '#0f172a', borderColor: '#1e293b', borderRadius: '8px' }}
                  itemStyle={{ color: '#e2e8f0' }}
                />
                <Area type="monotone" dataKey="stockValue" stroke="#3b82f6" strokeWidth={3} fillOpacity={1} fill="url(#colorValue)" />
              </AreaChart>
            </ResponsiveContainer>
          </div>
        </div>

        {/* Recent Movements List */}
        <div className="bg-slate-900 border border-slate-800 rounded-xl p-6 shadow-xl">
          <h3 className="text-lg font-semibold text-slate-200 mb-6">Recent Movements</h3>
          <div className="space-y-4">
            {movements.length === 0 ? (
              <p className="text-slate-500 text-sm">No recent transactions found.</p>
            ) : (
              movements.map((m: any) => (
                <div key={m.id} className="flex items-center justify-between p-3 rounded-lg bg-slate-950/50 border border-slate-800/50 hover:bg-slate-800/50 transition-colors">
                  <div>
                    <p className="text-sm font-medium text-slate-200">{m.productName}</p>
                    <p className="text-xs text-slate-500">{new Date(m.transactionDate).toLocaleString()}</p>
                  </div>
                  <div className={`flex items-center gap-2 font-medium ${m.type === 'In' ? 'text-emerald-400' : 'text-blue-400'}`}>
                    {m.type === 'In' ? '+' : '-'}{m.quantity}
                  </div>
                </div>
              ))
            )}
          </div>
        </div>
      </div>
    </div>
  );
}

function KPICard({ title, value, icon, trend, bgClass }: { title: string, value: string, icon: React.ReactNode, trend: string, bgClass: string }) {
  return (
    <div className="bg-slate-900 border border-slate-800 rounded-xl p-6 shadow-xl relative overflow-hidden group hover:-translate-y-1 transition-transform duration-300">
      <div className="absolute inset-0 bg-gradient-to-br from-white/5 to-transparent opacity-0 group-hover:opacity-100 transition-opacity duration-300" />
      <div className="flex justify-between items-start">
        <div>
          <p className="text-sm font-medium text-slate-400">{title}</p>
          <h3 className="text-3xl font-bold text-white mt-2">{value}</h3>
        </div>
        <div className={`p-3 rounded-xl ${bgClass}`}>
          {icon}
        </div>
      </div>
      <div className="mt-4 flex items-center text-sm">
        <span className={trend.startsWith('+') ? 'text-emerald-400 font-medium' : 'text-red-400 font-medium'}>
          {trend}
        </span>
        <span className="text-slate-500 ml-2">vs last month</span>
      </div>
    </div>
  );
}
