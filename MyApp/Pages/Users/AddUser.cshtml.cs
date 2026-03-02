using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApi.DTOs;
using WebApi.Entities;

namespace MyApp.Pages.Users
{
    [Authorize(Roles = "SuperAdmin")]
    public class AddUserModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AddUserModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public RegisterDto RegisterDto { get; set; } = new();

        [BindProperty]
        public string SelectedRole { get; set; } = "User";

        public List<SelectListItem> Roles { get; set; } = new();

        public async Task OnGet()
        {
            Roles = _roleManager.Roles
                .Select(r => new SelectListItem { Value = r.Name, Text = r.Name })
                .ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGet(); // repopulate roles
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
                await OnGet();
                return Page();
            }

            await _userManager.AddToRoleAsync(user, SelectedRole);

            return RedirectToPage("Users");
        }
    }
}
