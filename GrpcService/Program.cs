using DiplomaDictionary.Data;
using DiplomaDictionary.Domain.Services;
using DiplomaDictionary.Extensions;
using GrpcService.Services;

// указал тут ссылку на WebApi проект, чтобы не делать shared project для экстеншн методов

var builder = WebApplication.CreateBuilder(args);
// Далее аналогично Web Api программе настраиваем DI и так далее...
// DbContext
builder.Services.AddApplicationDbContext(builder.Configuration);
// IUnitOfWork
builder.Services.AddScoped<IUnitOfWork, ApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
// Services
builder.Services.AddScoped<IConceptService, ConceptService>();

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

await app.ApplyMigrationsAsync();

app.MapGrpcService<ConceptGrpcService>();

// Configure the HTTP request pipeline.
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();