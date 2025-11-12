# \# 📡 DevicesApi

# 

# A RESTful API for managing devices, built with ASP.NET Core, Entity Framework Core, and SQL Server. Fully containerized with Docker for streamlined development and testing.

# 

# ---

# 

# \## 🚀 Technologies and Design Patterns Used

# 

# \- \*\*ASP.NET Core 9.0\*\*

# \- \*\*Entity Framework Core\*\*

# &nbsp; - Automatic migrations

# &nbsp; - Model validation

# \- \*\*FluentValidation\*\*

# &nbsp; - Asynchronous validation with database access

# &nbsp; - Decoupled business rules

# \- \*\*AutoMapper\*\*

# &nbsp; - DTO-to-entity mapping

# \- \*\*Repository Pattern\*\*

# &nbsp; - Data access abstraction

# \- \*\*Service Layer\*\*

# &nbsp; - Centralized business logic

# \- \*\*Docker \& Docker Compose\*\*

# &nbsp; - API, SQL Server, and test runner in containers

# \- \*\*xUnit + Moq + FluentAssertions\*\*

# &nbsp; - Unit and service testing

# 

# ---

# 



3

# \## 🧪 Business Rules Implemented

# 

# \- ✅ Devices must have `Name`, `Brand`, and `State`

# \- ✅ Devices can not be deleted or updated if the state is in use

# 

# ---

# \## 📋 Request Logging Middleware

# 

# To improve observability and debugging, the API includes a custom middleware that logs all incoming HTTP requests and their corresponding responses.

# 

# ---

# 

# \### ✅ What It Logs

# 

# \- HTTP method and request path

# \- Response status code

# \- Execution time in milliseconds

# 

# ---

# 

# \### 🧱 Middleware Implementation

# 

# ```csharp

# public class RequestLoggingMiddleware {

# &nbsp;   private readonly RequestDelegate \_next;

# &nbsp;   private readonly ILogger<RequestLoggingMiddleware> \_logger;

# 

# &nbsp;   public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger) {

# &nbsp;       \_next = next;

# &nbsp;       \_logger = logger;

# &nbsp;   }

# 

# &nbsp;   public async Task InvokeAsync(HttpContext context) {

# &nbsp;       var stopwatch = Stopwatch.StartNew();

# 

# &nbsp;       \_logger.LogInformation("Incoming request: {method} {url}", context.Request.Method, context.Request.Path);

# 

# &nbsp;       await \_next(context);

# 

# &nbsp;       stopwatch.Stop();

# &nbsp;       \_logger.LogInformation("Response: {statusCode} in {elapsed}ms",

# &nbsp;           context.Response.StatusCode, stopwatch.ElapsedMilliseconds);

# &nbsp;   }

# }







# \## 🧰 API Endpoints

# 

# \### 🔍 List all devices

# 

# ```bash

# curl -X GET http://localhost:5000/api/devices

# 

# 🔍 Get device by ID

# 

# curl -X GET http://localhost:5000/api/devices/1

# 

# ➕ Create a new device

# 

# curl -X POST http://localhost:5000/api/devices \\

# &nbsp; -H "Content-Type: application/json" \\

# &nbsp; -d '{

# &nbsp;   "name": "Sensor A",

# &nbsp;   "brand": "Acme",

# &nbsp;   "state": "Avaliable"

# &nbsp; }'

# 

# ✏️ Update a device

# 

# curl -X PUT http://localhost:5000/api/devices/1 \\

# &nbsp; -H "Content-Type: application/json" \\

# &nbsp; -d '{

# &nbsp;   "name": "Sensor A",

# &nbsp;   "brand": "Acme",

# &nbsp;   "state": "InUse"

# &nbsp; }'

# 

# ❌ Delete a device

# 

# curl -X DELETE http://localhost:5000/api/devices/1

# 

# 🐳 Running with Docker

# 

# 1\. Build and run

# 

# docker-compose up --build

# 

# 2\. Access the API

# 

# http://localhost:5000/api/devices

# 

# 3\. Connect to SQL Server

# 

# Host: localhost

# 

# Port: 1433 (or 14333 if changed)

# 

# User: user

# 

# Password: password

# 

# Database: DevicesDb

# 

# 4\. Run tests

# 

# docker-compose run tests

# 

# 🧱 Project Structure

# 

# DevicesApiSolution/

# ├── DevicesApi/               # Main API project

# │   ├── Controllers/

# │   ├── Services/

# │   ├── Validators/

# │   └── Program.cs

# ├── DevicesDomain/           # Domain and entity project

# │   ├── Entities/

# │   ├── Repositories/

# │   └── DbContext.cs

# ├── DevicesDomain.Tests/     # Test project

# │   ├── Services/

# │   └── Validators/

# ├── docker-compose.yml

# └── README.md

# 

# 🧠 Tips

# 

# Use sqlserver as the host in connection strings inside Docker.

# 

# Use localhost,14333 to connect via SSMS outside Docker.

# 

# Migrations are applied automatically in Program.cs.

# 

# 🧠 Tips for improve the project

# 

# &nbsp;\* Use dapper to generic handles repository

# &nbsp;\* Use fluent validation for request

# &nbsp;\* Implement 

 

# 

&nbsp;  

# 

