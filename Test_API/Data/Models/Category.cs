using System.ComponentModel.DataAnnotations;

namespace Test_API.Data.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        public string? note { get; set; }

        public List<Item> Items { get; set; }
    }
}
