using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TestCommunityToolkit._1_Attribute;
using TestCommunityToolkit._2_Observable;

namespace TestCommunityToolkit._4_IoC.ViewModel
{
    public partial class PostWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private IList<PostVO> _posts;

        public void SetPosts(BlogVO blog)
        {
            Posts = blog.Posts ??= new ObservableCollection<PostVO>();
        }
    }
}
