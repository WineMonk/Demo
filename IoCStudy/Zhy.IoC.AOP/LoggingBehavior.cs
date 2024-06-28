
using Castle.DynamicProxy;

public class LoggingInterceptor : IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        Console.WriteLine($"Invoking method {invocation.Method.Name} at {DateTime.Now}");
        invocation.Proceed();
        Console.WriteLine($"Method {invocation.Method.Name} has completed at {DateTime.Now}");
    }
}