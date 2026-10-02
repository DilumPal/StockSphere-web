"use client";
import Link from "next/link";
import { Package, Users, Truck, Home, LogOut, LayoutDashboard, Settings } from "lucide-react";
import { useAuthStore } from "@/store/authStore";

export function Sidebar() {
  const logout = useAuthStore(s => s.logout);
  const user = useAuthStore(s => s.user);

  const links = [
    { name: "Dashboard", href: "/dashboard", icon: LayoutDashboard },
    { name: "Products", href: "/dashboard/products", icon: Package },
    { name: "Inventory", href: "/dashboard/inventory", icon: Package },
    { name: "Categories", href: "/dashboard/categories", icon: Settings },
    { name: "Suppliers", href: "/dashboard/suppliers", icon: Truck },
    { name: "Warehouses", href: "/dashboard/warehouses", icon: Home },
    { name: "Employees", href: "/dashboard/employees", icon: Users },
  ];

  return (
    <div className="w-64 bg-slate-950 border-r border-slate-800 flex flex-col h-screen p-4">
      <div className="flex items-center gap-2 mb-8 px-2">
        <div className="h-8 w-8 rounded-lg bg-gradient-to-tr from-blue-500 to-purple-600 flex items-center justify-center font-bold text-white shadow-lg shadow-blue-500/20">
          S
        </div>
        <span className="text-xl font-bold bg-clip-text text-transparent bg-gradient-to-r from-blue-400 to-purple-500">
          StockSphere
        </span>
      </div>

      <nav className="flex-1 space-y-2">
        {links.map((link) => {
          const Icon = link.icon;
          return (
            <Link 
              key={link.name} 
              href={link.href}
              className="flex items-center gap-3 px-3 py-2.5 rounded-lg text-slate-300 hover:text-white hover:bg-slate-800 transition-colors"
            >
              <Icon className="h-5 w-5" />
              <span>{link.name}</span>
            </Link>
          );
        })}
      </nav>

      <div className="mt-auto border-t border-slate-800 pt-4 px-2">
        <div className="flex items-center gap-3 mb-4">
          <div className="h-10 w-10 rounded-full bg-slate-800 flex items-center justify-center font-semibold border border-slate-700 text-white">
            {user?.firstName?.[0] || 'U'}
          </div>
          <div>
            <p className="text-sm font-medium text-white">{user?.firstName} {user?.lastName}</p>
            <p className="text-xs text-slate-500">{user?.role}</p>
          </div>
        </div>
        <button 
          onClick={() => logout()}
          className="flex items-center gap-2 text-slate-400 hover:text-red-400 w-full px-2 py-2 rounded-lg hover:bg-red-500/10 transition-colors"
        >
          <LogOut className="h-4 w-4" />
          <span>Sign Out</span>
        </button>
      </div>
    </div>
  );
}
