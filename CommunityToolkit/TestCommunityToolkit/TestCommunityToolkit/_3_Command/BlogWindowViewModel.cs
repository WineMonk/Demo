using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TestCommunityToolkit._1_Attribute;
using TestCommunityToolkit._2_Observable;

namespace TestCommunityToolkit._3_Command
{
    public partial class BlogWindowViewModel : ObservableObject
    {
        public BlogWindowViewModel()
        {
            _blogs = new ObservableCollection<BlogVO>()
            {
                new BlogVO()
                {
                    Name = "Blog 1",
                    Url  = "www.blog1.com",
                    Description = "This is the description of Blog 1.This is the description of Blog 1.This is the description of Blog 1.",
                    Posts = new ObservableCollection<PostVO>
                    {
                        new PostVO()
                        {
                            Title = "Post 1",
                            Content = "This is the content of Post 1.This is the content of Post 1.This is the content of Post 1.",
                            Comments = new ObservableCollection<CommentVO>
                            {
                                new CommentVO()
                                {
                                    Author = "John Doe",
                                    Email = "john.doe@example.com",
                                    Text = "This is the first comment of Post 1."
                                },
                                new CommentVO()
                                {
                                    Author = "Jane Doe",
                                    Email = "jane.doe@example.com",
                                    Text = "This is the second comment of Post 1."
                                }
                            }
                        },
                        new PostVO()
                        {
                            Title = "Post 2",
                            Content = "This is the content of Post 2.This is the content of Post 2.This is the content of Post 2.",
                            Comments = new ObservableCollection<CommentVO>
                            {
                                new CommentVO()
                                {
                                    Author = "John Doe",
                                    Email = "john.doe@example.com",
                                    Text = "This is the first comment of Post 2."
                                },
                                new CommentVO()
                                {
                                    Author = "Jane Doe",
                                    Email = "jane.doe@example.com",
                                    Text = "This is the second comment of Post 2."
                                }
                            }
                        },
                        new PostVO()
                        {
                            Title = "Post 3",
                            Content = "This is the content of Post 3.This is the content of Post 3.This is the content of Post 3.",
                            Comments = new ObservableCollection<CommentVO>
                            {
                                new CommentVO()
                                {
                                    Author = "John Doe",
                                    Email = "john.doe@example.com",
                                    Text = "This is the first comment of Post 3."
                                },
                                new CommentVO()
                                {
                                    Author = "Jane Doe",
                                    Email = "jane.doe@example.com",
                                    Text = "This is the second comment of Post 3."
                                }
                            }
                        }
                    }
                },
                new BlogVO()
                {
                    Name = "Blog 2",
                    Url  = "www.blog2.com",
                    Description = "This is the description of Blog 2.This is the description of Blog 2.This is the description of Blog 2.",
                },
                new BlogVO()
                {
                    Name = "Blog 3",
                    Url  = "www.blog3.com",
                    Description = "This is the description of Blog 3.This is the description of Blog 3.This is the description of Blog 3.",
                },
                new BlogVO()
                {
                    Name = "Blog 4",
                    Url  = "www.blog4.com",
                    Description = "This is the description of Blog 4.This is the description of Blog 4.This is the description of Blog 4.",
                },
                new BlogVO()
                {
                    Name = "Blog 5",
                    Url  = "www.blog5.com",
                    Description = "This is the description of Blog 5.This is the description of Blog 5.This is the description of Blog 5.",
                },
                new BlogVO()
                {
                    Name = "Blog 6",
                    Url  = "www.blog6.com",
                    Description = "This is the description of Blog 6.This is the description of Blog 6.This is the description of Blog 6.",
                },
                new BlogVO()
                {
                    Name = "Blog 7",
                    Url  = "www.blog7.com",
                    Description = "This is the description of Blog 7.This is the description of Blog 7.This is the description of Blog 7.",
                },
                new BlogVO()
                {
                    Name = "Blog 8",
                    Url  = "www.blog8.com",
                    Description = "This is the description of Blog 8.This is the description of Blog 8.This is the description of Blog 8.",
                },
                new BlogVO()
                {
                    Name = "Blog 9",
                    Url  = "www.blog9.com",
                    Description = "This is the description of Blog 9.This is the description of Blog 9.This is the description of Blog 9.",
                },
                new BlogVO()
                {
                    Name = "Blog 10",
                    Url  = "www.blog10.com",
                    Description = "This is the description of Blog 10.This is the description of Blog 10.This is the description of Blog 10.",
                }
            };
        }

        [ObservableProperty]
        private IList<BlogVO> _blogs;

        [RelayCommand]
        private void DeleteBlog(BlogVO blog)
        {
            _blogs.Remove(blog);
        }
    }
}
