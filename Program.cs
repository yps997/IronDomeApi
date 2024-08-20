using IronDomeApi.MiddleWares.Attack;
using IronDomeApi.MiddleWares.Global;
using IronDomeApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DBService>(options => options.UseSqlServer(connectionString));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<GlobalLoginMiddleware>();
app.UseWhen(
    context => context.Request.Path.StartsWithSegments("/api/attack"), appBuilder =>
    { 
        appBuilder.UseMiddleware<JwtValidationMiddleware>();
        appBuilder.UseMiddleware<GlobalLoginMiddleware>();
    });

app.MapControllers();

app.Run();
