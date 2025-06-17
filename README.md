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

* Supports permission-based **authorization across multiple systems**.
* Uses **JWT authentication** and **custom authorization middleware**.
* After permission validation, results are cached in **Redis** to improve performance and reduce DB queries.
* To check permissions, clients call the **public API** using `SystemName`, `PermissionKey`, and `UserId`.

---

Still updating...
