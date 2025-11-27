using Avalonia.Controls;

namespace AvaloniaFormTest.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {

            var selectFolderForm = new SelectFolderForm();
            selectFolderForm.ShowDialog(this);

        }
    }
}