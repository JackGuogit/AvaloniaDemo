using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;

namespace AvaloniaPrsimSimple.Views;

public partial class DragDropFilesView : UserControl
{
    public DragDropFilesView()
    {
        InitializeComponent();
    }

    private void Border_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        var ctl = sender as Control;
        if (ctl != null)
        {
            FlyoutBase.ShowAttachedFlyout(ctl);
        }
    }
}