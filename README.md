# Enterprise-Workflow-Platform
A platform for managing users, tasks, and processes with event-driven architecture.

## Running the application

### Using Docker (Production Environment)

This is the recommended way to run the application in a production-like environment.

**Prerequisites:**
- Docker Desktop

**To run:**
1. Open a terminal in the root of the project.
2. Run the command:
   ```bash
   docker-compose up -d --build
   ```
   This will build the images for the services and start all the containers in detached mode.

The gateway will be available at http://localhost:5130.

### Locally (Development Environment)

This method is for local development and debugging. You will need to start each service individually.

**Prerequisites:**
- .NET 9 SDK
- A running PostgreSQL instance. The application is pre-configured to connect to a PostgreSQL database on `localhost:5432` with username `postgres`, password `postgres`, and database `bonussystem`.
- The easiest way to start a compatible database is to use the one defined in the `docker-compose.yml` file. Run the following command in the root of the project:
  ```bash
  docker-compose up postgres-master
  ```

**To run:**
1. **Start Auth Service:**
   - Navigate to `src/auth.service/auth.api`
   - Run `dotnet run`
   - The service will be available at `http://localhost:5006`

2. **Start Users Service:**
   - Navigate to `src/users.service/users.api`
   - Run `dotnet run`
   - The service will be available at `http://localhost:5152`

3. **Start Gateway:**
   - Navigate to `src/gateway`
   - Run `dotnet run`
   - The gateway will be available at `http://localhost:5130`

The gateway will automatically route requests to the running services.