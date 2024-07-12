using Avalonia;
using ReactiveUI;
using SukiUI.MessageBox;
using System.Windows.Input;

namespace TestAvalonia.ViewModels;

public class MainViewModel : ViewModelBase
{
    public string Greeting => "Welcome to Avalonia!";

    public ICommand StartCommand { get; set; }

    public MainViewModel()
    {
        StartCommand = ReactiveCommand.Create(Start);
    }

    private void Start()
    {
        MessageBox messageBox = new MessageBox("提示", "开始你的表演！");
        messageBox.Show();
    }
}
