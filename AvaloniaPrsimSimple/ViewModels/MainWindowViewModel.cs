using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia.Input;
using AvaloniaPrsimSimple.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Prism.Common;

namespace AvaloniaPrsimSimple.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{

    public MainViewModel MainViewModel { get;  } = new MainViewModel();

    public ToDoListViewModel ToDoListViewModel { get;  } = new ToDoListViewModel();

    public ControlsCaseViewModel ControlsCaseViewModel { get;  } = new ControlsCaseViewModel();
    
    // public ObservableCollection<ViewModelBase> AllViewModels { get; set; } = new ObservableCollection<ViewModelBase>()
    //     .Add(new MainViewModel());

}
