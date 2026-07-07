using Booking.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer; 
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens; 
using Microsoft.OpenApi;

using Rebus.Config;
using Rebus.Routing.TypeBased;
using System.Text; 

var builder = WebApplication.CreateBuilder(args);

// 1. Configuramos la conexión con nuestro RabbitMQ
builder.Services.AddRebus(configure => configure
    .Logging(l => l.Console())
    // Conexión a tu RabbitMQ en Docker
    .Transport(t => t.UseRabbitMqAsOneWayClient("amqp://guest:guest@ticketflow-bus:5672"))
    // Enruta este evento específico a la cola de tu Worker
    .Routing(r => r.TypeBased().MapAssemblyOf<TicketFlow.Shared.Events.ReservationConfirmedEvent>("ticketflow-notifications-queue"))
);

builder.Services.AddControllers();

// 2. Documentación Swagger con soporte para el token JWT
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
});

// 3. Configuración del Servicio de Autenticación JWT
var jwtSecretKey = builder.Configuration["Jwt:Key"] ?? "ClaveSecretaSuperSeguraDeAlMenos32BytesDeLargo!";
var keyBytes = Encoding.UTF8.GetBytes(jwtSecretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "TicketFlowBackend",
        ValidAudience = builder.Configuration["Jwt:Audience"] ?? "TicketFlowFrontend",
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ClockSkew = TimeSpan.Zero
    };
});

// Agregamos el servicio de autorización
builder.Services.AddAuthorization();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();
builder.Services.AddDbContext<BookingDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // 4. Activamos los middlewares de Swagger visual
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 5. Activamos la autenticación estrictamente ANTES de la autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();