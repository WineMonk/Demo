using System.ComponentModel.DataAnnotations;

namespace TestEFCore.Entities
{
    public class CommentDTO
    {
        public int? Id { get; set; }
        public string Content { get; set; }
        public DateTime CommentedDate { get; set; } = DateTime.Now;
    }
}
