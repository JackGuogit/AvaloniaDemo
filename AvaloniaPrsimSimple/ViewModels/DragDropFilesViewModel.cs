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







            // ����Ƿ����ļ��Ϸ�
            if (routedEvent.Data.Contains("FileDropList"))
            {
                // ��ȡ�ļ�·���б�
                var fileDropList = routedEvent.Data.Get("FileDropList") as string[];
                if (fileDropList != null)
                {
                    // �����ļ�·��
                    foreach (var filePath in fileDropList)
                    {
                        Console.WriteLine("Dropped File: " + filePath);
                        // ����������ﴦ���ļ�·��
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