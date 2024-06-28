using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinioTest.entities
{
    public partial class FileItem:ObservableObject
    {
        [ObservableProperty]
        private string _fileName;
        [ObservableProperty]
        private string _fileUrl;
    }
}
