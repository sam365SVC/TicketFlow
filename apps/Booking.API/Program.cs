using System.Text;
using Booking.API.Application.Authorization;
using Booking.API.Infrastructure.Auth;
using Booking.API.Infrastructure.Authorization;
using Booking.API.Infrastructure.ExceptionHandling;
using Booking.API.Infrastructure.Messaging;
using Booking.API.Infrastructure.Seeding;
using Booking.Domain;
using Booking.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Documentación Swagger con soporte para el token JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "TicketFlow", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        // Tipo Http + scheme bearer: Swagger antepone "Bearer " automáticamente,
        // así que en el botón Authorize basta con PEGAR el token (sin "Bearer ").
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Pega aquí tu token JWT (sin la palabra 'Bearer')."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer", document),
            []
        }
    });

    // Sumamos los comentarios /// de los controladores y DTOs a Swagger.
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

builder.Services.AddDbContext<BookingDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddBookingMessaging(builder.Configuration);
builder.Services.AddBookingAuthServices(builder.Configuration);
builder.Services.Configure<AdminSeedOptions>(builder.Configuration.GetSection(AdminSeedOptions.SectionName));

var jwtSection = builder.Configuration.GetSection(JwtOptions.SectionName);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Sin esto, .NET renombra claims cortos (ej. "role") a URIs largas de Microsoft/W3C
        // al validar el token. Lo desactivamos para que el claim quede tal cual se generó.
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["SecretKey"]!)),
            ClockSkew = TimeSpan.Zero,
            RoleClaimType = "role"
        };
    });
builder.Services.AddAuthorizationBuilder()
    .AddPolicy(AuthorizationPolicies.RequireStaff, policy => policy.Requirements.Add(new MinimumRoleRequirement(UserRole.Staff)))
    .AddPolicy(AuthorizationPolicies.RequireAdmin, policy => policy.Requirements.Add(new MinimumRoleRequirement(UserRole.Admin)));

builder.Services.AddSingleton<IAuthorizationHandler, MinimumRoleHandler>();

var app = builder.Build();

// Seed idempotente del usuario root Admin (username/password: admin/admin). Corre en cada
// arranque, incluido cada "docker compose up" - no depende de correr nada a mano.
await AdminUserSeeder.SeedAsync(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

// Activamos la autenticación estrictamente ANTES de la autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
