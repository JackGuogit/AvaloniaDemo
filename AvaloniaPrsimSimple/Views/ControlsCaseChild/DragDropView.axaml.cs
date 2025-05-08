using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;

namespace AvaloniaPrsimSimple.Views;

public partial class DragDropView : UserControl
{

    private bool IsPopupPointerOver { get; set; }


    public DragDropView()
    {
        InitializeComponent();
        InitializeDrag();
    }

    private Point _startPoint;
    private bool _isDragging = false;
    private DispatcherTimer _timer;
    private void InitializeDrag()
    {
        // 鼠标按下
        DraggableBorder.PointerPressed += (s, e) =>
        {
            if (!e.GetCurrentPoint(DraggableBorder).Properties.IsLeftButtonPressed) return;
            _isDragging = true;
            _startPoint = e.GetPosition(MyCanvas); 
            PopupBorder.IsVisible = true; // 显示弹出控件
            e.Handled = true; // 标记事件已处理
            
            
            
            _timer.Start();
        };

        // 鼠标抬起
        DraggableBorder.PointerReleased += (s, e) =>
        {
            if (!_isDragging) return;
            _isDragging = false;
            // PopupBorder.IsVisible = false;
            e.Handled = true; // 标记事件已处理
            _timer.Stop();
        };

        // 鼠标移动
        DraggableBorder.PointerMoved += (s, e) =>
        {
            if (_isDragging)
            {
                var currentPosition = e.GetPosition(MyCanvas); // 相对于Canvas获取当前点
                var deltaX = currentPosition.X - _startPoint.X;
                var deltaY = currentPosition.Y - _startPoint.Y;

                var position = e.GetPosition(DraggableBorder);

                // Console.WriteLine((position));

                Canvas.SetLeft(DraggableBorder, currentPosition.X+deltaX-position.X);
                Canvas.SetTop(DraggableBorder,  currentPosition.Y+deltaY-position.Y);

                Point DraggableBorderCenter = new Point(Canvas.GetLeft(DraggableBorder)+DraggableBorder.Width/2,Canvas.GetTop(DraggableBorder)+DraggableBorder.Height/2);

                Console.WriteLine(PopupBorder.Bounds.Width);
                Canvas.SetLeft(PopupBorder,DraggableBorderCenter.X-PopupBorder.Bounds.Width/2);
                Canvas.SetTop(PopupBorder,  DraggableBorderCenter.Y+DraggableBorder.Bounds.Height/2);


                Console.WriteLine(PART_Popup.Bounds.Width);
                Canvas.SetLeft(PART_Popup,DraggableBorderCenter.X-PART_Popup.Bounds.Width/2);
                Canvas.SetTop(PART_Popup,  DraggableBorderCenter.Y+DraggableBorder.Bounds.Height/2);
                
                
                
                _startPoint = currentPosition; // 更新_startPoint为当前位置
                e.Handled = true;
            }
        };


        // 初始化计时器
        _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(10) };
        _timer.Tick += (s, e) =>
        {
            var currentLeft = Canvas.GetLeft(DraggableBorder);
            var currentTop = Canvas.GetTop(DraggableBorder);
            Canvas.SetLeft(DraggableBorder, currentLeft + (_startPoint.X - _startPoint.X));
            Canvas.SetTop(DraggableBorder, currentTop + (_startPoint.Y - _startPoint.Y));

            var popupLeft = Canvas.GetLeft(PopupBorder);
            var popupTop = Canvas.GetTop(PopupBorder);
            Canvas.SetLeft(PopupBorder, popupLeft + (_startPoint.X - _startPoint.X));
            Canvas.SetTop(PopupBorder, popupTop + (_startPoint.Y - _startPoint.Y));
        };
    }

    private void MyBorder_PointerEnter(object sender, PointerEventArgs e)
    {
        MyPopup.IsOpen = true;
    }

    private void MyBorder_PointerLeave(object sender, PointerEventArgs e)
    {
        MyPopup.IsOpen = false;
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        MyBorder = this.FindControl<Border>("MyBorder");
        MyPopup = this.FindControl<Popup>("MyPopup");
        PopupBorder = this.FindControl<Border>("PopupBorder");
        DraggableBorder = this.FindControl<Border>("DraggableBorder");
        MyCanvas = this.FindControl<Canvas>("MyCanvas");
        Part_TextBox = this.FindControl<TextBox>("Part_TextBox");
        PART_Popup = this.FindControl<Popup>("PART_Popup");
        // MyPopup.IsLightDismissEnabled = true;

        
    }

    private void Border_PointerEntered(object sender, PointerEventArgs e)
    {
        MyPopup.IsOpen = true;
        IsPopupPointerOver = true;
    }

    private void Border_PointerExited(object sender, PointerEventArgs e)
    {
        if (!IsPopupPointerOver)
        {
            MyPopup.IsOpen = false;
        }

    }


    private void Button_Click(object sender, RoutedEventArgs e)
    {
        // 处理按钮点击事件
        var button = (Button)sender;
        button.Content = "已点击";
        //_isDragging = true;
        // 可以在这里添加更多的逻辑
    }

    private void Part_TextBox_OnSizeChanged(object? sender, SizeChangedEventArgs e)
    {
        Point DraggableBorderCenter = new Point(Canvas.GetLeft(DraggableBorder)+DraggableBorder.Width/2,Canvas.GetTop(DraggableBorder)+DraggableBorder.Height/2);

        Console.WriteLine(PopupBorder.Bounds.Width);
        Canvas.SetLeft(PopupBorder,DraggableBorderCenter.X-PopupBorder.Bounds.Width/2);
        Canvas.SetTop(PopupBorder,  DraggableBorderCenter.Y+DraggableBorder.Bounds.Height/2);
    }
}