using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApi.Entities;

namespace MyApp.Pages.Users
{
    [Authorize(Roles = "SuperAdmin")]
    public class EditUserRolesModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EditUserRolesModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty(SupportsGet = true)]
        public string? Id { get; set; }

        public ApplicationUser? User { get; set; }
        public IList<string> CurrentRoles { get; set; } = new List<string>();
        public List<SelectListItem> AllRoles { get; set; } = new();

        [BindProperty]
        public List<string> SelectedRoles { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(Id))
                return NotFound();

            User = await _userManager.FindByIdAsync(Id);
            if (User == null)
                return NotFound();

            CurrentRoles = await _userManager.GetRolesAsync(User);

            AllRoles = _roleManager.Roles
                .Select(r => new SelectListItem { Value = r.Name, Text = r.Name })
                .ToList();

            SelectedRoles = CurrentRoles.ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Id))
                return NotFound();

            User = await _userManager.FindByIdAsync(Id);
            if (User == null)
                return NotFound();

            var existing = await _userManager.GetRolesAsync(User);
            var removeResult = await _userManager.RemoveFromRolesAsync(User, existing);
            if (!removeResult.Succeeded)
            {
                foreach (var err in removeResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, err.Description);
                }
                return Page();
            }

            if (SelectedRoles?.Count > 0)
            {
                var addResult = await _userManager.AddToRolesAsync(User, SelectedRoles);
                if (!addResult.Succeeded)
                {
                    foreach (var err in addResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, err.Description);
                    }
                    return Page();
                }
            }

            return RedirectToPage("Users");
        }
    }
}