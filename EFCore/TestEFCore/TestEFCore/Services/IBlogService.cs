using TestEFCore.Entities;

namespace TestEFCore.Services
{
    public interface IBlogService
    {
        // 获取所有博客
        Task<List<BlogDTO>> GetAllBlogs();

        // 根据博客ID获取博客
        Task<BlogDTO> GetBlogById(int blogId);

        // 创建新博客
        Task<BlogDTO> CreateBlog(BlogDTO newBlog);

        // 更新博客信息
        Task<BlogDTO> UpdateBlog(BlogDTO updatedBlog);

        // 删除博客
        Task DeleteBlog(int blogId);

        // 获取指定博客下的所有文章
        Task<List<PostDTO>> GetPostsByBlogId(int blogId);

        // 根据文章ID获取文章
        Task<PostDTO> GetPostById(int postId);

        // 创建新文章并关联到指定博客
        Task<PostDTO> CreatePost(int blogId, PostDTO newPost);

        // 更新文章信息
        Task<PostDTO> UpdatePost(PostDTO updatedPost);

        // 删除文章
        Task DeletePost(int postId);

        // 获取指定文章下的所有评论
        Task<List<CommentDTO>> GetCommentsByPostId(int postId);

        // 根据评论ID获取评论
        Task<CommentDTO> GetCommentById(int commentId);

        // 创建新评论并关联到指定文章
        Task<CommentDTO> CreateComment(int postId, CommentDTO newComment);

        // 更新评论信息
        Task<CommentDTO> UpdateComment(CommentDTO updatedComment);

        // 删除评论
        Task DeleteComment(int commentId);
    }
}
