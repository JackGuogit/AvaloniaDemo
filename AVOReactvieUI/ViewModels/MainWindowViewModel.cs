using ReactiveUI;
using System.Security.Cryptography.X509Certificates;

namespace AVOReactiveUI.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
#pragma warning disable CA1822 // Mark members as static
    public string Greeting => "Welcome to Avalonia!";
#pragma warning restore CA1822 // Mark members as static
    public MainWindowViewModel(MainViewModel mainViewModel)
    {
        CurrentViewModel= mainViewModel;
    }
    public MainViewModel CurrentViewModel { get; set; } 
}