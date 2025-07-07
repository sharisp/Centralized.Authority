## 👋 Welcome to the Authority Service

---

### 🔐 Authority Service

A powerful **Authority Management System** built with **.NET 8**, following **Domain-Driven Design (DDD)** principles (without an Application layer).

---

### 📦 Tech Stack

* ASP.NET Core 8
* Entity Framework Core
* JWT Bearer Authentication + Refresh Token
* SQL Server
* Redis
* Docker (optional)
* MediatR 

---

### 🚀 Core Features & Highlights

* 🧱 **DDD Structure**: Clear separation of Domain, Infrastructure, and API layers
* 💾 **Unit of Work**: Automatic DB commits for `POST`, `PUT`, and `DELETE`
  *(Manual commit needed for `GET` requests)*
* 📬 **MediatR Integration**: Decoupled command and domain event handling for clean architecture
* 🔐 **JWT Authentication with Refresh Token**
  * Secure login returns both **Access Token** and **Refresh Token**
  * Token refresh endpoint for seamless session continuation
  * Logout invalidates refresh tokens
* 🔍 **Permission-Based Authorization** across multiple systems with fine-grained control
   * Role-Based Access Control (RBAC):
   Menus are associated with permissions, enabling fine-grained UI control (e.g., button visibility).
   For pure API protection, permissions can be used independently of menus.
* 🚀 **Redis Caching**: High-performance caching of permission checks to reduce DB load
* 🧾 **Public Permission Check API**: Query permissions by providing `SystemName`, `PermissionKey`, and `UserId`
* ❄️ **Snowflake ID Generation** for users, roles, and permissions to ensure distributed uniqueness
* 🛠️ **Full RESTful API** support for managing users, roles, and permissions
* 📦 **Microservice Ready**: Distributed locking ensures safe Redis writes during permission updates

---

