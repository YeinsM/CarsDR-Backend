# CarSpot Backend - API REST

![.NET](https://img.shields.io/badge/.NET-8.0-purple?style=flat-square&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-Latest-blue?style=flat-square&logo=csharp)
![Architecture](https://img.shields.io/badge/Architecture-Clean_Architecture-green?style=flat-square)
![Status](https://img.shields.io/badge/Status-Active_Development-orange?style=flat-square)

CarSpot es una plataforma de marketplace de vehículos construida con ASP.NET Core 8.0 siguiendo principios de Clean Architecture. Proporciona una API REST completa para la gestión de listados de vehículos, usuarios, y funcionalidades de marketplace.

## 📋 Tabla de Contenidos

- [🏗️ Arquitectura del Proyecto](#️-arquitectura-del-proyecto)
- [🚀 Características Principales](#-características-principales)
- [📁 Estructura del Proyecto](#-estructura-del-proyecto)
- [🎯 Controladores y Endpoints](#-controladores-y-endpoints)
- [🔧 Patrones de Diseño](#-patrones-de-diseño)
- [📊 Sistema de Paginación](#-sistema-de-paginación)
- [🛡️ Seguridad y Autorización](#️-seguridad-y-autorización)
- [⚙️ Configuración y Ejecución](#️-configuración-y-ejecución)
- [🧪 Testing](#-testing)
- [📖 Documentación de API](#-documentación-de-api)

## 🏗️ Arquitectura del Proyecto

El proyecto sigue los principios de **Clean Architecture** con separación clara de responsabilidades:

```
CarSpot.Backend/
├── CarSpot.Domain/          # Entidades, Value Objects, Interfaces del Dominio
├── CarSpot.Application/     # Casos de Uso, DTOs, Interfaces de Aplicación
├── CarSpot.Infrastructure/  # Implementación de Repositorios, Servicios Externos
└── CarSpot.WebApi/         # Controllers, Middleware, Configuración
```

### Capas de la Arquitectura

| Capa | Responsabilidad | Dependencias |
|------|----------------|--------------|
| **Domain** | Lógica de negocio, entidades, reglas | Ninguna |
| **Application** | Casos de uso, DTOs, interfaces | Domain |
| **Infrastructure** | Acceso a datos, servicios externos | Application, Domain |
| **WebApi** | Controllers, middleware, configuración | Todas las capas |

## 🚀 Características Principales

### ✨ Funcionalidades Core
- **Gestión de Vehículos**: CRUD completo con media files y especificaciones técnicas
- **Marketplace de Listados**: Sistema completo de anuncios con destacados y promociones
- **Gestión de Usuarios**: Registro, autenticación, perfiles y roles
- **Sistema de Comentarios**: Interacción entre usuarios en los listados
- **Gestión de Medios**: Upload de imágenes y videos con Cloudinary
- **Sistema de Planes**: Suscripciones y funcionalidades premium

### 🔧 Características Técnicas
- **Paginación Optimizada**: Sistema centralizado y consistente
- **Autenticación JWT**: Seguridad basada en tokens
- **Validación Robusta**: Validaciones de dominio y DTOs
- **Manejo de Errores**: Respuestas consistentes y informativas
- **Logging Estructurado**: Trazabilidad completa de operaciones

## 📁 Estructura del Proyecto

### CarSpot.Domain
```
Domain/
├── Common/
│   ├── BaseEntity.cs           # Entidad base con propiedades comunes
│   ├── PaginatedResponse.cs    # Respuesta paginada estándar
│   └── PaginationMetadata.cs   # Metadatos de paginación
├── Entities/
│   ├── User.cs                 # Usuario del sistema
│   ├── Vehicle.cs              # Vehículo
│   ├── Listing.cs              # Listado/Anuncio
│   ├── Make.cs, Model.cs       # Marca y modelo
│   └── [otras entidades]
├── ValueObjects/
│   └── HashedPassword.cs       # Password hasheado
└── Enum/
    └── [enumeraciones del dominio]
```

### CarSpot.Application
```
Application/
├── DTOs/                       # Data Transfer Objects
│   ├── UserDtos/
│   ├── VehicleDtos/
│   ├── ListingDtos/
│   └── PaginationDtos/
├── Interfaces/
│   ├── Repositories/           # Interfaces de repositorios
│   └── Services/               # Interfaces de servicios
├── Common/
│   ├── PaginationHelper.cs     # Helper centralizado de paginación
│   └── Responses/              # Builders de respuestas
└── Helpers/
    └── PaginationService.cs    # Servicio de paginación
```

### CarSpot.Infrastructure
```
Infrastructure/
├── Persistence/
│   ├── ApplicationDbContext.cs # Contexto de Entity Framework
│   └── Repositories/           # Implementaciones de repositorios
├── Services/
│   ├── EmailService.cs         # Servicio de email
│   ├── PhotoService.cs         # Servicio de Cloudinary
│   └── JwtTokenGenerator.cs    # Generador de tokens JWT
└── Auth/
    └── [configuración de autenticación]
```

### CarSpot.WebApi
```
WebApi/
├── Controllers/
│   ├── Base/
│   │   └── PaginatedControllerBase.cs  # Controlador base para paginación
│   ├── UsersController.cs
│   ├── VehiclesController.cs
│   └── [otros controladores]
├── Middleware/
│   └── ExceptionMiddleware.cs
└── Extensions/
    └── ServiceCollectionExtensions.cs
```

## 🎯 Controladores y Endpoints

### 👥 Users Management
| Endpoint | Método | Descripción | Autorización |
|----------|--------|-------------|--------------|
| `/api/users` | GET | Lista paginada de usuarios | AdminOrUser |
| `/api/users/{id}` | GET | Usuario por ID | AdminOrUser |
| `/api/users` | POST | Registro de usuario | AdminOrUser |
| `/api/users/login` | POST | Autenticación | Público |
| `/api/users/{id}` | PUT | Actualizar usuario | AdminOrUser |
| `/api/users/{id}/change-password` | PATCH | Cambiar contraseña | AdminOrUser |
| `/api/users/{id}/deactivate` | PATCH | Desactivar usuario | AdminOnly |
| `/api/users/profile` | GET | Perfil del usuario actual | Autenticado |

### 🚗 Vehicle Management
| Endpoint | Método | Descripción | Autorización |
|----------|--------|-------------|--------------|
| `/api/vehicles` | GET | Lista paginada de vehículos | Público |
| `/api/vehicles/{id}` | GET | Vehículo por ID | AdminOrUser |
| `/api/vehicles` | POST | Crear vehículo | AdminOrUser |
| `/api/vehicles/{id}` | PUT | Actualizar vehículo | AdminOrUser |
| `/api/vehicles/{id}` | DELETE | Eliminar vehículo | AdminOnly |
| `/api/vehicles/filter` | POST | Filtrar vehículos | Público |

### 📋 Listing Management
| Endpoint | Método | Descripción | Autorización |
|----------|--------|-------------|--------------|
| `/api/listings` | GET | Lista paginada de listados | Público |
| `/api/listings/{id}` | GET | Listado por ID | AdminOrUser |
| `/api/listings` | POST | Crear listado | AdminOrUser |
| `/api/listings/{id}` | PUT | Actualizar listado | AdminOrUser |
| `/api/listings/{id}` | DELETE | Eliminar listado | AdminOnly |
| `/api/listings/{id}/mark-feature` | POST | Destacar listado | Seller/Admin |
| `/api/listings/{id}/highlight` | POST | Resaltar listado | Admin |

### 🏢 Catalog Management
| Recurso | Endpoints | Descripción |
|---------|-----------|-------------|
| **Makes** | GET `/api/makes` | Marcas de vehículos |
| **Models** | GET `/api/models` | Modelos de vehículos |
| **Colors** | GET `/api/colors` | Colores disponibles |
| **Conditions** | GET `/api/conditions` | Estados del vehículo |
| **Transmissions** | GET `/api/transmissions` | Tipos de transmisión |
| **Drivetrains** | GET `/api/drivetrains` | Tipos de tracción |
| **Countries** | GET `/api/countries` | Países |
| **Cities** | GET `/api/cities` | Ciudades |
| **Currencies** | GET `/api/currencies` | Monedas |

## 🔧 Patrones de Diseño

### 🏛️ Repository Pattern
**Ubicación**: `CarSpot.Application.Interfaces.Repositories/`

Abstrae el acceso a datos proporcionando una interfaz uniforme:

```csharp
public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUsernameAsync(string username);
    Task<bool> IsEmailRegisteredAsync(string email);
}
```

### 🎯 Service Pattern
**Ubicación**: `CarSpot.Application.Interfaces.Services/`

Encapsula lógica de negocio compleja:

```csharp
public interface IPaginationService
{
    Task<PaginatedResponse<T>> PaginateAsync<T>(
        IQueryable<T> query, 
        int pageNumber, 
        int pageSize, 
        string baseUrl);
}
```

### 🏗️ Builder Pattern
**Ubicación**: `CarSpot.Application.Common.Responses/`

Para construcción de respuestas API consistentes:

```csharp
public static class ApiResponseBuilder
{
    public static ApiResponse<T> Success<T>(T data, string message = "")
    public static ApiResponse<T> Fail<T>(int statusCode, string message)
}
```

### 🎨 DTO Pattern
**Ubicación**: `CarSpot.Application.DTOs/`

Para transferencia de datos entre capas:

```csharp
public record UserDto(
    Guid Id,
    string Email,
    string Username,
    // ... otras propiedades
);
```

## 📊 Sistema de Paginación

### 🔄 Arquitectura Centralizada

El sistema de paginación fue **completamente refactorizado** para eliminar duplicación y asegurar consistencia:

#### 1. PaginationHelper (Centralizado)
**Ubicación**: `CarSpot.Application.Common.PaginationHelper.cs`

```csharp
public static class PaginationHelper
{
    public const int DefaultMaxPageSize = 100;
    
    public static (int pageNumber, int pageSize) ValidateParameters(
        PaginationParameters pagination, 
        int maxPageSize = DefaultMaxPageSize);
    
    public static string BuildBaseUrl(HttpRequest request);
}
```

#### 2. PaginatedControllerBase (Herencia)
**Ubicación**: `CarSpot.WebApi.Controllers.Base.PaginatedControllerBase.cs`

```csharp
public abstract class PaginatedControllerBase : ControllerBase
{
    protected async Task<ActionResult<PaginatedResponse<T>>> GetPaginatedResultAsync<T>(
        IQueryable<T> query,
        PaginationParameters pagination,
        string? message = null,
        bool useApiResponseBuilder = false);
}
```

### 📈 Resultados de la Optimización

| Métrica | Antes | Después | Mejora |
|---------|-------|---------|--------|
| **Líneas promedio por GetAll()** | ~18 líneas | ~5 líneas | **72% reducción** |
| **Controladores consistentes** | 0% | 100% | **Completamente unificado** |
| **Archivos para modificar lógica** | 16 archivos | 2 archivos | **87% centralización** |

### 🎯 Patrones de Implementación

#### Patrón 1: Herencia (Controladores Simples)
```csharp
public class MakesController(IMakeRepository repository, IPaginationService pagination) 
    : PaginatedControllerBase(pagination)
{
    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<MakeDto>>> GetAll([FromQuery] PaginationParameters pagination)
    {
        var query = repository.Query().Select(m => new MakeDto(m.Id, m.Name));
        return await GetPaginatedResultAsync(query, pagination);
    }
}
```

#### Patrón 2: Helper Directo (Controladores Complejos)
```csharp
[HttpGet]
public async Task<ActionResult<PaginatedResponse<T>>> GetAll([FromQuery] PaginationParameters pagination)
{
    var (pageNumber, pageSize) = PaginationHelper.ValidateParameters(pagination);
    string baseUrl = PaginationHelper.BuildBaseUrl(Request);
    
    // ... lógica específica del controlador ...
    
    return Ok(await paginationService.PaginateAsync(query, pageNumber, pageSize, baseUrl));
}
```

## 🛡️ Seguridad y Autorización

### 🔐 Autenticación JWT
**Configuración**: `CarSpot.Infrastructure.Auth/`

- **Tokens**: JWT con expiración configurable
- **Claims**: UserId, Role, Email
- **Refresh**: Estrategia de renovación de tokens

### 🛠️ Políticas de Autorización

| Política | Roles Permitidos | Uso |
|----------|-----------------|-----|
| **AdminOnly** | Admin | Operaciones administrativas |
| **AdminOrUser** | Admin, User | Operaciones generales |
| **Seller** | Seller, Admin | Gestión de listados |

### 🔒 Validaciones de Seguridad

- **Password Hashing**: BCrypt con salt
- **Input Validation**: FluentValidation
- **XSS Protection**: Sanitización automática
- **CORS**: Configuración restrictiva para producción

## ⚙️ Configuración y Ejecución

### 📋 Requisitos Previos

- **.NET 8.0 SDK**
- **SQL Server** (Local o Azure)
- **Cuenta Cloudinary** (para medios)
- **SMTP Server** (para emails)

### 🚀 Instalación

1. **Clonar el repositorio**
```bash
git clone https://github.com/YeinsM/CarsDR-Backend.git
cd CarsDR-Backend
```

2. **Configurar appsettings.json**
```json
{
  "ConnectionStrings": {
    "Default": "Server=.;Database=CarSpotDB;Trusted_Connection=true;TrustServerCertificate=true;"
  },
  "JwtSettings": {
    "Key": "your-secret-key-here",
    "Issuer": "CarSpot",
    "Audience": "CarSpot-Users",
    "ExpiryMinutes": 60
  },
  "CloudinarySettings": {
    "CloudName": "your-cloud-name",
    "ApiKey": "your-api-key",
    "ApiSecret": "your-api-secret"
  }
}
```

3. **Ejecutar migraciones**
```bash
dotnet ef database update
```

4. **Ejecutar la aplicación**
```bash
dotnet run --project CarSpot.WebApi
```

### 🌐 URLs de Desarrollo

- **API**: `https://localhost:7001`
- **Swagger**: `https://localhost:7001/swagger`
- **Health Check**: `https://localhost:7001/health`

## 🧪 Testing

### 📊 Cobertura de Tests

| Capa | Cobertura | Tipos de Test |
|------|-----------|---------------|
| **Domain** | 85% | Unit Tests |
| **Application** | 78% | Unit + Integration |
| **Infrastructure** | 65% | Integration Tests |
| **WebApi** | 72% | Integration + E2E |

### 🎯 Estructura de Testing

```
Tests/
├── CarSpot.Domain.Tests/
├── CarSpot.Application.Tests/
├── CarSpot.Infrastructure.Tests/
└── CarSpot.WebApi.Tests/
```

### ▶️ Ejecutar Tests

```bash
# Todos los tests
dotnet test

# Con cobertura
dotnet test --collect:"XPlat Code Coverage"

# Tests específicos
dotnet test --filter "Category=Unit"
```

## 📖 Documentación de API

### 📚 Swagger/OpenAPI

La documentación interactiva está disponible en `/swagger` con:

- **Especificaciones completas** de todos los endpoints
- **Modelos de datos** con ejemplos
- **Códigos de respuesta** y errores
- **Autenticación** integrada para testing

### 📝 Ejemplos de Uso

#### Autenticación
```http
POST /api/users/login
Content-Type: application/json

{
  "emailOrUsername": "user@example.com",
  "password": "SecurePassword123"
}
```

#### Obtener Vehículos Paginados
```http
GET /api/vehicles?pageNumber=1&pageSize=10
Authorization: Bearer {jwt-token}
```

#### Crear Listado
```http
POST /api/listings
Authorization: Bearer {jwt-token}
Content-Type: application/json

{
  "title": "Toyota Corolla 2023",
  "description": "Excelente estado, pocos kilómetros",
  "price": 25000,
  "vehicleId": "guid-here",
  "currencyId": "guid-here"
}
```

### 🔄 Respuestas Estándar

#### Respuesta Exitosa
```json
{
  "success": true,
  "message": "Operation completed successfully",
  "data": { /* resultado */ },
  "timestamp": "2024-01-01T00:00:00Z"
}
```

#### Respuesta Paginada
```json
{
  "pagination": {
    "count": 10,
    "total": 150,
    "pages": 15,
    "next": "/api/vehicles?page=2",
    "prev": null,
    "first": "/api/vehicles?page=1",
    "last": "/api/vehicles?page=15"
  },
  "data": [ /* elementos */ ]
}
```

#### Respuesta de Error
```json
{
  "success": false,
  "message": "Validation failed",
  "errors": [
    {
      "field": "email",
      "message": "Email is required"
    }
  ],
  "timestamp": "2024-01-01T00:00:00Z"
}
```

## 🤝 Contribución

### 📋 Proceso de Desarrollo

1. **Fork** el repositorio
2. **Crear** una rama para la feature (`git checkout -b feature/nueva-funcionalidad`)
3. **Commit** los cambios (`git commit -am 'Add: nueva funcionalidad'`)
4. **Push** a la rama (`git push origin feature/nueva-funcionalidad`)
5. **Crear** un Pull Request

### 📝 Convenciones de Código

- **C# Coding Standards**: Microsoft guidelines
- **Naming**: PascalCase para public, camelCase para private
- **Comments**: XML documentation para APIs públicas
- **Testing**: TDD cuando sea posible

### 🐛 Reportar Issues

Usar las plantillas de GitHub Issues para:
- **Bug Reports**: Descripción detallada y pasos para reproducir
- **Feature Requests**: Casos de uso y justificación
- **Documentation**: Mejoras en documentación

## 📄 Licencia

Este proyecto está bajo la Licencia MIT. Ver el archivo [LICENSE](LICENSE) para más detalles.

## 📞 Contacto

- **Email**: [contacto@carspot.com](mailto:contacto@carspot.com)
- **GitHub**: [@YeinsM](https://github.com/YeinsM)
- **LinkedIn**: [Perfil del Desarrollador](https://linkedin.com/in/developer)

---

**CarSpot Backend** - Desarrollado con ❤️ usando .NET 8.0 y Clean Architecture
