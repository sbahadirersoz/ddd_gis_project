# GIS API (DDD / Clean Architecture)

A .NET 9 REST API for managing geospatial data (points of interest and polygons), built to practice Domain-Driven Design, Clean Architecture and containerized development.

> **Status:** work in progress. See the [roadmap](#roadmap).

## Tech stack

| Area | Technology |
|---|---|
| Framework | .NET 9, ASP.NET Core (controllers) |
| Architecture | DDD, Clean Architecture, CQRS-style handlers |
| Messaging / pipeline | MediatR with logging and validation behaviors |
| Validation | FluentValidation |
| Persistence | EF Core, Npgsql, NetTopologySuite |
| Database | PostgreSQL + PostGIS |
| Error handling | Result pattern in the Domain layer |
| Containers | Docker, Docker Compose |

## Architecture

```
┌──────────────────────────────┐
│ gis.API                      │  Controllers, composition root
└──────────────┬───────────────┘
               │ depends on
┌──────────────▼───────────────┐
│ Gis.Application              │  Use cases (MediatR), validators, pipeline behaviors
└──────────────┬───────────────┘
               │ depends on
┌──────────────▼───────────────┐
│ gis.Domain                   │  Entities, value objects, business rules, Result pattern
└──────────────▲───────────────┘
               │ implements Domain/Application interfaces
┌──────────────┴───────────────┐
│ Gis.Infrastructure           │  EF Core, repositories, PostGIS access
└──────────────────────────────┘
```

Dependency rule: the Domain depends on nothing. Application depends on Domain. Infrastructure implements the interfaces defined by the inner layers. The API wires everything together.

### Project structure

```
ddd_gis_project/
├── gis.API/               # Web API, controllers, Program.cs, Dockerfile
├── Gis.Application/       # Features (commands/queries), validators, behaviors
├── Gis.Infrastructure/    # DbContext, repositories, migrations, DI extensions
├── gis.Domain/            # Entities, business rules, domain services, Result<T>
├── compose.yaml           # API + PostGIS database
└── .env.example           # Template for local configuration
```

### Features

- **Points of interest (POI):** list and manage points on the map
- **Polygons:** create polygons and add holes, validated by domain business rules (rules factory) and NetTopologySuite geometry checks
- Request validation and logging through MediatR pipeline behaviors
- Generic repository for common data access

## Getting started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- `dotnet-ef` tool: `dotnet tool install --global dotnet-ef`

### 1. Configure

Copy the example file and set your own values:

```bash
cp .env.example .env
```

| Variable | Description |
|---|---|
| `DB_NAME` | Database name (use lowercase, for example `gis_ddd_database`) |
| `DB_USER` | Database user |
| `DB_PASSWORD` | Database password |
| `DEFAULT_CONNECTION` | Connection string for tools on your machine (`Host=localhost`) |

`.env` is ignored by git. Never commit real credentials.

### 2. Run with Docker

```bash
docker compose up --build
```

The API is available at `http://localhost:8080`.

Inside Docker the API reaches the database at host `db`. On your own machine it uses `localhost`. Compose sets the right connection string for the container.

### 3. Apply database migrations

With the database container running:

```bash
dotnet ef database update --project Gis.Infrastructure --startup-project gis.API
```

`dotnet ef` does not read `.env`. Export `DEFAULT_CONNECTION` in your shell first, or use [user secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets):

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<connection string>" --project gis.API
```

### Run locally without Docker for the API

```bash
docker compose up db -d
dotnet run --project gis.API
```

## API

| Method | Route | Description |
|---|---|---|
| GET | `/api/pois` | List points of interest |
| POST | `/api/polygons` | Create a polygon |
| POST | `/api/polygons/{id}/holes` | Add a hole to a polygon |

> Adjust the routes above to match your controllers. In Development, the OpenAPI document is served at `/openapi/v1.json`.

## Development notes

- Run the API in `Development` to validate the dependency injection graph at startup. Missing registrations fail fast with a clear error.
- Migrations live in `Gis.Infrastructure`. Commit them, and never edit a migration that has been applied elsewhere.
- Use lowercase snake_case for database names. Postgres treats quoted mixed-case names as different identifiers.
- Postgres container variables (`POSTGRES_*`) only apply on the first initialization of the data volume. To reset a development database: `docker compose down -v`.

## Roadmap

- [x] Layered solution with DDD structure
- [x] Dockerfile and Docker Compose with PostGIS
- [x] MediatR pipeline with validation and logging
- [ ] Initial EF Core migration and automatic migration in Development
- [ ] Seed data
- [ ] Unit tests (Domain, Application)
- [ ] Integration tests with Testcontainers
- [ ] Spatial queries (features within a radius, intersection)
- [ ] Global exception handling with `ProblemDetails`
- [ ] Authentication (JWT)
- [ ] GitHub Actions CI (build and test)

## What I learned

- Structuring a .NET solution with Clean Architecture and enforcing the dependency rule
- Writing a multi-stage Dockerfile for a multi-project solution
- Dependency injection troubleshooting (missing registrations, base type vs derived type)
- Docker networking (service names vs `localhost`) and Postgres initialization behavior
- EF Core migrations with spatial types

## License

Add a license file (for example MIT) before publishing.
