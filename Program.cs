using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WebApiAuthors.Filters;
using WebApiAuthors.Middlewares;
using WebApiAuthors.Services;
using WebApiAutores.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddResponseCaching();
builder.Services.AddTransient<MyActionFilter>();
builder.Services.AddHostedService<WriteFile>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ExceptionFilter));
}).AddJsonOptions(x =>
{
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<LoggerResponseMiddleware>();

app.UseHttpsRedirection();

app.UseResponseCaching();

app.UseAuthorization();

app.MapControllers();

app.Run();
