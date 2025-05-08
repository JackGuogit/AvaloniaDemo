using CommunityToolkit.Mvvm.Input;

namespace AvaloniaPrsimSimple.ViewModels;

public partial class MainViewModel:ViewModelBase
{

    private string _greeting = "Welcome to Avalonia!";
    public string Greeting {
        get
        {
            return _greeting;
        }
        set
        {
            _greeting = value;
            OnPropertyChanged(nameof(Greeting));
        }
        
    }
    
    [RelayCommand]
    public void SetGreet()
    {
        Greeting = $"Hello, {Greeting}!";
    }

    public MainViewModel()
    {
        // var pubSubEvent = eventAggregator.GetEvent<PubSubEvent<string>>();
        // pubSubEvent.Subscribe((string s) =>
        // {
        //     var dataContext = DataContext as MainWindowViewModel;
        //     dataContext.Greeting = s;
        // });
        //
        // pubSubEvent.Publish("Hello Avalonia!");   
    }
}