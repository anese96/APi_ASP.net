using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test_API.Data.Models
{
    public class Item
    {
        [Key] 
        public int IdItem { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public double Price { get; set; }
        public string? Notes { get; set; }
        public byte[]? Image { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
