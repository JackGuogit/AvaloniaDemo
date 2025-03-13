using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using AVOReactiveUI.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace AVOReactiveUI.Views;

public partial class MainView : ReactiveUserControl<MainViewModel>
{
    public MainView()
    {
        InitializeComponent();



        this.WhenActivated(disposables =>
        {
            this.BindCommand(ViewModel, vm => vm.ButtonCommand, v => v.button).DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.Time, v => v.textBlock.Text);

        });
    
    }
}