import { Sidebar } from "@/components/layout/sidebar";

export default function DashboardLayout({ children }: { children: React.ReactNode }) {
  return (
    <div className="min-h-screen bg-slate-900 flex text-slate-100">
      <Sidebar />
      <main className="flex-1 p-8 overflow-y-auto bg-slate-900/95">
        {children}
      </main>
    </div>
  );
}
