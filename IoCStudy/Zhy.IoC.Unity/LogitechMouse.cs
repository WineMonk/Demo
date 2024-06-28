using Zhy.IoC.Core;

namespace Zhy.IoC.Unity
{
    internal class LogitechMouse : ConstructBase, IMouse
    {
        public LogitechMouse() : base() { }

        public LogitechMouse(string type) : base()
        {
            Type = type;
        }

        public string Type { get; set; }
    }
}
