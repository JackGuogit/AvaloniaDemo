using Avalonia;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using AVOReactiveUI.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace AVOReactiveUI.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            // 处理 ShowMessageInteraction 交互
            ViewModel?.ShowMessageInteraction.RegisterHandler(async interaction =>
            {
                var message = interaction.Input;
                // 显示消息对话框
                var result = true;
                // 返回结果给 ViewModel
                interaction.SetOutput(result);
            }).DisposeWith(disposables);
        });

    }


}