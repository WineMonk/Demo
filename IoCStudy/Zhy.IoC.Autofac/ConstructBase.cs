namespace Zhy.IoC.Autofac
{
    public class ConstructBase
    {
        public ConstructBase()
        {
            Console.WriteLine($"{this.GetType().FullName} - 被构造了");
        }
    }
}
