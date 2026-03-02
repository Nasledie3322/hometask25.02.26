using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApi.DTOs;
using WebApi.Interfaces;

using Microsoft.AspNetCore.Authorization;

namespace MyApp.Pages.Products
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class AddProductModel(IProductService productService) : PageModel
    {
        private readonly IProductService service = productService;
        
        [BindProperty]
        public CreateProductDto createProductDto {get; set;} = new();
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var product = new CreateProductDto(createProductDto.Name, createProductDto.Price, createProductDto.Description);
            var result = await service.AddProduct(createProductDto);
            return RedirectToPage("/Products/Products");
        }
    }
}
