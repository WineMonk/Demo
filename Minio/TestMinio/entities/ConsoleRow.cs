using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinioTest.entities
{
    public partial class ConsoleRow: ObservableObject
    {
        [ObservableProperty]
        private string _dateTime;
        [ObservableProperty]
        private string _level;
        [ObservableProperty]
        private string _text;
    }
}
