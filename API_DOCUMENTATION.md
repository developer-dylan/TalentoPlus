# TalentoPlus API - Documentación Completa

## Descripción

API REST completa para el sistema de gestión de recursos humanos TalentoPlus. Construida con ASP.NET Core 8.0, utiliza JWT para autenticación y PostgreSQL como base de datos.

## Características Principales

- ✅ **Autenticación JWT**: Seguridad basada en tokens
- ✅ **Roles**: Admin y Employee
- ✅ **Autoregistro de Empleados**: Endpoint público para registro
- ✅ **Generación de CV en PDF**: Descarga profesional de hojas de vida
- ✅ **Envío de Emails**: Notificaciones SMTP reales
- ✅ **Patrón Repository**: Arquitectura limpia y mantenible
- ✅ **Pruebas Automatizadas**: Unit tests e Integration tests
- ✅ **Docker Ready**: Completamente dockerizado
- ✅ **Swagger UI**: Documentación interactiva

## Endpoints Disponibles

### 🔓 Públicos (Sin autenticación)

#### 1. Listar Departamentos
```http
GET /api/departamentos
```
**Respuesta:**
```json
[
  {
    "id": 1,
    "name": "Tecnología"
  },
  {
    "id": 2,
    "name": "Marketing"
  }
]
```

#### 2. Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "admin@talento.com",
  "password": "Admin123."
}
```
**Respuesta:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

#### 3. Autoregistro de Empleado
```http
POST /api/empleados/autoregistro
Content-Type: application/json

{
  "firstName": "Juan",
  "lastName": "Pérez",
  "email": "juan.perez@empresa.com",
  "password": "Password123!",
  "jobTitle": "Desarrollador",
  "departmentId": 1
}
```
**Respuesta:**
```json
{
  "message": "Registro exitoso. Se ha enviado un correo de bienvenida."
}
```

### 🔒 Protegidos (Requieren JWT)

#### 4. Obtener Datos del Empleado Autenticado
```http
GET /api/empleados/me
Authorization: Bearer {token}
```
**Respuesta:**
```json
{
  "id": 5,
  "fullName": "Juan Pérez",
  "email": "juan.perez@empresa.com",
  "jobTitle": "Desarrollador",
  "department": "Tecnología",
  "status": "Active",
  "phone": "555-1234",
  "address": "Calle 123",
  "salary": 5000.00
}
```

#### 5. Descargar CV en PDF
```http
GET /api/empleados/me/cv
Authorization: Bearer {token}
```
**Respuesta:** Archivo PDF descargable

## Configuración

### Variables de Entorno

Configura las siguientes variables en `appsettings.json` o como variables de entorno:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=TalentoDB;Username=postgres;Password=yourpassword"
  },
  "Jwt": {
    "Secret": "SuperSecretKey1234567890_ChangeMeInProd"
  },
  "Smtp": {
    "Host": "smtp.gmail.com",
    "Port": "587",
    "User": "your-email@gmail.com",
    "Password": "your-app-password"
  }
}
```

### Ejecutar Localmente

```bash
cd TalentoPlus.Api
dotnet run
```

La API estará disponible en: `http://localhost:5001`

### Ejecutar con Docker

```bash
docker compose up talentoplus.api
```

## Pruebas

### Ejecutar Todas las Pruebas
```bash
dotnet test
```

### Pruebas Incluidas

**Unit Tests:**
- `CvServiceTests`: Validación de generación de PDF
- `EmployeeEntityTests`: Validación de lógica de entidades

**Integration Tests:**
- `DepartmentsIntegrationTests`: Prueba del endpoint de departamentos con base de datos en memoria

## Arquitectura

```
TalentoPlus.Api/
├── Controllers/          # Endpoints de la API
├── DTOs/                 # Data Transfer Objects
├── Repositories/         # Acceso a datos
│   ├── Interfaces/
│   └── Implementations/
├── Services/             # Lógica de negocio
│   ├── Interfaces/
│   └── Implementations/
└── Program.cs            # Configuración de la aplicación

TalentoPlus.Data/         # Capa compartida de datos
├── Entities/             # Modelos de base de datos
└── AppDbContext.cs       # Contexto de EF Core

TalentoPlus.Tests/        # Pruebas automatizadas
├── UnitTests/
└── IntegrationTests/
```

## Seguridad

- **JWT**: Tokens con expiración de 7 días
- **HTTPS**: Redirección automática
- **Passwords**: Validación con Identity (mínimo 6 caracteres, mayúsculas, minúsculas, números)
- **Roles**: Separación entre Admin y Employee
- **CORS**: Configurar según necesidades del cliente

## Uso desde el Frontend

### Ejemplo con JavaScript (Fetch API)

```javascript
// Login
const loginResponse = await fetch('http://localhost:5001/api/auth/login', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({
    email: 'user@example.com',
    password: 'Password123!'
  })
});

const { token } = await loginResponse.json();

// Guardar token
localStorage.setItem('token', token);

// Obtener datos del empleado
const employeeResponse = await fetch('http://localhost:5001/api/empleados/me', {
  headers: {
    'Authorization': `Bearer ${token}`
  }
});

const employeeData = await employeeResponse.json();
console.log(employeeData);
```

## Swagger UI

Accede a la documentación interactiva en:
```
http://localhost:5001/swagger
```

Desde Swagger puedes:
- Ver todos los endpoints
- Probar las llamadas directamente
- Autenticarte con JWT
- Ver los modelos de datos

## Troubleshooting

### Error: "No existe la relación Employees"
Ejecuta las migraciones:
```bash
dotnet ef database update --project TalentoPlus.Api
```

### Error: "Failed to send email"
Verifica la configuración SMTP en `appsettings.json`. Para Gmail, necesitas una "App Password".

### Error: "Unauthorized"
Asegúrate de incluir el header `Authorization: Bearer {token}` en las peticiones protegidas.

## Licencia

Este proyecto es parte del sistema TalentoPlus.
