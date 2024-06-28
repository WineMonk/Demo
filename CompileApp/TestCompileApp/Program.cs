using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCompileApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CreateCodeDll1();
            Console.ReadKey();
        }
        public static void CreateCodeDll1()
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateExecutable = true;
            parameters.GenerateInMemory = false;
            parameters.OutputAssembly = "test.exe";
            parameters.ReferencedAssemblies.Add("System.dll");
            string sourceFile = @"
                        using System;
                        public class MyCode
                        {
                            public static void Main(string[] args)
                            {
                                Console.Write(""i'm running!"");
                                Console.Read();
                            }
                        }
                        ";
            CompilerResults cr = provider.CompileAssemblyFromSource(parameters, sourceFile);
            if (cr.Errors.Count > 0)
            {
                Console.Write("Errors building " + sourceFile + " into " + cr.PathToAssembly);
                foreach (CompilerError ce in cr.Errors)
                {
                    Console.Write(ce.ToString());
                }
            }
            else
            {
                Console.Write("编译成功");
            }
        }
    }
}
