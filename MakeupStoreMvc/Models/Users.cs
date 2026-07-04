using Microsoft.AspNetCore.Identity;

namespace MakeupStoreMvc.Models
{
    public class Users : IdentityUser
    {
        public string FullName { get; set; }
    }
}