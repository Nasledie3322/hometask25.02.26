using System;

namespace WebApi.DTOs;

public class ProductDto
{
    public int Id {get; set;}
    public string Name {get; set;}=null!;
    public decimal Price {get; set;}
    public string Description {get; set;}=null!;
    public ProductDto(int id, string name, decimal price, string description)
    {
        Id = id;
        Name = name;
        Price = price;
        Description = description;   
    }
    public ProductDto(){}

} 