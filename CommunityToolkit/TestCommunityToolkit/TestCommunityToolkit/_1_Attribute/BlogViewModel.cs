using CommunityToolkit.Mvvm.ComponentModel;

namespace TestCommunityToolkit._1_Attribute
{
    public class BaseObject
    {
        // Some common properties and methods...
    }

    [INotifyPropertyChanged]
    public partial class BlogViewModel : BaseObject
    {
        // Some properties and methods...
    }
}
