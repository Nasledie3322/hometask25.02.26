using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApi.DTOs;
using WebApi.Entities;

namespace RazorSide.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    public RegisterModel(UserManager<ApplicationUser> userManager,
                         SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;

    }

    [BindProperty]
    public RegisterDto RegisterDto { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var email = RegisterDto.Email.Trim();
            var fullName = RegisterDto.FullName.Trim();

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = fullName
            };

            var result = await _userManager.CreateAsync(user, RegisterDto.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return Page();
            }

            // newly registered users become plain Users by default
            await _userManager.AddToRoleAsync(user, "User");

            // auto signin so the act of registering leaves the user authenticated
            await _signInManager.SignInAsync(user, isPersistent: false);

            // after self‑registration redirect to a page the new user can actually view
            // (the original code sent everyone to the user‑management page, which is
            // restricted to SuperAdmin and caused a 403/"couldn't register" feeling)
            return RedirectToPage("/Products/Products");
        }

    }
}
