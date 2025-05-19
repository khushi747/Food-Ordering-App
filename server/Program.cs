using Microsoft.EntityFrameworkCore;
using ordermanagement.Data;
using ordermanagement.Service;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ordermanagement.Events;
using ordermanagement.Producer;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ChefService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddControllers();

builder.Services.AddMassTransit(x =>
{
 x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("puffin.rmq2.cloudamqp.com", "ysaakyrp", h =>
        {
            h.Username("ysaakyrp");
            h.Password("gs-Uyp1qiqJY4F5DtEe_vKP1yCG2Sz-C");
        });
    });
});


builder.Services.AddControllers();
// builder.Services.AddMassTransit(x =>
// {
//     x.SetKebabCaseEndpointNameFormatter();

//     x.UsingRabbitMq((context, cfg) =>
//     {
//         cfg.Host("rabbitmq://localhost");

//         cfg.Publish<OrderStatusUpdatedEvent>(e => e.ExchangeType = "topic");

//         cfg.ConfigureEndpoints(context);
//     });
// });

// builder.Services.AddMassTransit(x => {
//     x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config => {
//         config.Host(new Uri("rabbit://localhost"), h =>{
//             h.Username("guest");
//             h.Password("guest");
//         });
//     }))
// });

// builder.Services.AddMassTransitHostedService();
   
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IOrderStatusPublisher, OrderStatusPublisher>();
builder.Services.AddScoped<IOrderCreatePublisher, OrderCreatePublisher>();

var connectionString = builder.Configuration.GetConnectionString("dbcs");
builder.Services.AddDbContext<GininternsContext>(
    options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)), 
    ServiceLifetime.Scoped
);
builder.Services.AddCors();

var app = builder.Build();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
