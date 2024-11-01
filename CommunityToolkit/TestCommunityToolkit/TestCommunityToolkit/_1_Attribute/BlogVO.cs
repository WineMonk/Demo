using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Xml.Linq;
using TestCommunityToolkit._2_Observable;
using TestCommunityToolkit._4_IoC.View;

namespace TestCommunityToolkit._1_Attribute
{
    public partial class BlogVO : ObservableObject
    {
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
        private void BlogInfo()
        {
            MessageBox.Show($"Name: {_name}\nUrl: {_url}\nDescription: {_description}");
        }
        // [RelayCommand]
        //
        //         ↓
        //
        // private ICommand blogInfoCommand;
        // public ICommand BlogInfoCommand => blogInfoCommand ??= new RelayCommand(BlogInfo);

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
