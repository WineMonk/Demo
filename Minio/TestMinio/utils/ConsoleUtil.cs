using MinioTest.entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinioTest.utils
{
    public  class ConsoleUtil
    {
        public static ObservableCollection<ConsoleRow> ConsoleRows {  get; set; } 
            = new ObservableCollection<ConsoleRow>();

        public static void Info(string text)
        {
            WriteRow(DateTime.Now.ToString(), "Info", text);
        }

        public static void Error(string text) 
        {
            WriteRow(DateTime.Now.ToString(), "Error", text);
        }

        public static void Clear()
        {
            ConsoleRows.Clear();
        }

        private static void WriteRow(string datetime, string level, string text)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                ConsoleRows.Add(new ConsoleRow { DateTime = datetime, Level = level, Text = text });
            });
        }
    }
}
