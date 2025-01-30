
using Microsoft.EntityFrameworkCore;
using ordermanagement.Data;
using ordermanagement.Controllers;
using ordermanagement.Service;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<UserService>();
builder.Services.AddControllers()
    //.AddJsonOptions(options =>
    //{
    //    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    //    options.JsonSerializerOptions.WriteIndented = true;
    //})
    ;




builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//var provider = builder.Services.BuildServiceProvider();
//var config = provider.GetService<IConfiguration>();
var connectionString = builder.Configuration.GetConnectionString("dbcs");
builder.Services.AddDbContext<GininternsContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)), ServiceLifetime.Scoped);

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

//app.MapMenuitemEndpoints();

app.Run();
