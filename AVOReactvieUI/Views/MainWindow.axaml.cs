using Avalonia;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using AVOReactiveUI.ViewModels;

namespace AVOReactiveUI.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
    }


}