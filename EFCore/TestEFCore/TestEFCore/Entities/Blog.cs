using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TestEFCore.Entities
{
    [Table("blogs")]
    public class Blog
    {
        [Key]
        public int BlogId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public IList<Post> Posts { get; set; } = new List<Post>();
    }
}
