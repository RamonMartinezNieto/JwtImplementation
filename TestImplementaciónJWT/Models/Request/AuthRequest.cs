using System.ComponentModel.DataAnnotations;

namespace TestImplementaciónJWT.Models.Request
{
    public class AuthRequest
    {
        [Required]
        public string UserName { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}
