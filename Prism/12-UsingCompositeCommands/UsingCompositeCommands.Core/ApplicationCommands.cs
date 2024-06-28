using Prism.Commands;

namespace UsingCompositeCommands.Core
{
    public interface IApplicationCommands
    {
        CompositeCommand MyCompositeCommand { get; }
    }

    public class ApplicationCommands : IApplicationCommands
    {
        private CompositeCommand _saveCommand = new CompositeCommand();
        public CompositeCommand MyCompositeCommand
        {
            get { return _saveCommand; }
        }
    }
}
