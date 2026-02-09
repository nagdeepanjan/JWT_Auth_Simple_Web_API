using JWT_Auth.Data;
using JWT_Auth.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Servers = new List<OpenApiServer>
        {
            new() { Url = "https://jwt-auth-simple-web-api.wittyhill-7ad7b710.eastus.azurecontainerapps.io" }
        };
        return Task.CompletedTask;
    });
});
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<UserDbContext>(options=>options.UseSqlServer(builder.Configuration.GetConnectionString("UserDatabaseConnectionString")));     //DB


builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
