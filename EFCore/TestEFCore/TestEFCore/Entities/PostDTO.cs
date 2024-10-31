using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TestEFCore.Entities
{
    public class PostDTO
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishedDate { get; set; } = DateTime.Now;
        public IList<CommentDTO> Comments { get; set; } = new List<CommentDTO>();
    }
}
