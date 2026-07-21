using AuthService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AuthDbContext>(option =>
    option.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddControllers();

var app = builder.Build();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
