using BenchmarkDotNet.Attributes;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows.Input;

namespace AVOMvvm.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
#pragma warning disable CA1822 // Mark members as static

    private string greeting = "Welcome to Avalonia!";

    public string Greeting
    {
        get { return greeting; }
        set { this.SetProperty(ref greeting, value); }
    }

    public ICommand GreetingCommand { get; set; }

    public MainWindowViewModel()
    {
        GreetingCommand = new RelayCommand(GreetingM);
    }

    private int i = 1;

    private void GreetingM()
    {
        Greeting = Greeting + i++;
    }

#pragma warning restore CA1822 // Mark members as static
}