using CloudinaryDotNet;
using GymApp.Configuration;
using GymApp.Data;
using GymApp.Repositorio;
using GymApp.Repositorio.Interface;
using GymApp.Servico;
using GymApp.Servico.Provedores;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var cloudinaryUrl = builder.Configuration["CLOUDINARY_URL"];

if (!string.IsNullOrEmpty(cloudinaryUrl))
{
    var cloudinary = new Cloudinary(cloudinaryUrl);
    builder.Services.AddSingleton(cloudinary);

    builder.Services.AddScoped<IImageStorageService, CloudinaryImageService>();
    Console.WriteLine("CLOUDINARY INICIADA");
}
// 1) Logging
builder.Host.UseGymAppLogging();

// 2) Telemetria
builder.Services.AddGymAppTelemetry();

var myAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<GymAppDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddScoped<IExercicioRepository, ExercicioRepository>();
builder.Services.AddScoped<ITreinoRepository, TreinoRepository>();
builder.Services.AddScoped<CreateExercicioHandler>();
builder.Services.AddScoped<CreateTreinoHandler>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseExceptionMiddleware();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
