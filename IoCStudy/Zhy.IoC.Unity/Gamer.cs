using Unity;
using Zhy.IoC.Core;

namespace Zhy.IoC.Unity
{
    internal class Gamer : ConstructBase, IPerson
    {
        public IComputer Computer { get; set; }
        [Dependency]
        public IKeyboard Keyboard { get; set; }
        public IMouse Mouse { get; set; }

        [InjectionConstructor]
        public Gamer(IComputer computer) : base()
        {
            Computer = computer;
        }

        [InjectionMethod]
        public void Inject(IMouse mouse)
        {
            this.Mouse = mouse;
        }

        public void Work()
        {
            Console.WriteLine("The player is playing...");
        }
    }
}
