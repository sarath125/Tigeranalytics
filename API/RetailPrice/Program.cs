using AspNetCoreRateLimit;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using RetailPrice.Business.PricingService;
using RetailPrice.Business.ProductCategoryService;
using RetailPrice.Business.ProductService;
using RetailPrice.Business.StoreService;
using RetailPrice.Models;
using RetailPrice.Profiles;

var builder = WebApplication.CreateBuilder(args);

// Services Configuration
// -------------------------------------------------

// AutoMapper Configuration
builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

// Controllers Configuration
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null; // Preserve casing
});

// Scoped Services for Business Logic
builder.Services.AddScoped<IStoreService, StoreService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();
builder.Services.AddScoped<IPricingService, PricingService>();

// Database Context Configuration
builder.Services.AddDbContext<RetailPriceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS Configuration
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins(allowedOrigins)
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

// Hangfire Configuration
builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();

// Swagger Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Caching and Rate Limiting Configuration
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// Application Pipeline Configuration
// -------------------------------------------------

var app = builder.Build();

// CORS Middleware
app.UseCors("AllowSpecificOrigin");

// Development Environment Configuration
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Rate Limiting Middleware
app.UseIpRateLimiting();

// Authentication Middleware (if needed)
app.UseAuthentication(); // Include this if authentication is part of your project

// Authorization Middleware
app.UseAuthorization();

// Map Controllers
app.MapControllers();

// Hangfire Dashboard
app.UseHangfireDashboard();

// Run the Application
app.Run();
