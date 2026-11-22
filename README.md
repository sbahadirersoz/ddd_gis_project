# GIS API (DDD / Clean Architecture)

A .NET 9 REST API for geospatial data (points of interest, lines and polygons), built to practice Domain-Driven Design, Clean Architecture and test-driven development.

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

- **Points of interest:** create, read, update, delete, plus nearest, N closest and in-range queries by id, name, WKT or lon/lat
- **Lines:** create, read, update, soft delete, plus nearest, N closest and in-range queries by id or lon/lat
- **Polygons:** create, read, rename / change status, and add holes; the shell must be closed and not self-intersecting, and every hole must lie inside the shell
- Distances in meters, computed by PostGIS on `geography` columns with GIST indexes
- Errors as RFC 7807 `ProblemDetails` (400 validation, 404 not found, 409 conflict) through a Result pattern (expected errors are values, not exceptions)
- Unique names that ignore soft-deleted rows (filtered unique index)

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

All routes start with `/api/v1`. Example requests with status assertions live in `gis.API/lines.http` and `gis.API/polygons.http` (JetBrains / VS Code REST client).

### Polygons

| Method | Route | Description |
|---|---|---|
| POST | `/polygons/create` | Create a polygon from a closed coordinate ring |
| GET | `/polygons/{id}` | Shell WKT, holes, name, description, status |
| POST | `/polygons/{id}/holes` | Add a hole (must be inside the shell) |
| PATCH | `/polygons/{id}` | Change name, description or status |

### Lines

| Method | Route | Description |
|---|---|---|
| POST | `/lines/create` | Create a line |
| GET | `/lines/{id}` | Get a line |
| PATCH | `/lines` | Update name, description, coordinates or status |
| DELETE | `/lines/{id}` | Soft delete |
| GET | `/lines/closestById`, `/lines/closestByLatLon` | Nearest line |
| GET | `/lines/closesByCount`, `/lines/closesByLatLon` | N closest lines |
| GET | `/lines/inRange`, `/lines/inRange/byLatLon` | Lines within a distance in meters |

### Points of interest

| Method | Route | Description |
|---|---|---|
| GET | `/pois` | List points of interest |
| POST | `/pois/create` | Create a point |
| GET | `/pois/{id}` | Get a point |
| PATCH | `/pois` | Update a point |
| DELETE | `/pois/{id}` | Delete a point |
| GET / POST | `/pois/closestBy{Id,Name,LatLon,Wkt}` | Nearest point |
| GET / POST | `/pois/closesBy{Count,NameCount,LatLon,WktCount}` | N closest points |
| GET / POST | `/pois/inRange`, `/pois/inRange/by{Name,LatLon,Wkt}` | Points within a distance in meters |

WKT variants are POST (the WKT goes in the body). In Development, the OpenAPI document is served at `/openapi/v1.json`.

## Testing

```bash
dotnet test Tests
```

177 unit tests (xUnit, NSubstitute, FluentAssertions) cover domain rules, handlers with the real NetTopologySuite services, validators, mappers and error-to-HTTP mapping. GitHub Actions builds and runs them on every pull request.

## Development notes

- Run the API in `Development` to validate the dependency injection graph at startup. Missing registrations fail fast with a clear error.
- Migrations live in `Gis.Infrastructure`. Commit them, and never edit a migration that has been applied elsewhere.
- Use lowercase snake_case for database names. Postgres treats quoted mixed-case names as different identifiers.
- Postgres container variables (`POSTGRES_*`) only apply on the first initialization of the data volume. To reset a development database: `docker compose down -v`.

## Roadmap

- [x] Layered solution with DDD structure
- [x] Docker Compose with PostGIS
- [x] MediatR pipeline with validation and logging
- [x] Unit tests (Domain, Application, Infrastructure)
- [x] Spatial queries (nearest, N closest, within a radius)
- [x] `ProblemDetails` errors and a global exception handler
- [x] GitHub Actions CI
- [ ] Integration tests with Testcontainers
- [ ] Polygon delete and spatial queries
- [ ] Authentication (JWT)

## What I learned

- Structuring a .NET solution with Clean Architecture and enforcing the dependency rule
- Writing a multi-stage Dockerfile for a multi-project solution
- Dependency injection troubleshooting (missing registrations, base type vs derived type)
- Docker networking (service names vs `localhost`) and Postgres initialization behavior
- EF Core migrations with spatial types
- Test-first bug fixing: a red test before every fix, then checks against the real API
- Geometry rules with NetTopologySuite (closed rings, self-intersection, holes inside a shell)
