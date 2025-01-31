namespace ordermanagement.Dtos.Admin
{
    public class ViewMenuDto
    {
        //public int ItemId { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string? Image { get; set; }
    }
}
