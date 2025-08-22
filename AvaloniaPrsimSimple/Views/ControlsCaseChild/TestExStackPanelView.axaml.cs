using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Threading;
using AvaloniaPrsimSimple.ViewModels;
using System;
using System.Collections.Specialized;

namespace AvaloniaPrsimSimple.Views;

public partial class TestExStackPanelView : UserControl
{
    private ItemsControl _itemsControl;

    public TestExStackPanelView()
    {
        InitializeComponent();
        this.AttachedToVisualTree += TestExStackPanelView_AttachedToVisualTree;
    }

    private void TestExStackPanelView_AttachedToVisualTree(object sender, VisualTreeAttachmentEventArgs e)
    {
        _itemsControl = this.FindControl<ItemsControl>("ItemsControlWithTransitions");

        if (DataContext is TestExStackPanelViewModel viewModel)
        {
            // 监听集合变化事件
            viewModel.StackPanelItems.CollectionChanged += StackPanelItems_CollectionChanged;
        }
    }

    private void StackPanelItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        // 当有新项添加时，确保动画效果正常显示
        if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
        {
            // 先添加.notransitions类以禁用过渡动画
            _itemsControl.Classes.Add("notransitions");
            
            // 使用调度器延迟执行，确保UI已更新
            Dispatcher.UIThread.Post(() =>
            {
                // 移除.notransitions类以启用过渡动画
                _itemsControl.Classes.Remove("notransitions");
            }, DispatcherPriority.Render);
        }
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);

        // 清理事件订阅
        if (DataContext is TestExStackPanelViewModel viewModel)
        {
            viewModel.StackPanelItems.CollectionChanged -= StackPanelItems_CollectionChanged;
        }
    }
}