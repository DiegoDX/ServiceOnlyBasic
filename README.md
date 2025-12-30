# ServiceOnlyBasic – Clean Architecture (.NET 10)

ServiceOnlyBasic is a modular Backend API built using Clean Architecture principles, implementing layers for Core, Application, Infrastructure and WebAPI.  
The solution includes JWT authentication, EF Core 10, SQLite for testing and SQL Server for development/production.

---

## 🧱 Architecture
The solution follows Clean Architecture principles:
- Core: business rules and entities
- Application: use cases and services
- Infrastructure: EF Core, repositories, security
- WebAPI: HTTP, authentication, middleware

---

## 🛠️ Tech Stack

- **.NET 10 WebAPI**
- **Clean Architecture**
- **Entity Framework Core 10**
- **SQL Server** (dev/prod)
- **SQLite in-memory** (tests)
- **JWT Authentication + Refresh Tokens**
- **xUnit
- **FluentValidation**

---

## Features
- Countries & Cities CRUD
- Pagination & filtering
- JWT authentication
- Role & policy authorization
- Refresh tokens
- Rate limiting
- API versioning

## 🚀 Running the API

1. Configure your `appsettings.json`:

json
"Jwt": {
  "Secret": "CHANGE_ME_IN_DEVELOPMENT"
}

2. Run EF migrations:
dotnet ef database update --project src/Infrastructure --startup-project src/WebAPI

3. Start the API:

dotnet run --project src/WebAPI


API will be available at:

https://localhost:7222/scalar/v1

🧪 Running Tests
dotnet test

SQLite in-memory is used automatically when the environment is Testing

👨‍💻 Author

Diego Carranza
Backend Developer – C# / .NET