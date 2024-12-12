using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text.Json.Serialization;
using System.Windows;
using TestCommunityToolkit._2_Observable;
using TestCommunityToolkit._4_IoC.View;

namespace TestCommunityToolkit._1_Attribute
{
    public partial class BlogVO : ObservableObject
    {
        [property: JsonIgnore]
        [ObservableProperty]
        private string _name;

        // [ObservableProperty]
        //
        //         ↓
        //
        // public string? Name
        // {
        //     get => name;
        //     set
        //     {
        //         if (!EqualityComparer<string?>.Default.Equals(name, value))
        //         {
        //             string? oldValue = name;
        //             OnNameChanging(value);
        //             OnNameChanging(oldValue, value);
        //             OnPropertyChanging();
        //             name = value;
        //             OnNameChanged(value);
        //             OnNameChanged(oldValue, value);
        //             OnPropertyChanged();
        //         }
        //     }
        // }

        [ObservableProperty]
        private string _description;

        [ObservableProperty]
        private string _url;

        [ObservableProperty]
        private IList<PostVO> _posts;

        [RelayCommand]
        private void OnBlogInfo()
        {
            MessageBox.Show($"Name: {Name}\nUrl: {Url}\nDescription: {Description}");
        }
        // [RelayCommand]
        //
        //       ↓
        //
        // private ICommand blogInfoCommand;
        // public ICommand BlogInfoCommand => blogInfoCommand ??= new RelayCommand(BlogInfo);

        [RelayCommand]
        private async Task OnAddPostAsync()
        {
            await Task.Delay(1000);
            // Add a new post to the list...
        }
        // [RelayCommand]
        //
        //       ↓
        //
        // private ICommand addPostCommand;
        // public IAsyncRelayCommand AddPostCommand => addPostCommand ??= new AsyncRelayCommand(new Func<Task>(AddPost));


        [RelayCommand]
        private void OpenBlog()
        {
            PostWindow window = new PostWindow();
            window.Owner = Application.Current.MainWindow;
            dynamic vm = window.DataContext;
            vm.SetPosts(this);
            window.ShowDialog();
        }
    }
}
