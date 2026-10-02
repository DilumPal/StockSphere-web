"use client";
import { useEffect, useRef } from "react";
import * as signalR from "@microsoft/signalr";
import { toast } from "sonner";
import { useAuthStore } from "@/store/authStore";

export function SignalRProvider({ children }: { children: React.ReactNode }) {
  const token = useAuthStore((state) => state.token);
  const connectionRef = useRef<signalR.HubConnection | null>(null);

  useEffect(() => {
    if (!token) {
      if (connectionRef.current) {
        connectionRef.current.stop();
        connectionRef.current = null;
      }
      return;
    }

    const connect = async () => {
      try {
        const connection = new signalR.HubConnectionBuilder()
          .withUrl("http://localhost:5000/hubs/inventory", {
            accessTokenFactory: () => token,
          })
          .withAutomaticReconnect()
          .build();

        connection.on("ReceiveLowStockAlert", (data: any) => {
          toast.error("Low Stock Alert!", {
            description: `${data.productName} (${data.sku}) is running low in ${data.warehouseName}. Current Stock: ${data.currentStock}`,
            duration: 8000,
            icon: '⚠️'
          });
        });

        await connection.start();
        console.log("SignalR Connected to InventoryHub.");
        connectionRef.current = connection;
      } catch (err) {
        console.error("SignalR Connection Error: ", err);
      }
    };

    connect();

    return () => {
      if (connectionRef.current) {
        connectionRef.current.stop();
      }
    };
  }, [token]);

  return <>{children}</>;
}
