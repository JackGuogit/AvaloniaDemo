using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;

namespace AvaloniaFormTest;

public partial class SelectFolderForm : Window
{
    public string SelecteFolderPath => FolderPath.Text;

    public SelectFolderForm()
    {
        InitializeComponent();
    }

    private async void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var folderPickerOptions = new FolderPickerOpenOptions
        {
            AllowMultiple = false
        };

        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel is null)
        {
            return;
        }

        var selectedFolders = await topLevel.StorageProvider.OpenFolderPickerAsync(folderPickerOptions);

        if (selectedFolders != null && selectedFolders.Count == 1)
        {
            IStorageFolder selectedFolder = selectedFolders[0];
            // 获取文件夹名称和路径（注意平台兼容性）
            string folderPath = selectedFolder.Path.LocalPath;
            FolderPath.Text = folderPath;
        }
        else;
        {
            FolderPath.Text = string.Empty;
        }
    }
}