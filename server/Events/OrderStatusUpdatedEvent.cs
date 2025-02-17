namespace ordermanagement.Events
{
    public class OrderStatusUpdatedEvent
    {
        public int OrderId { get; set; }
        public string Status { get; set; } = "";
        public int UserId { get; set; }
    }
}