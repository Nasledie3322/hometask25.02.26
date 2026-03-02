using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApi.Entities;

namespace MyApp.Pages.Users
{
    [Authorize(Roles = "SuperAdmin")]
    public class UsersModel : PageModel
    {
        public List<(ApplicationUser User, IList<string> Roles)> users = new();
        private readonly UserManager<ApplicationUser> _userManager;
        public UsersModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task OnGetAsync()
        {
            var allUsers = _userManager.Users.ToList();
            foreach (var u in allUsers)
            {
                var roles = await _userManager.GetRolesAsync(u);
                users.Add((u, roles));
            }
        }
    }
}
