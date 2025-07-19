using CloudinaryDotNet;
using GymApp.Configuration;
using GymApp.Configuration.Exceptions;
using GymApp.Data;
using GymApp.Repositorio;
using GymApp.Repositorio.Interface;
using GymApp.Servico.ExercicioHandler;
using GymApp.Servico.ExercicioHAndler;
using GymApp.Servico.Provedores;
using GymApp.Servico.TreinoHandler;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
//EXCEPTIONS SERVICES
builder.Services.AddProblemDetails(configure =>
{
    configure.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
    };
});
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

//CloudinaryConfig
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

//CORS CONFIG
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

//DATABASE CONFIGS
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<GymAppDbContext>(options =>
    options.UseNpgsql(connectionString));


//SERVICES    
builder.Services.AddScoped<IExercicioRepository, ExercicioRepository>();
builder.Services.AddScoped<ITreinoRepository, TreinoRepository>();
builder.Services.AddScoped<CreateExercicioHandler>();
builder.Services.AddScoped<CreateTreinoHandler>();
builder.Services.AddScoped<UpdateExercicioHandler>();
builder.Services.AddScoped<ExercicioQueryHandler>();
builder.Services.AddScoped<DeleteExercicioHandler>();


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
//app.UseExceptionMiddleware();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
