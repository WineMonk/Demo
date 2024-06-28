namespace TestCAD
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string dwgFile = "data/test.dwg";
            CadDocReader cadDocReader = new CadDocReader(dwgFile);
            cadDocReader.ReadTable();
            Console.Read();
        }
    }
}
