Welcome here:

---

### 🔐 Authority Service

This is an **Authority Management** project built with **.NET 8**, following **Domain-Driven Design (DDD)** principles.

#### ✅ Key Features

* **(DDD)**: Clear separation between Domain, Infrastructure, and API layers.(no application layer)
* **Mapperly**: Source generator-based object mapping for high performance.
* **MediatR**: Handles domain events and decouples internal logic.
* **Unit of Work**: Automatically commits changes to the database for `POST`, `PUT`, and `DELETE` HTTP requests.
  (For `GET` requests, you need to call `IUnitOfWork.SaveChanges()` manually if needed.)

#### 🔐 Authorization System

feature list:

* Supports permission-based **authorization across multiple systems**.
* Uses **JWT authentication** and **custom authorization middleware**.
* Caches permission validation results in **Redis** to boost performance and reduce database load.
* Clients check permissions by calling a **public API** with `SystemName`, `PermissionKey`, and `UserId`.
* Uses **Snowflake ID** generation for `User`, `Role`, and `Permission` entities.
* Supports microservices deployment with distributed locking to prevent concurrent writes to Redis during permission updates.
* Provides a **complete permission management API**, allowing you to manage users, roles, and permissions via RESTful endpoints.

---

Still updating...
