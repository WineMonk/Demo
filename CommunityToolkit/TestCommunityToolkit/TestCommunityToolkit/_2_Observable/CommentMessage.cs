using CommunityToolkit.Mvvm.Messaging.Messages;

namespace TestCommunityToolkit._2_Observable
{
    public class CommentMessage : RequestMessage<(bool success, string message)>
    {
        public CommentVO Comment { get; set; }

        public CommentMessage(CommentVO comment)
        {
            Comment = comment;
        }
    }
}
