using Zhy.IoC.Core;

namespace Zhy.IoC.Autofac
{
    public class Gamer : ConstructBase, IPerson
    {
        public IComputer Computer { get; set; }
        [InjectProperty]
        public IKeyboard Keyboard { get; set; }
        public IMouse Mouse { get; set; }

        public Gamer(IComputer computer) : base()
        {
            Computer = computer;
        }

        public void Inject(IMouse mouse)
        {
            Mouse = mouse;
        }

        public void Work()
        {
            Console.WriteLine("The player is playing...");
        }
    }
}
