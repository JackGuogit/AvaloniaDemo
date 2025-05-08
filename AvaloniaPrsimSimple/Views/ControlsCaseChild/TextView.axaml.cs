using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace AvaloniaPrsimSimple.Views;

public partial class TextView : UserControl
{
    public TextView()
    {
        InitializeComponent();
    }
    private void TextBox_PointerPressed(object sender, PointerPressedEventArgs e)
    {
        if (e.ClickCount == 2)
        {
            var textBox = (TextBox)sender;
            textBox.IsReadOnly = false;
            textBox.Focus(); // 确保TextBox获得焦点
        }
    }

    private void TextBox_KeyDown(object sender, KeyEventArgs e)
    {
        var textBox = (TextBox)sender;
        textBox.IsReadOnly = false;
    }
}