using Microsoft.AspNetCore.Identity;

namespace WebApplication2.Data
{
    public class APIUser : IdentityUser
    {
        public string FirstName { get; set; }    
        public string LastName { get; set; }    
    }
}
