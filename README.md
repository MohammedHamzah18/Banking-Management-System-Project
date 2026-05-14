# Banking Management System

Professional ASP.NET Core Banking Management System using MVC, Web API, EF Core, SQL Server, Repository Pattern, Service Layer, JWT authentication, role authorization, middleware, responsive Bootstrap UI, SQL scripts, and Postman examples.

## Folder Structure

```text
src/
  BankingSystem.Domain/     Entities, enums, DTOs, shared models
  BankingSystem.Data/       EF Core DbContext, repositories, unit of work
  BankingSystem.Services/   Business logic and interfaces
  BankingSystem.Web/        MVC controllers, API controllers, middleware, views, wwwroot
database/                   SQL schema, stored procedures, sample data
docs/                       Postman examples
```

## Setup

1. Install .NET 8 SDK, SQL Server or LocalDB, and Visual Studio 2022.
2. Open `BankingManagementSystem.sln`.
3. Update `src/BankingSystem.Web/appsettings.json` if your SQL Server connection string is different.
4. Run EF Core migrations:

```powershell
dotnet tool install --global dotnet-ef
dotnet restore
dotnet ef migrations add InitialCreate --project src/BankingSystem.Data --startup-project src/BankingSystem.Web
dotnet ef database update --project src/BankingSystem.Data --startup-project src/BankingSystem.Web
dotnet run --project src/BankingSystem.Web
```

5. Open `https://localhost:5001` or the URL printed by `dotnet run`.
6. Open Swagger at `/swagger` for API testing.

## Architecture

This solution follows 3-layer architecture:

- Presentation Layer: MVC views and REST API controllers in `BankingSystem.Web`.
- Business Layer: Service interfaces and implementations in `BankingSystem.Services`.
- Data Layer: EF Core `BankingDbContext`, repositories, and unit of work in `BankingSystem.Data`.
- Domain Layer: Entities, DTOs, enums, and common reusable classes in `BankingSystem.Domain`.

Controllers stay thin. They validate HTTP input and call services. Services enforce business rules such as active account checks, insufficient balance prevention, transfer validation, account status changes, and EMI calculation. The data layer centralizes persistence.

## Database Relationships

- `Roles` 1-to-many `Users`
- `Users` 1-to-0/1 `Customers`
- `Customers` 1-to-many `Accounts`
- `Customers` 1-to-many `Loans`
- `Accounts` 1-to-many `Transactions`
- `Transactions` optionally reference a destination account for transfers

Unique constraints protect role names, user emails, customer emails, account numbers, and transaction references.

## Authentication Flow

1. User registers or logs in through `/api/auth`.
2. Passwords are hashed using BCrypt.
3. On successful login, the API returns a JWT.
4. The client sends `Authorization: Bearer <token>` on protected requests.
5. ASP.NET Core JWT middleware validates issuer, audience, expiry, and signature.
6. Role claims enable `Admin`, `Employee`, and `Customer` authorization rules.

## Middleware Flow

Request enters `ExceptionHandlingMiddleware`, then `RequestLoggingMiddleware`, then static files/routing/auth/controller execution. Exceptions are converted to consistent JSON responses, and request duration is logged.

## Dependency Injection

`Program.cs` registers DbContext, repository/unit-of-work, and all services as scoped dependencies. Controllers receive interfaces through constructor injection, which keeps code testable and loosely coupled.

## Interview Questions

Q: Why use a service layer?
A: It keeps business rules out of controllers and makes workflows like transfer, loan approval, and account freezing easier to test and maintain.

Q: Why use repository pattern with EF Core?
A: It creates a consistent data access abstraction and keeps persistence concerns separate from business logic. EF Core already implements many repository-like features, so this project uses a lightweight generic repository.

Q: How is transfer consistency handled?
A: The source debit, destination credit, and transaction insert happen in one EF Core `SaveChangesAsync` call. In production, wrap high-value operations in an explicit database transaction and use row/version concurrency checks.

Q: How does JWT authorization work?
A: Login creates signed claims including role. Protected endpoints use `[Authorize]` and role filters to allow only permitted users.

Q: What is the difference between MVC and Web API here?
A: MVC renders HTML screens for staff workflows. Web API exposes JSON endpoints for Postman, mobile clients, or SPA front ends.

Q: How is EMI calculated?
A: EMI uses the standard reducing balance formula with monthly interest rate, principal, and tenure in months.

Q: What improvements would you add for production?
A: Refresh tokens, account locking, audit trails on every mutation, concurrency tokens, explicit database transactions, background jobs, real payment gateway integration, full test suite, and CI/CD.
