using Api.Context;
using Api.Services.V1;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Npgsql;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder => tracerProviderBuilder
        .AddAspNetCoreInstrumentation()
        .AddNpgsql()
        .AddConsoleExporter());

builder.Services
    .AddGrpc()
    .AddJsonTranscoding();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SST Alumni Association API", Version = "v1" });

    var filePath = Path.Combine(AppContext.BaseDirectory, "Api.xml");
    c.IncludeXmlComments(filePath);
    c.IncludeGrpcXmlComments(filePath, includeControllerXmlComments: true);
});

builder.Services.AddGrpcSwagger();

builder.Services.AddNpgsql<AppDbContext>(
    builder.Configuration.GetConnectionString("Postgres")
);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

app.MapGrpcService<UserServiceV1>();

app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "SST Alumni Association API v1"); });

app.Run();