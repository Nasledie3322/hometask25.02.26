
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApi.Interfaces;

using Microsoft.AspNetCore.Authorization;

namespace MyApp.Pages.Products
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class DeleteProductModel(IProductService productService) : PageModel
    {
        private readonly IProductService service = productService;
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost(int id)
        {
            await service.DeleteProduct(id);
            return RedirectToPage("/Products/Products");
        }
    }
}
