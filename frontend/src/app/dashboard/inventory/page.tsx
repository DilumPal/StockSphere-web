"use client";
import { useEffect, useState } from "react";
import api from "@/lib/axios";
import { Search, ArrowDownCircle, ArrowUpCircle, AlertCircle } from "lucide-react";
import { Input } from "@/components/ui/input";

export default function InventoryPage() {
  const [stocks, setStocks] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchStocks = async () => {
      try {
        const response = await api.get('/stocks');
        setStocks(response.data);
      } catch (error) {
        console.error('Failed to fetch stocks', error);
      } finally {
        setLoading(false);
      }
    };
    fetchStocks();
  }, []);

  return (
    <div className="space-y-6 animate-in fade-in slide-in-from-bottom-4 duration-500">
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-3xl font-bold tracking-tight text-white">Inventory Ledger</h1>
          <p className="text-slate-400 mt-1">Real-time stock levels across all warehouses</p>
        </div>
      </div>

      <div className="bg-slate-900 border border-slate-800 rounded-xl shadow-xl overflow-hidden">
        <div className="p-4 border-b border-slate-800 flex items-center gap-4">
          <div className="relative flex-1 max-w-sm">
            <Search className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-slate-500" />
            <Input 
              placeholder="Filter stock by SKU or Warehouse..." 
              className="pl-9 bg-slate-950 border-slate-800 text-slate-200 focus-visible:ring-blue-500"
            />
          </div>
        </div>
        <div className="overflow-x-auto">
          <table className="w-full text-sm text-left">
            <thead className="text-xs uppercase bg-slate-950/50 text-slate-400 border-b border-slate-800">
              <tr>
                <th className="px-6 py-4 font-medium">Product / SKU</th>
                <th className="px-6 py-4 font-medium">Warehouse</th>
                <th className="px-6 py-4 font-medium text-right">Quantity In Stock</th>
                <th className="px-6 py-4 font-medium">Status</th>
                <th className="px-6 py-4 font-medium text-right">Last Updated</th>
              </tr>
            </thead>
            <tbody>
              {loading ? (
                <tr><td colSpan={5} className="px-6 py-4 text-center text-slate-500">Loading hardware data...</td></tr>
              ) : stocks.length === 0 ? (
                <tr><td colSpan={5} className="px-6 py-4 text-center text-slate-500">No inventory found</td></tr>
              ) : stocks.map((s: any) => (
                <tr key={s.id} className="border-b border-slate-800/50 hover:bg-slate-800/20 transition-colors">
                  <td className="px-6 py-4">
                    <p className="font-medium text-slate-200">{s.product?.name}</p>
                    <p className="text-xs text-slate-500 mt-0.5">{s.product?.sku}</p>
                  </td>
                  <td className="px-6 py-4 text-slate-300">
                    <span className="px-2.5 py-1 bg-slate-800 text-slate-300 rounded-full text-xs border border-slate-700">
                      {s.warehouse?.name} (Aisle {s.aisle}, Bin {s.bin})
                    </span>
                  </td>
                  <td className="px-6 py-4 text-right font-mono text-slate-200">
                    {s.quantity}
                  </td>
                  <td className="px-6 py-4">
                    {s.quantity <= (s.product?.reorderLevel || 10) ? (
                      <span className="flex items-center gap-1.5 text-red-400 text-xs font-medium bg-red-500/10 w-fit px-2 py-1 rounded-full border border-red-500/20">
                        <span className="h-1.5 w-1.5 rounded-full bg-red-500 animate-pulse"></span> Low Stock
                      </span>
                    ) : (
                      <span className="flex items-center gap-1.5 text-emerald-400 text-xs font-medium bg-emerald-500/10 w-fit px-2 py-1 rounded-full border border-emerald-500/20">
                        <span className="h-1.5 w-1.5 rounded-full bg-emerald-500"></span> Optimal
                      </span>
                    )}
                  </td>
                  <td className="px-6 py-4 text-right text-slate-400 text-xs">
                    {new Date(s.lastUpdated).toLocaleString()}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}
