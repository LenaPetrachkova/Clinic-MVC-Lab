using Microsoft.AspNetCore.Identity;

namespace ClinicDomain.Model
{
    public class User : IdentityUser
    {
        public int ClinicId { get; set; }
    }
}
