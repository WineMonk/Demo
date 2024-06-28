namespace Zhy.IoC.Core
{
    public interface IPerson
    {
        IComputer Computer { get; set; }
        IKeyboard Keyboard { get; set; }
        IMouse Mouse { get; set; }
        void Work();
    }
}
