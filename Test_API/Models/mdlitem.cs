using System.ComponentModel.DataAnnotations;

namespace Test_API.Models
{
    public class mdlitem
    {
        [MaxLength(100)]
        public string Name { get; set; }
        public double Price { get; set; }
        public string? Notes { get; set; }
        public IFormFile Image { get; set; }
        public int CategoryId { get; set; }
    }
}
