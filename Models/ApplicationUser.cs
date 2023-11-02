using Microsoft.AspNetCore.Identity;

namespace PWEBTP.Models
{
    public class ApplicationUser : IdentityUser
    { 
        public string PrimeiroNome { get; set; }
        public string UltimoNome { get; set; }
    }
}
