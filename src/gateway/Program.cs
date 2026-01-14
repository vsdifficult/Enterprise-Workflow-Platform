using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("auth", new OpenApiInfo { Title = "Auth API", Version = "v1" });
    c.SwaggerDoc("users", new OpenApiInfo { Title = "Users API", Version = "v1" });
});

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/auth/v1/swagger.json", "Auth API v1");
        c.SwaggerEndpoint("/swagger/users/v1/swagger.json", "Users API v1");
    });
}

app.MapReverseProxy();

app.Run();