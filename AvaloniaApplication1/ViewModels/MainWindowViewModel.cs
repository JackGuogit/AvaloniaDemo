using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AvaloniaApplication1.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
#pragma warning disable CA1822 // Mark members as static
    public string Greeting => "Welcome to Avalonia!";
#pragma warning restore CA1822 // Mark members as static

    public ICommand ClickCommand { get; }

    public ObservableCollection<string> Items { get; } = new();

    public MainWindowViewModel()
    {
        ClickCommand = new RelayCommand(OnClick);
    }

    private void OnClick()
    {
    }
}