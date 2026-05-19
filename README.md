<p align="center">
  <img src="./readme/images/logoàsense.png" alt="AdminSense" width="100">
</p>



# WebShop -Store, Card and Admin Panel

![Blazor](https://img.shields.io/badge/Blazor-Server-512BD4?style=flat-square&logo=blazor&logoColor=white) ![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet&logoColor=white) ![C#](https://img.shields.io/badge/C%23-13.0-239120?style=flat-square&logo=csharp&logoColor=white) ![SQL Server](https://img.shields.io/badge/SQL%20Server-Database-CC2927?style=flat-square&logo=microsoftsqlserver&logoColor=white) ![License](https://img.shields.io/badge/License-MIT-green?style=flat-square&logo=opensourceinitiative&logoColor=white) ![Copyright](https://img.shields.io/badge/Copyright-2026-blue?style=flat-square)


A digital platform or website that enables businesses to sell goods or services directly to consumers over the internet.

---

## 📖 Project Overview

**WebSHop** is a full-stack storefront: customers explore the catalog and use a cart, while staff use an admin area backed by a relational database and secure accounts.

---

## 🎯 Objective

Build an e-commerce web application to **list**, **manage**, and **sell** products online.

---

## ✨ Key Features

- **Product catalog** — Browse items with images, descriptions, and prices.
- **Shopping cart** — Add, remove, and update line items before checkout.
- **Admin panel** — Manage products, categories, and orders.
- **Authentication** — Customer registration, sign-in, and sign-out.

---

## 🛠️ Technologies Used

| Layer | Stack |
|--------|--------|
| **Backend** | ASP.NET Core (.NET 10), C# |
| **Database** | SQL Server (CRUD for products, users, orders, and related data) |
| **Frontend** | **Blazor Server** + Bootstrap (UI patterns: OpenIconic for navigation, Bootstrap Icons for actions) |
| **Data / Auth** | Entity Framework Core, ASP.NET Core Identity, AutoMapper |

---

## 📋 Prerequisites

- [Visual Studio](https://visualstudio.microsoft.com/) (recommended) or [Visual Studio Code](https://code.visualstudio.com/) with the C# / .NET extensions  
- [SQL Server](https://www.microsoft.com/sql-server) (local instance or compatible connection)  
- [.NET SDK](https://dotnet.microsoft.com/download) compatible with the solution (e.g. **.NET 10** for the current target framework)

---

## 🚀 Installation & Usage

1. **Clone** this repository and open the solution or the `WebSHop` project folder.
2. **Configure the database** — In `appsettings.json` (and `appsettings.Development.json` if needed), set the `ConnectionStrings:connWebshop` value to your SQL Server instance.
3. **Apply migrations** (from the `WebSHop` directory):

   ```bash
   dotnet ef database update
   ```

   *(Requires the EF Core tools: `dotnet tool install --global dotnet-ef` if not already installed.)*

4. **Run** the application:

   ```bash
   cd WebSHop
   dotnet run
   ```

5. Open the URL shown in the terminal (typically `https://localhost:5xxx` or `http://localhost:5xxx`) in your browser.

---

