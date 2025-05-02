using DependencyInjectionDemo.Services;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register services with different lifecycles
builder.Services.AddTransient<IIdGenerator, TransientIdGenerator>();
builder.Services.AddScoped<IDatabaseContext, DatabaseContext>();
builder.Services.AddSingleton<ICacheService, SingletonCacheService>();

// Enable scope validation
builder.Services.AddOptions<ServiceProviderOptions>()
    .Configure(options => options.ValidateScopes = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// DI Lifecycle Demos
Console.WriteLine("--- Running DI Demos ---");

// Scenario 1: Basic lifecycle demo
using (var scope = app.Services.CreateScope())
{
    var transient1 = scope.ServiceProvider.GetRequiredService<IIdGenerator>();
    var transient2 = scope.ServiceProvider.GetRequiredService<IIdGenerator>();
    Console.WriteLine($"Transient IDs: {transient1.NewId} vs {transient2.NewId}");

    var scoped1 = scope.ServiceProvider.GetRequiredService<IDatabaseContext>();
    var scoped2 = scope.ServiceProvider.GetRequiredService<IDatabaseContext>();
    scoped1.AddOrder("Order1");
    Console.WriteLine($"Scoped orders count: {scoped2.GetOrders().Count}");
}

// Scenario 2: Singleton cache persists across scopes
Console.WriteLine("\n--- Singleton Cache Demo ---");
var singletonCache = app.Services.GetRequiredService<ICacheService>();
singletonCache.Add("config", "cache_value");

using (var scope = app.Services.CreateScope())
{
    var cachedValue = scope.ServiceProvider.GetRequiredService<ICacheService>().Get("config");
    Console.WriteLine($"Cached value: {cachedValue}");
}

// Scenario 3: Captive dependency demo
Console.WriteLine("\n--- Captive Dependency Demo ---");
try
{
    var invalidService = new InvalidService(app.Services.GetRequiredService<IDatabaseContext>());
    Console.WriteLine("This line won't execute");
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Correctly caught captive dependency: {ex.Message}");
}

// Scenario 4: Safe scoped resolution in singleton
Console.WriteLine("\n--- Safe Scoped Resolution in Singleton ---");
var reportService = new ReportService(app.Services.GetRequiredService<IServiceScopeFactory>());
reportService.GenerateReport();

await app.RunAsync();

// Supporting classes (must be after top-level statements)
public class InvalidService
{
    public InvalidService(IDatabaseContext dbContext) { }
}

public class ReportService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ReportService(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

    public void GenerateReport()
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IDatabaseContext>();
        dbContext.AddOrder("ReportOrder");
        Console.WriteLine($"Report orders: {dbContext.GetOrders().Count}");
    }
}