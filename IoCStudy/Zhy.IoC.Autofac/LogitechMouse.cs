using Zhy.IoC.Core;

namespace Zhy.IoC.Autofac
{
    public class LogitechMouse : ConstructBase, IMouse
    {
        public LogitechMouse() : base() { }

        public LogitechMouse(string type) : base()
        {
            Type = type;
        }

        public string Type { get; set; }
    }
}
