using System;

namespace ordermanagement.Models
{
    public partial class Orderdetail
    {
        public int OrderId { get; set; }
        public string? ItemsDetails { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalPrice { get; set; }
        public int OrderdetailId { get; set; }
        public virtual Order? Order { get; set; }
    }
}
