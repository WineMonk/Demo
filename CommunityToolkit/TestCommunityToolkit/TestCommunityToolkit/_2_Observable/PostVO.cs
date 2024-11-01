using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows;
using TestCommunityToolkit._5_Messenger;

namespace TestCommunityToolkit._2_Observable
{
    public partial class PostVO : ObservableRecipient, IRecipient<CommentMessage>
    {
        [ObservableProperty]
        private string _title;
        [ObservableProperty]
        private string _content;
        [ObservableProperty]
        private IList<CommentVO> _comments;

        [RelayCommand]
        private void AddComment()
        {
            IsActive = true;
            try
            {
                CommentWindow window = new CommentWindow();
                window.Owner = Application.Current.MainWindow;
                window.DataContext = new CommentVO();
                window.ShowDialog();
            }
            finally
            {
                IsActive = false;
            }
        }

        public void Receive(CommentMessage message)
        {
            this.Comments.Add(message.Comment);
            message.Reply(new() { message = "Comment added successfully!", success = true });
        }
    }
}
