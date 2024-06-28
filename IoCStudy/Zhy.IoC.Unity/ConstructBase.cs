namespace Zhy.IoC.Unity
{
    internal class ConstructBase
    {
        public ConstructBase()
        {
            Console.WriteLine($"{this.GetType().FullName} - 被构造了");
        }
    }
}
