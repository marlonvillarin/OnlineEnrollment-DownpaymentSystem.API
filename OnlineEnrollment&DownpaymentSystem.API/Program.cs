using OnlineEnrollment_DownpaymentSystem.API.Class;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ILoginRepository, LoginClass>();
builder.Services.AddScoped<IStudentRepository, StudentClass>();
builder.Services.AddScoped<INotificationRepository, NotificationClass>();
builder.Services.AddScoped<IValidationRepository, ValidationClass>();
builder.Services.AddScoped<IGradesRepository, GradesClass>(); 
builder.Services.AddScoped<IPaymentRepository, PaymentClass>();
builder.Services.AddScoped<IStudentDocumentRepository, DocumentClass>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentClass>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



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

app.Run();
