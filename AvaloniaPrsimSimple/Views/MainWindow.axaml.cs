using Avalonia.Controls;
using AvaloniaPrsimSimple.ViewModels;
using Prism.Events;

namespace AvaloniaPrsimSimple.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public MainWindow(string title) : this()
    {
        Title = title;
    }

    public MainWindow(IEventAggregator eventAggregator) : this()
    {

    }
    
}