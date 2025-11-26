using DiplomaDictionary.Data;
using DiplomaDictionary.Domain.Services;
using DiplomaDictionary.Extensions;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddApplicationDbContext(builder.Configuration);
// IUnitOfWork
builder.Services.AddScoped<IUnitOfWork, ApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
// Services
builder.Services.AddScoped<IConceptService, ConceptService>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Diploma Dictionary API", Version = "v1" });
});

var app = builder.Build();

await app.ApplyMigrationsAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();