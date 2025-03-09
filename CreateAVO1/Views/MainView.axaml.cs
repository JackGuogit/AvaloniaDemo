using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using AVOReactiveUI.ViewModels;
using ReactiveUI;

namespace AVOReactiveUI.Views;

public partial class MainView : ReactiveUserControl<MainViewModel>
{
    public MainView()
    {
        InitializeComponent();
        this.BindCommand(ViewModel, vm => vm.ButtonCommand, v => v.button);
        this.WhenActivated(disposables =>
        {
            this.Bind(ViewModel, vm => vm.Time, v => v.textBlock.Text);
        });
    
    }
}