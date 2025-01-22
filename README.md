# API de ToDo

La API de ToDo es una aplicación web RESTful desarrollada con .NET 8, siguiendo principios de arquitectura limpia. Proporciona endpoints para gestionar tareas, incluyendo creación, consulta, actualización y eliminación de tareas, junto con autenticación de usuarios mediante JWT.

---

## Características

- **Gestión de Tareas**:
  - Crear, actualizar, eliminar y consultar tareas.
  - Filtrar tareas por descripción y estado de completado.
- **Autenticación**:
  - Autenticación de usuarios con JWT.
  - Endpoints protegidos que requieren autenticación.
- **Manejo de Errores**:
  - Gestión robusta de errores para la base de datos y excepciones generales.
- **Pruebas**:
  - Pruebas unitarias para controladores y servicios.
- **Documentación Swagger**:
  - Documentación de la API con pruebas interactivas.

---

## Estructura del Proyecto

- `Controllers`: Maneja las solicitudes y respuestas HTTP.
  - `AuthenticationController`: Gestiona la autenticación de usuarios utilizando JWT.
  - `TasksController`: Gestiona las operaciones relacionadas con las tareas.
- `Services`: Contiene la lógica de negocio (por ejemplo, `TaskServiceRepository`, `TokenService`).
- `Infrastructure`:
  - `AppDbContext`: Contexto de base de datos usando Entity Framework Core.
  - Modelos de datos y DTOs.
- `Tests`: Pruebas unitarias utilizando xUnit y Moq.

---

## Requisitos

- **SDKs**:
  - .NET SDK 8.0
- **Base de Datos**:
  - SQL Server (mediante contenedor Docker).
- **Herramientas**:
  - Docker y Docker Compose
  - Visual Studio o cualquier IDE compatible con .NET 8.0

---

## Instalación y Configuración

### Clonar el Repositorio

```bash
git clone <url-del-repositorio>
cd <carpeta-del-repositorio>

## Configurar Variables de Entorno

Crea un archivo `appsettings.json` o utiliza el existente. Configura las opciones de JWT y la conexión a la base de datos:

```json
{
  "JwtSettings": {
    "Issuer": "TaskToDoAPI",
    "Audience": "TaskToDoUsers",
    "SecretKey": "YourSuperSecretKey1234567890"
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ToDoDb;User Id=sa;Password=YourPassword123;"
  }
}
## Instrucciones de Docker

### Dockerfile

El `Dockerfile` se utiliza para construir la imagen de la API. Contiene las siguientes etapas:

1. **Restauración de Dependencias**: Copia el archivo del proyecto y restaura las dependencias.
2. **Construcción**: Compila la aplicación en modo `Release`.
3. **Publicación**: Genera los archivos necesarios para ejecutar la aplicación.
4. **Imagen Final**: Usa la imagen base de `mcr.microsoft.com/dotnet/aspnet:8.0` y copia los archivos publicados para ejecutar la API.

---

### docker-compose.yml

El archivo `docker-compose.yml` configura dos servicios:

- **`api`**: Servicio de la API que expone el puerto `5000`.
- **`db`**: Base de datos SQL Server que expone el puerto `1433`.

---

### Ejecutar con Docker

1. Asegúrate de que Docker esté ejecutándose.
2. Construye y ejecuta los contenedores con el siguiente comando:

```bash
docker-compose up -d --build

## Servicios

### API
- Disponible en: [http://localhost:5000](http://localhost:5000)

### Swagger UI
- Documentación interactiva disponible en: [http://localhost:5000/swagger/index.html](http://localhost:5000/swagger/index.html)

---

## Parar los Contenedores

Para detener y eliminar los contenedores, usa el siguiente comando:

```bash
docker-compose down

## Ejecutar Localmente

### Restaurar Dependencias

Ejecuta el siguiente comando para restaurar las dependencias del proyecto:

```bash
dotnet restore

### Aplicar Migraciones

Aplica las migraciones de la base de datos con el siguiente comando:

```bash
dotnet ef database update --project ToDo.Infrastructure --startup-project ToDo.API

## Ejecutar la Aplicación

Ejecuta el proyecto localmente con el siguiente comando:

```bash
dotnet run --project ToDo.API

## Acceder a la API

- **Documentación Swagger**: [http://localhost:5000/swagger/index.html](http://localhost:5000/swagger/index.html)

## Pruebas

### Ejecutar Pruebas Unitarias

El proyecto incluye pruebas unitarias para controladores y servicios. Usa el siguiente comando para ejecutarlas:

```bash
dotnet test
## Endpoints de la API

### Autenticación

| Método | Endpoint                   | Descripción              |
|--------|----------------------------|--------------------------|
| POST   | `/api/authentication/login` | Iniciar sesión y obtener un JWT. |

#### Cuerpo de la Solicitud:

```json
{
  "username": "admin",
  "password": "password"
}

### Gestión de Tareas

| Método | Endpoint                  | Descripción                      |
|--------|---------------------------|----------------------------------|
| GET    | `/api/tasks`              | Consultar todas las tareas.      |
| GET    | `/api/tasks/{id}`         | Consultar una tarea por ID.      |
| POST   | `/api/tasks`              | Crear una nueva tarea.           |
| PUT    | `/api/tasks/{id}`         | Actualizar una tarea existente.  |
| DELETE | `/api/tasks/{id}`         | Eliminar una tarea.              |
| PATCH  | `/api/tasks/{id}/complete` | Marcar una tarea como completada. |

## Estructura del Proyecto

### /ToDo.API

- **/Controllers**
  - `AuthenticationController.cs`
  - `TasksController.cs`
- **/Infrastructure**
  - `AppDbContext.cs`
  - `Models/`
- **/Services**
  - `TaskServiceRepository.cs`
  - `TokenService.cs`
- **/Tests**
  - `AuthControllerTests.cs`
  - `TasksControllerTests.cs`
  - `TaskServiceTests.cs`




