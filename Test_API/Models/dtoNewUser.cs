using System.ComponentModel.DataAnnotations;

namespace Test_API.Models
{
    public class dtoNewUser
    {
       // public int IdUser { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        public string? phoneNumber { get; set; }
    }
}
