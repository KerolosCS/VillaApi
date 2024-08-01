using API1.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Logger File 
//Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File("Log/villaLogger.txt" , rollingInterval : RollingInterval.Day).CreateLogger();
//builder.Host.UseSerilog();
builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Custom Logger 
//builder.Services.AddSingleton<API1.Logging.ILogging, API1.Logging.Logging>();

builder.Services.AddDbContext<ApplicationDbContext>(o => {
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
