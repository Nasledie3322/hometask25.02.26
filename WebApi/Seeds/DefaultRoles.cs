using System;
using Microsoft.AspNetCore.Identity;

namespace WebApi.Seeds;

public class DefaultRoles
{
    public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        // application roles: SuperAdmin has full authority, Admin can perform CRUD, User is read‑only
        string[] roles = { "SuperAdmin", "Admin", "User" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}
