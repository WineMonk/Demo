using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelLocator.ViewModels.vm1
{
    public class TestWindowViewModel : BindableBase
    {
        private string _content = "Test";
        public string Content
        {
            get { return _content; }
            set { SetProperty(ref _content, value); }
        }
    }
}
