using System;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using WebApi.Entities;

namespace WebApi.Interfaces;

public interface IProductService
{
    Task<List<ProductDto>> GetProducts();
    Task<List<ProductDto>> GetAllAsync();
    Task<string> AddProduct(CreateProductDto productDto);
    Task<ProductDto> AddAsync(CreateProductDto productDto);
    Task<ProductDto> GetProduct(int id);
    Task DeleteAsync(int id);
}
