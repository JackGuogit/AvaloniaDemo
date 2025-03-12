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
    // 必须声明 AvaloniaProperty 支持数据绑定
    //public static readonly StyledProperty<MainWindowViewModel?> ViewModelProperty =
    //    AvaloniaProperty.Register<MainWindow, MainWindowViewModel?>(nameof(ViewModel));

}