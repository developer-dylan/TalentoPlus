# TalentoPlus

TalentoPlus is a web application designed to help Human Resources (HR) teams manage employees efficiently. It allows you to organize staff data, generate professional CVs, and answer questions about your workforce using Artificial Intelligence.

## Features

*   **Employee Management**: You can add, edit, and delete employee information.
*   **Excel Import**: Easily upload a list of employees from an Excel file to populate the database.
*   **Professional CV**: Generate, view, and print a professional-looking Resume/CV for any employee.
*   **AI Assistant**: Ask questions like "How many employees are active?" or "Who is the manager?" and get real answers powered by Google Gemini AI.
*   **Dashboard**: View quick statistics and charts about your team.
*   **Secure Access**: Admin login is required to manage sensitive data.

## Technologies Used

This project was built using modern tools:
*   **C# & ASP.NET Core MVC**: The main framework for the web application.
*   **PostgreSQL**: The database used to store all employee data.
*   **Entity Framework Core**: To manage database operations.
*   **Google Gemini API**: The Artificial Intelligence engine.
*   **Bootstrap 5**: For a responsive and beautiful user interface.
*   **EPPlus**: To read and process Excel files.

## How to Run the Project

### Prerequisites
Before you start, make sure you have installed:
1.  **.NET SDK** (Version 8.0 or newer).
2.  **PostgreSQL** (Database server).

### Installation Steps

1.  **Download the Code**:
    Clone this repository or download the files to your computer.

2.  **Configure the Database**:
    *   Open the file `TalentoPlus.Web/appsettings.json`.
    *   Find the `ConnectionStrings` section.
    *   Update the connection string with your PostgreSQL username and password.
    *   Example: `"Host=localhost;Database=TalentoDB;Username=postgres;Password=Qwe.123*"`

3.  **Configure AI (Optional)**:
    *   In the same `appsettings.json` file, find the `Gemini` section.
    *   Add your API Key: `"ApiKey": "AIzaSyCs1KKm1KSOENvfoScCEXRBPYccrZ102eE"`.

4.  **Run the Application**:
    *   Open your terminal or command prompt.
    *   Navigate to the project folder.
    *   Run the following command to apply database changes:
        ```bash
        dotnet ef database update --project TalentoPlus.Web
        ```
    *   Start the app:
        ```bash
        dotnet run --project TalentoPlus.Web
        ```

5.  **Open in Browser**:
    *   The terminal will show a URL (usually `http://localhost:5079`).
    *   Open that link in your web browser.

## How to Run with Docker (Recommended)

If you have Docker installed, you can run the entire application (App + Database) with a single command:

1.  **Open Terminal** in the project root.
2.  **Run Docker Compose**:
    ```bash
    docker compose up --build
    ```
3.  **Access the App**:
    *   Open `http://localhost:5000` in your browser.
    *   The database will be automatically created and seeded.

## How to Use

1.  **Login**: Use the default admin credentials (e.g., `admin@talento.com` / `Admin123!`) if seeded, or register a new user.
2.  **Import Data**: Go to the **"Importar Excel"** menu to upload your employee data file.
3.  **Manage Employees**: Go to **"Empleados"** to see the list.
    *   Click the **Eye Icon** to view the Professional CV.
    *   Click **"Imprimir CV"** to print it or save as PDF.
4.  **Ask the AI**: Go to the **"Tablero"** (Dashboard) and type a question in the chat box. The AI will analyze your data and answer in Spanish.

## REST API

This project includes a complete REST API that can be consumed by the web application or external clients.

### API Endpoints

**Public Endpoints (No Authentication Required):**

*   `GET /api/departamentos` - List all departments
*   `POST /api/auth/login` - Login and get JWT token
    ```json
    {
      "email": "admin@talento.com",
      "password": "Admin123."
    }
    ```
*   `POST /api/empleados/autoregistro` - Employee self-registration
    ```json
    {
      "firstName": "John",
      "lastName": "Doe",
      "email": "john@example.com",
      "password": "Password123!",
      "jobTitle": "Developer",
      "departmentId": 1
    }
    ```

**Protected Endpoints (Require JWT Token):**

*   `GET /api/empleados/me` - Get current employee data
*   `GET /api/empleados/me/cv` - Download employee CV as PDF

### Using the API

1.  **Start the API**:
    ```bash
    dotnet run --project TalentoPlus.Api
    ```
    Or with Docker:
    ```bash
    docker compose up talentoplus.api
    ```

2.  **Access Swagger UI**: Open `http://localhost:5001/swagger` to test the API interactively.

3.  **Authentication**: 
    *   Call `/api/auth/login` to get a JWT token
    *   Include the token in subsequent requests: `Authorization: Bearer {your-token}`

### Running Tests

The project includes unit and integration tests:

```bash
dotnet test
```

---
*Created for the TalentoPlus Project.*
