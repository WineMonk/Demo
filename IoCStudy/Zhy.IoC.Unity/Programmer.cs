using Zhy.IoC.Core;

namespace Zhy.IoC.Unity
{
    internal class Programmer : ConstructBase, IPerson
    {
        public IComputer Computer { get; set; }
        public IKeyboard Keyboard { get; set; }
        public IMouse Mouse { get; set; }

        public Programmer(IComputer computer, IKeyboard keyboard, IMouse mouse) : base()
        {
            Computer = computer;
            Keyboard = keyboard;
            Mouse = mouse;
        }

        public void Work()
        {
            Console.WriteLine("Programmers are building software...");
        }
    }
}
