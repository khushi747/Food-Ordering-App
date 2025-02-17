namespace ordermanagement.Dtos.Chef
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public string UserName { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderStatusDto> OrderStatuses { get; set; } = new();
    }

}
