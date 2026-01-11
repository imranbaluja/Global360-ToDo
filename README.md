## API Documentation (Swagger/OpenAPI)

Interactive API documentation is available via Swagger UI when the backend is running.

- Visit: `http://localhost:5186/swagger` in your browser
- Use this interface to explore and test API endpoints

---

# TODO Sample App

## Overview

This is a simple TODO list application featuring an Angular frontend and an ASP.NET Core Web API backend. The backend uses an in-memory store for demonstration purposes. The app allows users to view, add, and delete TODO items.

## Tech Stack

- **Frontend:** Angular 20.x
- **Backend:** ASP.NET Core 10 Web API
- **Testing:** xUnit (backend)

---

## Summary

Simple TODO list app with Angular frontend and ASP.NET Web API backend (in-memory store). Built to satisfy a short test: view, add and delete items.

## Features

- View, add, and delete TODO items
- In-memory backend storage (no database required)
- Basic backend unit tests
- CORS enabled for local development

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js & npm](https://nodejs.org/)

### Backend Setup

1. Navigate to `backend/TodoApi`.
2. Run `dotnet restore` to install dependencies.
3. Start the API:
   ```sh
   dotnet run --urls=http://localhost:5186
   ```
   The API will be available at `http://localhost:5186/api/todos`.

### Frontend Setup

1. Navigate to `frontend`.
2. Run `npm install` to install dependencies.
3. Start the Angular app:
   ```sh
   npm start
   ```
   The app will be available at `http://localhost:4200`.

**Note:** If you change ports, update `src/environments/environment.ts` (`apiUrl`) accordingly.

---

## Running Tests

- **Backend:**
  - Unit tests are in `backend/TodoApi.Tests`.
  - Run tests with:
    ```sh
    dotnet test
    ```

---

## API Endpoints

---

# TODO Sample App

This is a simple TODO list application featuring an Angular frontend and an ASP.NET Core Web API backend. The backend uses an in-memory store for demonstration purposes. The app allows users to view, add, and delete TODO items.

---

## Table of Contents

- [Backend: API, Swagger, Config, Run, Test](#backend-api-swagger-config-run-test)
- [Frontend: Install, Config, Run, Test](#frontend-install-config-run-test)
- [API Endpoints](#api-endpoints)
- [Troubleshooting](#troubleshooting)
- [Contributing](#contributing)
- [License](#license)

---

## Backend: API, Swagger, Config, Run, Test

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Configuration

- All configuration is in `backend/TodoApi/appsettings.json` and `appsettings.Development.json`.
- The default API port is `5186`. You can change it in `Properties/launchSettings.json` or via the `--urls` flag.

### Running the Backend API

1. Open a terminal and navigate to `backend/TodoApi`.
2. Restore dependencies:
   ```sh
   dotnet restore
   ```
3. Run the API:
   ```sh
   dotnet run --urls=http://localhost:5186
   ```
   The API will be available at `http://localhost:5186/api/todos`.

### API Documentation (Swagger)

- When the backend is running, open [http://localhost:5186/swagger](http://localhost:5186/swagger) in your browser.
- Use Swagger UI to explore and test API endpoints interactively.

### Running Backend Tests

1. In a terminal, navigate to `backend/TodoApi.Tests` or `backend/TodoApi`.
2. Run:
   ```sh
   dotnet test
   ```
   This will execute all backend unit tests (xUnit).

---

## Frontend: Install, Config, Run, Test

### Prerequisites

- [Node.js & npm](https://nodejs.org/)

### Installation & Configuration

1. Open a terminal and navigate to `frontend`.
2. Install dependencies:
   ```sh
   npm install
   ```
3. If your backend runs on a different port, update `src/environments/environment.ts`:
   ```typescript
   export const environment = {
     apiUrl: "http://localhost:5186/api",
   };
   ```

### Running the Frontend

1. In the `frontend` directory, start the Angular app:
   ```sh
   npm start
   ```
   The app will be available at [http://localhost:4200](http://localhost:4200).

### Running Frontend Unit Tests

1. In the `frontend` directory, run:
   ```sh
   npm test
   ```
   This will execute all frontend unit tests using Karma and ChromeHeadless.

---

## API Endpoints

Base URL: `http://localhost:5186/api/todos`

| Method | Endpoint          | Description             |
| ------ | ----------------- | ----------------------- |
| GET    | `/api/todos`      | Get all TODO items      |
| POST   | `/api/todos`      | Add a new TODO item     |
| PUT    | `/api/todos`      | Update an existing TODO |
| DELETE | `/api/todos/{id}` | Delete a TODO item      |

### Endpoint Details

#### GET `/api/todos`

Retrieves all TODO items.

**Response:**

```json
[
  { "id": 1, "text": "First todo" },
  { "id": 2, "text": "Second todo" }
]
```

#### POST `/api/todos`

Creates a new TODO item.

**Request Body:**

```json
{ "text": "New todo" }
```

**Response:**

```json
{ "id": 3, "text": "New todo" }
```

#### PUT `/api/todos`

Updates an existing TODO item.

**Request Body:**

```json
{ "id": 1, "text": "Updated todo" }
```

**Response:**

```json
{ "id": 1, "text": "Updated todo" }
```

#### DELETE `/api/todos/{id}`

Deletes a TODO item by ID.

**Response:** `204 No Content` on success, `404 Not Found` if item doesn't exist.

---

## Troubleshooting

- **CORS errors:** Ensure backend is running and CORS is enabled (default).
- **Port issues:** Make sure frontend `apiUrl` matches backend port.
- **Data loss:** Data is stored in memory; restarting backend clears all TODOs.
- **Permissions:** If you see EACCES errors, ensure you have write permissions to the relevant directories.

---

## Contributing

Pull requests are welcome! For major changes, please open an issue first to discuss what you would like to change.

---

## License

This project is open source and available under the [MIT License](LICENSE) (add LICENSE file if needed).

---

## Notes

- No database required — data stored in memory; restarting backend clears todos.
- For production, consider persisting to a database, adding validation, authentication, and improved error handling.
