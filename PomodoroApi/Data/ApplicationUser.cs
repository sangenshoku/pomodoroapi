using Microsoft.AspNetCore.Identity;

namespace PomodoroApi.Data;

public class ApplicationUser : IdentityUser
{
    public ICollection<Models.Task>? Tasks { get; set; }
}