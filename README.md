# .NET Core Dependency Injection Lifecycle Demo

A practical demonstration of:
- Singleton lifecycle
- Scoped lifecycle
- Transient lifecycle
- Captive dependency detection
- Safe scope resolution patterns

## Run Instructions
1. Clone repo
2. Open in Visual Studio
3. Build & Run
4. Observe console output

## Key Takeaways
- Use `ValidateScopes` during development
- Prefer `IServiceScopeFactory` over direct scoped dependencies in Singletons
- Profile memory for long-running apps
