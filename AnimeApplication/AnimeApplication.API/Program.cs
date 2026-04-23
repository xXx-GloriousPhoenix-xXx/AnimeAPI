using AnimeApplication.Application;
using AnimeApplication.Infrastructure;
using Microsoft.OpenApi;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// -------- Onion layers --------
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// -------- Controllers --------
builder.Services.AddControllers();

// -------- Swagger / OpenAPI --------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo {
        Title = "Anime API",
        Version = "v1",
        Description = "API для керування аніме та вайфу"
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);
});

// -------- CORS --------
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

// -------- Middleware pipeline --------
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }