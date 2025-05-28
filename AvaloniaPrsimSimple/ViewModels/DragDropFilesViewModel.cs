using Avalonia.Input;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;


namespace AvaloniaPrsimSimple.ViewModels
{
	public partial class DragDropFilesViewModel : ViewModelBase
    {

        private ObservableCollection<string> _files=new ObservableCollection<string>();
        public ObservableCollection<string> Files
        {
            get => _files;
            set => SetProperty(ref _files, value);
        }



		[RelayCommand]
		public void GetDragDropFiles(DragEventArgs routedEvent)
		{
            IDataObject data = routedEvent.Data;
            string? v = data.GetText();
            IEnumerable<Avalonia.Platform.Storage.IStorageItem>? enumerable = data.GetFiles();


            foreach (var item in enumerable)
            {
                Console.WriteLine(item.Name);
                Files.Add(item.Name);
            }







            // 检查是否是文件拖放
            if (routedEvent.Data.Contains("FileDropList"))
            {
                // 获取文件路径列表
                var fileDropList = routedEvent.Data.Get("FileDropList") as string[];
                if (fileDropList != null)
                {
                    // 遍历文件路径
                    foreach (var filePath in fileDropList)
                    {
                        Console.WriteLine("Dropped File: " + filePath);
                        // 你可以在这里处理文件路径
                    }
                }
                else
                {
                    Console.WriteLine("No files found in the drop data.");
                }
            }
            else
            {
                Console.WriteLine("Dropped data is not a file list.");
            }

        }


	}
}