using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ordermanagement.Models
{
    public partial class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }

        public virtual User User { get; set; } = null!;

        public virtual ICollection<Orderdetail> Orderdetails { get; set; } = new List<Orderdetail>();
        public virtual ICollection<Orderstatus> Orderstatuses { get; set; } = new List<Orderstatus>();
    }
}
