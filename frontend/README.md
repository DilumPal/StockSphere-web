# StockSphere 🚀

![StockSphere Overview](https://img.shields.io/badge/Status-Active-brightgreen) ![.NET Version](https://img.shields.io/badge/.NET-10.0-blue) ![Next.js Version](https://img.shields.io/badge/Next.js-15-black)

**StockSphere** is a modern, full-stack, real-time inventory and supply chain management platform. It is designed to help businesses efficiently manage products, suppliers, warehouses, and physical stock movements through an intuitive and responsive analytics dashboard.

---

## ✨ Features

- **Real-Time Inventory Tracking:** Stock levels are automatically computed and updated atomically whenever Purchase Orders (receiving stock) or Sales Orders (dispatching stock) are completed.
- **Live WebSocket Notifications:** Integrates ASP.NET Core SignalR to push real-time alerts to the frontend dashboard the exact moment a product's stock drops below its defined reorder threshold.
- **Immutable Audit Ledger:** Every single stock movement (In, Out, or Adjustment) is securely logged in a read-only inventory transaction ledger.
- **Master Data Management:** Full CRUD capabilities for managing Product catalogs, Supplier relationships, and Warehouse locations.
- **Supabase Cloud Storage:** Product images are streamed directly to Supabase object storage buckets via the .NET API.
- **Secure Authentication:** JWT-based authentication and role-based access control (RBAC) secure the API endpoints.
- **Rich Analytics Dashboard:** Visually stunning, glassmorphism-styled dashboard featuring Recharts data visualizations for inventory valuation trends.

---

## 🏗️ Architecture & Tech Stack

The project is decoupled into a backend REST API and a separate frontend web client.

### Backend (API)
Developed using **Clean Architecture** principles to separate core domain logic from infrastructure dependencies.
- **Framework:** C# / ASP.NET Core Web API (.NET 10)
- **Database:** PostgreSQL (via Docker)
- **ORM:** Entity Framework Core
- **Real-Time:** ASP.NET Core SignalR (WebSockets)
- **Storage:** Supabase REST API
- **Testing:** xUnit & Moq

### Frontend (Web Client)
- **Framework:** Next.js 15 (React 19)
- **Styling:** Tailwind CSS (with modern Glassmorphism aesthetics)
- **State Management:** Zustand
- **HTTP Client:** Axios
- **Icons & UI:** Lucide React & Sonner (Toast Notifications)
- **Data Visualization:** Recharts

---

## 🚀 Getting Started

Follow these instructions to run the complete StockSphere system locally on your machine.

### Prerequisites
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (for the PostgreSQL database)
- [.NET SDK 10.0](https://dotnet.microsoft.com/)
- [Node.js (v18 or higher)](https://nodejs.org/)

### 1. Start the Database
The database infrastructure is orchestrated via Docker Compose.
`ash
cd backend
docker-compose up -d
`
*(This starts PostgreSQL on port 5434 and pgAdmin on http://localhost:5050)*

### 2. Configure Environment Variables
In the ackend/StockSphere.Api folder, ensure your ppsettings.json is configured with your JWT Secret and Supabase credentials:
`json
{
  "JwtSettings": { "Secret": "your-256-bit-secret-key" },
  "Supabase": {
    "Url": "https://your-project.supabase.co",
    "Key": "your-service-role-key",
    "BucketName": "products"
  }
}
`

### 3. Run the Backend API
Apply the EF Core database migrations and start the server:
`ash
cd backend/StockSphere.Api
dotnet ef database update
dotnet run
`
*(The API and SignalR Hub will be listening on http://localhost:5000)*

### 4. Run the Frontend Dashboard
Open a new terminal window and start the Next.js client:
`ash
cd frontend
npm install
npm run dev
`
*(The web application will be accessible at http://localhost:3000)*

---

## 📖 License
This project is licensed under the MIT License.

![CodeRabbit Pull Request Reviews](https://img.shields.io/coderabbit/prs/github/DilumPal/StockSphere?utm_source=oss&utm_medium=github&utm_campaign=DilumPal%2FStockSphere&labelColor=171717&color=FF570A&link=https%3A%2F%2Fcoderabbit.ai&label=CodeRabbit+Reviews)