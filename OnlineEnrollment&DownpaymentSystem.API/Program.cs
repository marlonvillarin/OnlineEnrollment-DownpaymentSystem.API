using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnlineEnrollment_DownpaymentSystem.API.Class;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// JWT Settings from appsettings.json
var jwtSettings = builder.Configuration.GetSection("JwtSettings");

// -----------------------
// Authentication & Authorization
// -----------------------
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["SecretKey"])
        )
    };
});

builder.Services.AddAuthorization();

// -----------------------
// Dependency Injection
// -----------------------
builder.Services.AddScoped<ILoginRepository, LoginClass>();
builder.Services.AddScoped<IStudentRepository, StudentClass>();
builder.Services.AddScoped<INotificationRepository, NotificationClass>();
builder.Services.AddScoped<IValidationRepository, ValidationClass>();
builder.Services.AddScoped<IGradesRepository, GradesClass>();
builder.Services.AddScoped<IPaymentRepository, PaymentClass>();
builder.Services.AddScoped<IStudentDocumentRepository, DocumentClass>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentClass>();

// Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Enrollment API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token.\nExample: \"Bearer abc123\""
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// -----------------------
// Middleware
// -----------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // ✅ must be before authorization
app.UseAuthorization();

app.MapControllers();

app.Run();