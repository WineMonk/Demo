using System.ComponentModel.DataAnnotations;

namespace TestEFCore.Entities
{
    public class BlogDTO
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public IList<PostDTO> Posts { get; set; } = new List<PostDTO>();
    }
}
