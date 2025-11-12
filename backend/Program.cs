using backend.Db;
using backend.Models;
using backend.Services;
using Microsoft.EntityFrameworkCore;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// ==========================
// 🔐 Configuración de Stripe
// ==========================
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

// ======================================
// 🗄️ Configurar EF Core con PostgreSQL
// ======================================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// =====================================
// 🌐 Configuración de CORS (Frontend)
// =====================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
        policy.WithOrigins("https://agorasoftlandingqa.ngrok.app") // 👈 dominio del frontend
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// ======================================
// 💳 Servicios relacionados con Stripe
// ======================================
builder.Services.Configure<StripeModel>(builder.Configuration.GetSection("Stripe"));
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<ChargeService>();
builder.Services.AddScoped<ProductService>();

// ======================================
// ⚙️ Inyección de dependencias propias
// ======================================
builder.Services.AddScoped<ClerkService>();
builder.Services.AddScoped<IOrganizationService, OrganizationService>();
builder.Services.AddScoped<WorkerService>();

// ======================================
// ❤️ Health Checks y Swagger
// ======================================
builder.Services.AddHealthChecks();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ======================================
// 🚀 Construcción de la aplicación
// ======================================
var app = builder.Build();

// ======================================
// 🧩 Middleware y pipeline
// ======================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ✅ Habilitar CORS antes de controladores
app.UseCors("AllowAngular");

app.UseAuthorization();

// ✅ Endpoint de verificación
app.MapHealthChecks("/health");

// ✅ Mapear controladores
app.MapControllers();

// ✅ Ejecutar aplicación
app.Run();