using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApi.DTOs;
using WebApi.Interfaces;

using Microsoft.AspNetCore.Authorization;

namespace MyApp.Pages.Products
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class UpdateProductModel(IProductService productService) : PageModel
    {
        private readonly IProductService service = productService;
        
        public ProductDto productDto {get; set;}=new();


        public async Task OnGet(int id) 
        {
            productDto = await service.GetProduct(id);
        }
        [BindProperty]
        public CreateProductDto createProductDto {get; set;}=new();
        public async Task<IActionResult> OnPostAsync(int id)
        { 
            var product = new CreateProductDto(createProductDto.Name, createProductDto.Price, createProductDto.Description);
            await service.UpdateProduct(id, product); 
            return RedirectToPage("/Products/Products");
        }
    }
}
