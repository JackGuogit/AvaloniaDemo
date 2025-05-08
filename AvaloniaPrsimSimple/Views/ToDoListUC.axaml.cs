using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Prism.Events;

namespace AvaloniaPrsimSimple.Views;

public partial class ToDoListUC : UserControl
{
    public ToDoListUC()
    {
        InitializeComponent();
    }
    public ToDoListUC(IEventAggregator eventAggregator) : this()
    {

    }
}