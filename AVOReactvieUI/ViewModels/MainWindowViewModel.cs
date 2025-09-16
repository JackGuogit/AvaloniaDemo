using ReactiveUI;
using System.Reactive.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AVOReactiveUI.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
#pragma warning disable CA1822 // Mark members as static
    public string Greeting => "Welcome to Avalonia!";
#pragma warning restore CA1822 // Mark members as static


    private readonly Interaction<string, bool> _showMessageInteraction=new Interaction<string, bool>();


    public ICommand ShowMainViewCommand => ReactiveCommand.CreateFromTask( async () =>
    {
        await ShowMessage("hello word");
    });
    public MainWindowViewModel(MainViewModel mainViewModel)
    {
        CurrentViewModel= mainViewModel;






    }
    public MainViewModel CurrentViewModel { get; set; } 


    public Interaction<string, bool> ShowMessageInteraction => _showMessageInteraction;


    public async Task ShowMessage(string message)
    {
        var result = await _showMessageInteraction.Handle(message);
        // 处理结果

        if (result != null) 
        {
            // 处理结果
        }
    }




}