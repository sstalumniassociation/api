using Api.Services.V1;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder => tracerProviderBuilder
        .AddAspNetCoreInstrumentation()
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

var app = builder.Build();

app.MapGrpcService<UserServiceV1>();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SST Alumni Association API v1");
});

app.Run();
