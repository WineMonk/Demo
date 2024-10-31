using Microsoft.AspNetCore.Mvc;
using TestEFCore.Entities;
using TestEFCore.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestEFCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        // 获取所有博客
        [HttpGet]
        public async Task<ActionResult<List<BlogDTO>>> GetAllBlogs()
        {
            var blogs = await _blogService.GetAllBlogs();
            return Ok(blogs);
        }

        // 根据博客ID获取博客
        [HttpGet("{blogId}")]
        public async Task<ActionResult<BlogDTO>> GetBlogById(int blogId)
        {
            var blog = await _blogService.GetBlogById(blogId);
            if (blog == null)
            {
                return NotFound();
            }
            return Ok(blog);
        }

        // 创建新博客
        [HttpPost]
        public async Task<ActionResult<BlogDTO>> CreateBlog([FromBody] BlogDTO newBlog)
        {
            var createdBlog = await _blogService.CreateBlog(newBlog);
            return CreatedAtAction("api/Blog", new { blogId = createdBlog.Id }, createdBlog);
        }

        // 更新博客信息
        [HttpPut("{blogId}")]
        public async Task<ActionResult<BlogDTO>> UpdateBlog(int blogId, [FromBody] BlogDTO updatedBlog)
        {
            if (blogId != updatedBlog.Id)
            {
                return BadRequest("博客ID不匹配");
            }
            var result = await _blogService.UpdateBlog(updatedBlog);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // 删除博客
        [HttpDelete("{blogId}")]
        public async Task<ActionResult> DeleteBlog(int blogId)
        {
            await _blogService.DeleteBlog(blogId);
            return NoContent();
        }

        // 获取指定博客下的所有文章
        [HttpGet("{blogId}/posts")]
        public async Task<ActionResult<List<PostDTO>>> GetPostsByBlogId(int blogId)
        {
            var posts = await _blogService.GetPostsByBlogId(blogId);
            return Ok(posts);
        }

        // 根据文章ID获取文章
        [HttpGet("{blogId}/posts/{postId}")]
        public async Task<ActionResult<PostDTO>> GetPostById(int blogId, int postId)
        {
            var post = await _blogService.GetPostById(postId);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        // 创建新文章并关联到指定博客
        [HttpPost("{blogId}/posts")]
        public async Task<ActionResult<PostDTO>> CreatePost(int blogId, [FromBody] PostDTO newPost)
        {
            var createdPost = await _blogService.CreatePost(blogId, newPost);
            return CreatedAtAction(nameof(GetPostById), new { blogId = blogId, postId = createdPost.Id }, createdPost);
        }

        // 更新文章信息
        [HttpPut("{blogId}/posts/{postId}")]
        public async Task<ActionResult<PostDTO>> UpdatePost(int blogId, int postId, [FromBody] PostDTO updatedPost)
        {
            if (postId != updatedPost.Id)
            {
                return BadRequest("文章ID不匹配");
            }
            var result = await _blogService.UpdatePost(updatedPost);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // 删除文章
        [HttpDelete("{blogId}/posts/{postId}")]
        public async Task<ActionResult> DeletePost(int blogId, int postId)
        {
            await _blogService.DeletePost(postId);
            return NoContent();
        }

        // 获取指定文章下的所有评论
        [HttpGet("{blogId}/posts/{postId}/comments")]
        public async Task<ActionResult<List<CommentDTO>>> GetCommentsByPostId(int blogId, int postId)
        {
            var comments = await _blogService.GetCommentsByPostId(postId);
            return Ok(comments);
        }

        // 根据评论ID获取评论
        [HttpGet("{blogId}/posts/{postId}/comments/{commentId}")]
        public async Task<ActionResult<CommentDTO>> GetCommentById(int blogId, int postId, int commentId)
        {
            var comment = await _blogService.GetCommentById(commentId);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment);
        }

        // 创建新评论并关联到指定文章
        [HttpPost("{blogId}/posts/{postId}/comments")]
        public async Task<ActionResult<CommentDTO>> CreateComment(int blogId, int postId, [FromBody] CommentDTO newComment)
        {
            var createdComment = await _blogService.CreateComment(postId, newComment);
            return CreatedAtAction(nameof(GetCommentById), new { blogId = blogId, postId = postId, commentId = createdComment.Id }, createdComment);
        }

        // 更新评论信息
        [HttpPut("{blogId}/posts/{postId}/comments/{commentId}")]
        public async Task<ActionResult<CommentDTO>> UpdateComment(int blogId, int postId, int commentId, [FromBody] CommentDTO updatedComment)
        {
            if (commentId != updatedComment.Id)
            {
                return BadRequest("评论ID不匹配");
            }
            var result = await _blogService.UpdateComment(updatedComment);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // 删除评论
        [HttpDelete("{blogId}/posts/{postId}/comments/{commentId}")]
        public async Task<ActionResult> DeleteComment(int blogId, int postId, int commentId)
        {
            await _blogService.DeleteComment(commentId);
            return NoContent();
        }
    }
}
