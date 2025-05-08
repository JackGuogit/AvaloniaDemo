using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using System.Diagnostics;

namespace AvaloniaPrsimSimple.Views;

public partial class ControlsCaseButtonView : UserControl
{
    public ControlsCaseButtonView()
    {
        InitializeComponent();
        
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var visualRoot = this.GetVisualRoot() as Window;
        MessageDialog.Show("This is a message dialog!",visualRoot);


        string a = "122";
        string b =  a;
        b = "abc";
        ChangeStr(ref a);
        Debug.WriteLine(a);
        Debug.WriteLine(b);

        b.Replace("a", "b");
        
        Debug.WriteLine(b);


        int c = 1;
        changeInt(ref c);
        Debug.WriteLine(c);


    }

    public void ChangeStr(ref string str)
    {
        str = "change";

    }

    public void changeInt(ref int a)
    {
        a = 2;
    }
}