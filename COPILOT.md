# GitHub Copilot Instructions for .NET 8 API Development

## Code Style & Conventions

### Naming Conventions
- **PascalCase**: Classes, methods, properties, enums, namespaces
- **camelCase**: Local variables, parameters, private fields
- **UPPER_CASE**: Constants
- **Prefix interfaces** with `I` (e.g., `IUserService`)
- **Suffix async methods** with `Async` (e.g., `GetUserAsync`)

### File Organization
- One class per file
- File name matches class name
- Use folders to group related functionality
- Place interfaces in separate files from implementations

## Architecture Patterns

### Project Structure
```
api/              # Web API layer (controllers, middleware, configuration)
domain/           # Business logic, entities, interfaces
external/         # Clients for calling external APIs
repo/             # Database access, queries
tests/            # Unit and integration tests
```

### Dependency Injection
- Register services in `Program.cs`
- Use constructor injection
- Prefer interfaces over concrete types
- Use appropriate service lifetimes (Singleton, Scoped, Transient)

### Error Handling
- Use `Result<T>` pattern or custom exception types
- Implement global exception handling middleware
- Return appropriate HTTP status codes
- Include meaningful error messages

## Code Quality Standards

### Performance
- Use `async/await` for I/O operations
- Prefer `IAsyncEnumerable<T>` for streaming data
- Use `Span<T>` and `Memory<T>` for performance-critical code
- Implement proper cancellation token support

### Security
- Validate all inputs
- Use parameterized queries (no string concatenation)
- Implement proper authentication/authorization
- Sanitize outputs to prevent XSS

### Testing
- Write unit tests for business logic
- Use integration tests for API endpoints
- Mock external dependencies
- Maintain >80% code coverage

## Build & Deployment Commands

### Development
```bash
dotnet build                    # Build solution
dotnet test                     # Run tests
dotnet run --project api        # Start API
```

### Code Quality
```bash
dotnet format                   # Format code
dotnet build --verbosity normal # Check for warnings
```

### Package Management
```bash
dotnet restore                  # Restore packages
dotnet add package <name>       # Add package
dotnet remove package <name>    # Remove package
```

## API Development Guidelines

### REST API Conventions
- **Use plural resource names** in endpoints (e.g., `/api/users` not `/api/user`)
- Controllers should be named with plural form (e.g., `UsersController`)
- Follow RESTful HTTP verbs: GET, POST, PUT, DELETE
- Use consistent URL patterns: `/api/resources/{id}`

### Minimal APIs
- Use minimal APIs for simple CRUD operations
- Group related endpoints using `MapGroup()`
- Use descriptive route names with `.WithName()`
- Implement proper HTTP status codes

### Data Transfer Objects (DTOs)
- Use DTOs for API contracts
- Keep DTOs simple and focused
- Use record types for immutable DTOs
- Implement validation attributes

### Configuration
- Use `appsettings.json` for configuration
- Implement strongly-typed configuration classes
- Use different settings per environment
- Never commit secrets to source control

## Database Guidelines

### Entity Framework Core
- Use Code First approach
- Implement proper entity configurations
- Use migrations for schema changes
- Implement soft deletes where appropriate
- Use connection resiliency patterns

### Performance
- Use `AsNoTracking()` for read-only queries
- Implement proper indexing strategy
- Use projection to select only needed fields
- Implement pagination for large datasets

## Common Patterns

### Repository Pattern
```csharp
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
```

### Service Pattern
```csharp
public interface IUserService
{
    Task<UserDto?> GetUserByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<UserDto> CreateUserAsync(CreateUserDto dto, CancellationToken cancellationToken = default);
}
```

**IMPORTANT**: Controllers should extract properties from DTOs and pass them as individual parameters to domain services. This maintains clean architecture by preventing the domain layer from depending on API layer DTOs.

**Controllers should**:
```csharp
// Extract from DTO and pass individual parameters
var user = await _userService.CreateUserAsync(
    createUserDto.AccountId,
    createUserDto.FirstName,
    createUserDto.LastName,
    createUserDto.PhoneNumber,
    createUserDto.UserRole,
    cancellationToken);
```

**Domain services use individual parameters**:
```csharp
Task<User> CreateUserAsync(Guid accountId, string firstName, string lastName, string phoneNumber, UserRole userRole, CancellationToken cancellationToken = default);
```

## Notes for GitHub Copilot
- Always check existing code patterns before implementing new features
- Prefer editing existing files over creating new ones
- Follow the established project structure
- Run `dotnet build` and `dotnet test` after making changes
- Use existing NuGet packages already referenced in the project
