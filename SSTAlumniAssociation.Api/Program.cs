using SSTAlumniAssociation.Api.Authorization;
using SSTAlumniAssociation.Api.Authorization.Admin;
using SSTAlumniAssociation.Api.Authorization.Member;
using SSTAlumniAssociation.Api.Authorization.OwnerOrAdmin;
using SSTAlumniAssociation.Api.Context;
using SSTAlumniAssociation.Api.Services.V1;
using Calzolari.Grpc.AspNetCore.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder => tracerProviderBuilder
        .AddNpgsql()
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation()
        .AddConsoleExporter());


builder.Services.AddTransient<IClaimsTransformation, ClaimsTransformation>();

builder.Services.AddSingleton<IAuthorizationHandler, AdminSystemAdminHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, AdminExcoHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, MemberNonRevokedHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, OwnerOrAdminHandler>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var projectId = builder.Configuration.GetValue<string>("Firebase:ProjectId");
        options.Authority = $"https://securetoken.google.com/{projectId}";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = $"https://securetoken.google.com/{projectId}",
            ValidAudience = projectId,
            ValidateIssuerSigningKey = true,
            ValidateTokenReplay = true
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(Policies.Admin, policy =>
        policy.AddRequirements(new AdminRequirement()))
    .AddPolicy(Policies.Member, policy =>
        policy.AddRequirements(new MemberRequirement()))
    .AddPolicy(Policies.OwnerOrAdmin, policy =>
        policy.AddRequirements(new OwnerOrAdminRequirement()));

builder.Services
    .AddGrpc(options => options.EnableMessageValidation())
    .AddJsonTranscoding();

builder.Services.AddValidator<CreateUserRequestValidator>();
builder.Services.AddGrpcValidation();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SST Alumni Association API", Version = "v1" });

    if (builder.Environment.IsDevelopment())
    {
        c.AddServer(new OpenApiServer { Description = "Development", Url = "http://localhost:5200" });
    }

    c.AddServer(new OpenApiServer { Description = "Production", Url = "https://api.sstaa.org" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Firebase ID Token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
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
        }
    );

    var filePath = Path.Combine(AppContext.BaseDirectory, "SSTAlumniAssociation.Api.xml");
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

app.UseAuthentication();
app.UseAuthorization();

// Require authorization by default and opt-out for anonymous routes
app.MapGrpcService<ArticleServiceV1>();
app.MapGrpcService<UserServiceV1>();
app.MapGrpcService<EventServiceV1>();
app.MapGrpcService<AuthServiceV1>();

app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "SST Alumni Association API v1"); });

app.Run();