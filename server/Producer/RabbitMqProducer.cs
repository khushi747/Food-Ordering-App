using MassTransit;
using Mysqlx.Crud;
using ordermanagement.Dtos.User;
using ordermanagement.Events;
using ordermanagement.Models;
using System;
using System.Threading.Tasks;

public class OrderStatusPublisher : IOrderStatusPublisher
{
    private readonly IBus _bus;

    public OrderStatusPublisher(IBus bus)
    {
        _bus = bus;
    }

    public async Task PublishOrderStatusUpdate(OrderStatusUpdatedEvent orderEvent)
    {
        var queueName = $"order-status-updates-user-{orderEvent.UserId}";
        var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:{queueName}"));

        await endpoint.Send(orderEvent);
    }
}

public interface IOrderStatusPublisher
{
    Task PublishOrderStatusUpdate(OrderStatusUpdatedEvent orderEvent);
}

public class OrderCreatePublisher : IOrderCreatePublisher
{
    private readonly IBus _bus;

    public OrderCreatePublisher(IBus bus)
    {
        _bus = bus;
    }

    public async Task PublishOrderCreateUpdate(OrderDto orderEvent)
    {
        var queueName = $"order-create-user";
        var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:{queueName}"));

        await endpoint.Send(orderEvent);
    }
}

public interface IOrderCreatePublisher
{
    Task PublishOrderCreateUpdate(OrderDto orderEvent);
}
