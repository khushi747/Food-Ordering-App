﻿namespace ordermanagement.Dtos.User
{
    public class CreateOrderDto
    {
        public int UserId { get; set; }
        public List<OrderItemDto> Items { get; set; } = [];

    }
    public class OrderItemDto
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }
}
