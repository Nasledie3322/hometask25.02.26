using Microsoft.AspNetCore.Identity;
namespace WebApi.Entities;
public class ApplicationUser: IdentityUser
{
    public string FullName{get; set;} = null!;
    public DateTime CreatedAtUtc{get; set;} = DateTime.UtcNow;
}