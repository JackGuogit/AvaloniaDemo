using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AvaloniaPrsimSimple.Views;

public partial class MessageDialog : Window
{
    private Point _dragStartPoint;
    public MessageDialog()
    {
        InitializeComponent();
    }
    
    public static void Show(string message, Window owner = null)
    {
        var dialog = new MessageDialog
        {
            MessageText = { Text = message },

        };
        if (owner != null)
        {
            dialog.Owner = owner;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner; // 确保对话框在Owner窗口居中
        }
  
        dialog.ShowDialog(owner);
    }

    public static void Show(string title, string message, Window owner = null)
    {


        Show(message, owner);
    }

    private void OnOkClicked(object sender, RoutedEventArgs e)
    {
        Close();
    }
    private void OnTitleBarMouseDown(object sender, PointerPressedEventArgs e)
    {
        // 获取鼠标按下时的初始位置
        _dragStartPoint = e.GetPosition(this);

        // 注册鼠标移动事件
        this.PointerMoved += OnTitleBarMouseMoved;
        this.PointerReleased += OnTitleBarMouseReleased;
    }

    private void OnTitleBarMouseMoved(object sender, PointerEventArgs e)
    {
        var delta = e.GetPosition(this) - _dragStartPoint;

        // 更新窗口位置，确保使用 PixelPoint
        this.Position = new PixelPoint(
            (int)(this.Position.X + delta.X),
            (int)(this.Position.Y + delta.Y)
        );
    }

    private void OnTitleBarMouseReleased(object sender, PointerReleasedEventArgs e)
    {
        // 取消鼠标事件监听
        this.PointerMoved -= OnTitleBarMouseMoved;
        this.PointerReleased -= OnTitleBarMouseReleased;
    }
}