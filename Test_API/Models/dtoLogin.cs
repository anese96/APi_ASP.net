using System.ComponentModel.DataAnnotations;

namespace Test_API.Models
{
    public class dtoLogin
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
