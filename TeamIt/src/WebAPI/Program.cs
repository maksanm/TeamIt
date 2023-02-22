using Application;
using Application.Hubs;
using Infrastructure;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
	using var scope = app.Services.CreateScope();
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
	}
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();

app.UseRouting();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/chatHub");

app.Run();

// Make Program class visible for integration tests
public partial class Program
{ }