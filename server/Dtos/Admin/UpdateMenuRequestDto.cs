﻿namespace ordermanagement.Dtos.Admin
{
    public class UpdateMenuRequestDto
    {

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string? Image { get; set; }
    }
}