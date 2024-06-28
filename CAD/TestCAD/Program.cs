namespace TestCAD
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string dwgFile = "D:\\admin\\Desktop\\测试图-测绘&dfx&表格\\8东8支8北\\01-夏茅站结构总平面图.dwg";
            CadDocReader cadDocReader = new CadDocReader(dwgFile);
            cadDocReader.ReadTable();
            Console.Read();
        }
    }
}
