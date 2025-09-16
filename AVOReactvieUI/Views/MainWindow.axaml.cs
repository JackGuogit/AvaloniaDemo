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
            // ���� ShowMessageInteraction ����
            ViewModel?.ShowMessageInteraction.RegisterHandler(async interaction =>
            {
                var message = interaction.Input;
                // ��ʾ��Ϣ�Ի���
                var result = true;
                // ���ؽ���� ViewModel
                interaction.SetOutput(result);
            }).DisposeWith(disposables);
        });

    }


}