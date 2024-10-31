using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TestEFCore.Entities
{
    [Table("posts")]
    public class Post
    {
        [Key]
        public int PostId { get; set; }
        [Required]
        [StringLength(200)]
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishedDate { get; set; } = DateTime.Now;
        public int BlogId { get; set; }
        [ForeignKey("BlogId")]
        public Blog Blog { get; set; }
        public IList<Comment> Comments { get; set; } = new List<Comment>();
    }
}
