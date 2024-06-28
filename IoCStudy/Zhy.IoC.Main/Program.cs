using Autofac;
using Zhy.IoC.Core;
using Zhy.IoC.Autofac;
using Autofac.Core;
using Autofac.Configuration;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;
using Unity.Interception.Interceptors.InstanceInterceptors.InterfaceInterception;
using Unity.Interception;
using Unity;
using Zhy.IoC.AOP;
using Unity.Interception.ContainerIntegration;
using Unity.Interception.Interceptors.TypeInterceptors.VirtualMethodInterception;
using Autofac.Extras.DynamicProxy;

namespace Zhy.IoC.Main
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TestAOP();
        }

        static void TestAOP()
        {
            var builder = new ContainerBuilder();
            builder.Register(c => new LoggingInterceptor());
            builder.RegisterType<MyService>()
                   .As<IMyService>()
                   .EnableInterfaceInterceptors()
                   .InterceptedBy(typeof(LoggingInterceptor));
            var container = builder.Build();
            var service = container.Resolve<IMyService>();
            service.DoWork();
        }

        static void TestAutofac()
        {
            // 配置依赖
            //var config = new ConfigurationBuilder();
            //config.AddJsonFile("DI-Autofac.json");
            //// config.AddXmlFile("DI-Autofac.xml");
            //var module = new ConfigurationModule(config.Build());
            //var builder = new ContainerBuilder();
            //builder.RegisterModule(module);
            //var container = builder.Build();
            //IPerson programmer = container.Resolve<IPerson>();
            //programmer.Work();

            // 生命周期
            //var builder = new ContainerBuilder();
            //builder.RegisterType<LenovoComputer>().As<IComputer>().InstancePerLifetimeScope();
            //var container = builder.Build();
            //IComputer computer0 = container.Resolve<IComputer>();
            //using (var scope = container.BeginLifetimeScope())
            //{
            //    IComputer computer1 = scope.Resolve<IComputer>();
            //    IComputer computer2 = scope.Resolve<IComputer>();
            //    Console.WriteLine("computer0: " + computer0.GetHashCode());
            //    Console.WriteLine("computer1: " + computer1.GetHashCode());
            //    Console.WriteLine("computer2: " + computer2.GetHashCode());
            //    Console.WriteLine($"computer0 == computer1: {computer0 == computer1}");
            //    Console.WriteLine($"computer1 == computer2: {computer1 == computer2}");
            //}

            // 指定参数
            //var builder = new ContainerBuilder();
            //builder.RegisterType<LogitechMouse>().WithParameter("type","502").As<IMouse>();
            //var container = builder.Build();
            //IMouse mouse = container.Resolve<IMouse>();
            //Console.WriteLine("mouse type: " + mouse.Type);

            // 重复注册
            //var builder = new ContainerBuilder();
            //builder.RegisterType<LenovoComputer>().As<IComputer>();
            //builder.RegisterType<TogarKeyboard>().As<IKeyboard>();
            //builder.RegisterType<LogitechMouse>().As<IMouse>();
            //builder.RegisterType<Gamer>()
            //    // 方法注入
            //    .OnActivating(e =>
            //    {
            //        IMouse mouse = e.Context.Resolve<IMouse>();
            //        e.Instance.Inject(mouse);
            //    })
            //    // 属性注入
            //    .PropertiesAutowired((g, o) => g.Name == "Keyboard")
            //    // 构造函数注入
            //    .UsingConstructor(typeof(IComputer))
            //    .Named<IPerson>("person");
            //builder.RegisterType<Programmer>().Named<IPerson>("programmer");
            //var container = builder.Build();
            //IPerson programmer = container.ResolveNamed<IPerson>("programmer");
            //IPerson person = container.ResolveNamed<IPerson>("person");
            //programmer.Work();
            //person.Work();

            // 注入顺序（与编码顺序无关）：构造函数注入 > 属性注入 > 方法注入
            //var builder = new ContainerBuilder();
            //builder.RegisterType<LenovoComputer>().As<IComputer>();
            //builder.RegisterType<TogarKeyboard>().As<IKeyboard>();
            //builder.RegisterType<LogitechMouse>().As<IMouse>();
            //builder.RegisterType<Gamer>()
            //    // 方法注入
            //    .OnActivating(e =>
            //    {
            //        IMouse mouse = e.Context.Resolve<IMouse>();
            //        e.Instance.Inject(mouse);
            //    })
            //    // 属性注入
            //    .PropertiesAutowired((g, o) => g.Name == "Keyboard")
            //    // 构造函数注入
            //    .UsingConstructor(typeof(IComputer))
            //    .As<IPerson>();
            //var container = builder.Build();
            //IPerson gamer = container.Resolve<IPerson>();
            //gamer.Work();

            //var builder = new ContainerBuilder();
            //builder.RegisterType<LenovoComputer>().As<IComputer>();
            //builder.RegisterType<TogarKeyboard>().As<IKeyboard>();
            //builder.RegisterType<LogitechMouse>().As<IMouse>();
            //builder.RegisterType<Programmer>().As<IPerson>();
            //var container = builder.Build();
            //IPerson programmer = container.Resolve<IPerson>();
            //programmer.Work();
        }

        static void TestUnity()
        {
            //ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            //fileMap.ExeConfigFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "DI-Unity.config");
            //Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            //UnityConfigurationSection section = (UnityConfigurationSection)configuration.GetSection(UnityConfigurationSection.SectionName);
            //IUnityContainer container = new UnityContainer();
            //section.Configure(container, "Unity_Container");
            //IPerson programmer = container.Resolve<IPerson>("Programmer");
            //programmer.Work();
            //IPerson gamer = container.Resolve<IPerson>("Gamer");
            //gamer.Work();

            //IUnityContainer container = new UnityContainer();
            //container.RegisterType<IComputer, LenovoComputer>(new PerThreadLifetimeManager());
            //IComputer computer0 = container.Resolve<IComputer>();
            //IComputer computer1 = container.Resolve<IComputer>();
            //IComputer computer2 = null;
            //Task.Run(() =>
            //{
            //    computer2 = container.Resolve<IComputer>();
            //}).Wait();
            //Console.WriteLine("computer0: " + computer0.GetHashCode());
            //Console.WriteLine("computer1: " + computer1.GetHashCode());
            //Console.WriteLine("computer2: " + computer2.GetHashCode());
            //Console.WriteLine($"computer0 == computer1: {computer0 == computer1}");
            //Console.WriteLine($"computer0 == computer2: {computer0 == computer2}");

            //IUnityContainer container = new UnityContainer();
            //container.RegisterType<IMouse, LogitechMouse>(new InjectionConstructor("502"));
            //IMouse mouse_502 = container.Resolve<IMouse>();
            //Console.WriteLine("Type: " + mouse_502.Type);
            //container.RegisterType<IMouse, LogitechMouse>(new InjectionConstructor("304"));
            //IMouse mouse_304 = container.Resolve<IMouse>();
            //Console.WriteLine("Type: " + mouse_304.Type);

            //IUnityContainer container = new UnityContainer();
            //container.RegisterType<IComputer, LenovoComputer>(); 
            //container.RegisterType<IKeyboard, TogarKeyboard>(); 
            //container.RegisterType<IMouse, LogitechMouse>(); 
            //container.RegisterType<IPerson, Programmer>("Programmer"); 
            //container.RegisterType<IPerson, Gamer>("Gamer"); 
            //IPerson programmer = container.Resolve<IPerson>("Programmer");
            //programmer.Work();
            //IPerson gamer = container.Resolve<IPerson>("Gamer");
            //gamer.Work();
        }
    }
}
