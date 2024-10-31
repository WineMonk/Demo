using BloggingApp;
using Microsoft.EntityFrameworkCore;
using TestEFCore.Entities;

namespace TestEFCore.Services
{
    public class BlogService : IBlogService
    {
        private readonly BloggingContext _context;

        public BlogService(BloggingContext context)
        {
            _context = context;
        }

        // 获取所有博客
        public async Task<List<BlogDTO>> GetAllBlogs()
        {
            List<Blog> blogs = await _context.Blogs.ToListAsync();
            List<BlogDTO> blogDTOs = new List<BlogDTO>();
            foreach (var blog in blogs)
            {
                BlogDTO blogDTO = new BlogDTO();
                blogDTO.Name = blog.Name;
                blogDTO.CreatedDate = blog.CreatedDate;
                blogDTO.Posts = blog.Posts.Select(p => new PostDTO
                {
                    Title = p.Title,
                    Content = p.Content,
                    PublishedDate = p.PublishedDate,
                    Comments = p.Comments.Select(c => new CommentDTO
                    {
                        Content = c.Content,
                        CommentedDate = c.CommentedDate
                    }).ToList()
                }).ToList();
                blogDTOs.Add(blogDTO);
            }
            return blogDTOs;
        }

        // 根据博客ID获取博客
        public async Task<BlogDTO> GetBlogById(int blogId)
        {
            var blog = await _context.Blogs.FindAsync(blogId);
            if (blog != null)
            {
                return ConvertToBlogDTO(blog);
            }
            return null;
        }

        // 创建新博客
        public async Task<BlogDTO> CreateBlog(BlogDTO newBlog)
        {
            Blog newBlogEntity = ConvertToBlog(newBlog);
            _context.Blogs.Add(newBlogEntity);
            await _context.SaveChangesAsync();
            return newBlog;
        }

        // 更新博客信息
        public async Task<BlogDTO> UpdateBlog(BlogDTO updatedBlog)
        {
            Blog blog = ConvertToBlog(updatedBlog);
            _context.Entry(blog).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return updatedBlog;
        }

        // 删除博客
        public async Task DeleteBlog(int blogId)
        {
            var blogToDelete = await _context.Blogs.FindAsync(blogId);
            if (blogToDelete != null)
            {
                _context.Blogs.Remove(blogToDelete);
                await _context.SaveChangesAsync();
            }
        }

        // 获取指定博客下的所有文章
        public async Task<List<PostDTO>> GetPostsByBlogId(int blogId)
        {
            var blog = await _context.Blogs.FindAsync(blogId);
            if (blog != null)
            {
                return blog.Posts.Select(ConvertToPostDTO).ToList();
            }
            return new List<PostDTO>();
        }

        // 根据文章ID获取文章
        public async Task<PostDTO> GetPostById(int postId)
        {
            Post? post = await _context.Posts.FindAsync(postId);
            if (post != null)
            {
                return ConvertToPostDTO(post);
            }
            return null;
        }

        // 创建新文章并关联到指定博客
        public async Task<PostDTO> CreatePost(int blogId, PostDTO newPost)
        {
            var blog = await _context.Blogs.FindAsync(blogId);
            if (blog != null)
            {
                Post newPostEntity = ConvertToPost(newPost);
                _context.Posts.Add(newPostEntity);
                await _context.SaveChangesAsync();
                if (newPost.Comments != null && newPost.Comments.Count > 0)
                {
                    foreach (var comment in newPost.Comments)
                    {
                        await CreateComment(newPostEntity.PostId, comment);
                    }
                }
                return newPost;
            }
            throw new ArgumentException("指定的博客ID不存在");
        }

        // 更新文章信息
        public async Task<PostDTO> UpdatePost(PostDTO updatedPost)
        {
            Post updatedPostEntity = ConvertToPost(updatedPost);
            _context.Entry(updatedPostEntity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return updatedPost;
        }

        // 删除文章
        public async Task DeletePost(int postId)
        {
            var postToDelete = await _context.Posts.FindAsync(postId);
            if (postToDelete != null)
            {
                _context.Posts.Remove(postToDelete);
                await _context.SaveChangesAsync();
            }
        }

        // 获取指定文章下的所有评论
        public async Task<List<CommentDTO>> GetCommentsByPostId(int postId)
        {
            var post = await _context.Posts.FindAsync(postId);
            if (post != null)
            {
                return post.Comments.Select(ConvertToCommentDTO).ToList();
            }
            return new List<CommentDTO>();
        }

        // 根据评论ID获取评论
        public async Task<CommentDTO> GetCommentById(int commentId)
        {
            Comment? comment = await _context.Comments.FindAsync(commentId);
            if (comment != null)
            {
                return ConvertToCommentDTO(comment);
            }
            return null;
        }

        // 创建新评论并关联到指定文章
        public async Task<CommentDTO> CreateComment(int postId, CommentDTO newComment)
        {
            var post = await _context.Posts.FindAsync(postId);
            if (post != null)
            {
                Comment newCommentEntity = ConvertToComment(newComment);
                _context.Comments.Add(newCommentEntity);
                await _context.SaveChangesAsync();
                return newComment;
            }
            throw new ArgumentException("指定的文章ID不存在");
        }

        // 更新评论信息
        public async Task<CommentDTO> UpdateComment(CommentDTO updatedComment)
        {
            Comment updatedCommentEntity = ConvertToComment(updatedComment);
            _context.Entry(updatedCommentEntity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return updatedComment;
        }

        // 删除评论
        public async Task DeleteComment(int commentId)
        {
            var commentToDelete = await _context.Comments.FindAsync(commentId);
            if (commentToDelete != null)
            {
                _context.Comments.Remove(commentToDelete);
                await _context.SaveChangesAsync();
            }
        }

        private BlogDTO ConvertToBlogDTO(Blog blog)
        {
            BlogDTO blogDTO = new BlogDTO();
            blogDTO.Id = blog.BlogId;
            blogDTO.Name = blog.Name;
            blogDTO.CreatedDate = blog.CreatedDate;
            blogDTO.Posts = blog.Posts.Select(ConvertToPostDTO).ToList();
            return blogDTO;
        }

        private Blog ConvertToBlog(BlogDTO blogDTO)
        {
            Blog blog = new Blog();
            blog.BlogId = blogDTO.Id ?? 0;
            blog.Name = blogDTO.Name;
            blog.CreatedDate = blogDTO.CreatedDate;
            blog.Posts = blogDTO.Posts.Select(p =>
            {
                Post post = ConvertToPost(p);
                post.Blog = blog;
                return post;
            }).ToList();
            return blog;
        }

        private PostDTO ConvertToPostDTO(Post post)
        {
            PostDTO postDTO = new PostDTO();
            postDTO.Id = post.PostId;
            postDTO.Title = post.Title;
            postDTO.Content = post.Content;
            postDTO.PublishedDate = post.PublishedDate;
            postDTO.Comments = post.Comments.Select(ConvertToCommentDTO).ToList();
            return postDTO;
        }

        private Post ConvertToPost(PostDTO postDTO)
        {
            Post post = new Post();
            post.PostId = postDTO.Id ?? 0;
            post.Title = postDTO.Title;
            post.Content = postDTO.Content;
            post.PublishedDate = postDTO.PublishedDate;
            post.Comments = postDTO.Comments.Select(c =>
            {
                Comment comment = ConvertToComment(c);
                comment.Post = post;
                return comment;
            }).ToList();
            return post;
        }

        private CommentDTO ConvertToCommentDTO(Comment comment)
        {
            CommentDTO commentDTO = new CommentDTO();
            commentDTO.Id = comment.CommentId;
            commentDTO.Content = comment.Content;
            commentDTO.CommentedDate = comment.CommentedDate;
            return commentDTO;
        }

        private Comment ConvertToComment(CommentDTO commentDTO)
        {
            Comment comment = new Comment();
            comment.CommentId = commentDTO.Id ?? 0;
            comment.Content = commentDTO.Content;
            comment.CommentedDate = commentDTO.CommentedDate;
            return comment;
        }
    }
}
