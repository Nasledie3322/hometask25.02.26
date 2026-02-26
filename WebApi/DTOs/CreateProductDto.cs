using System;

namespace WebApi.DTOs;

public class CreateProductDto
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string Description { get; set; } = null!;
    public int Stock { get; set; }

    public CreateProductDto(string name, decimal price, string description, int stock = 0)
    {
        Name = name;
        Price = price;
        Description = description;
        Stock = stock;
    }

    public CreateProductDto() { }
} 