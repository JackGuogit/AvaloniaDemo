using AVOReactiveUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using System.Reactive;
using Avalonia.ReactiveUI;
using System.Diagnostics;
using Avalonia.Threading;
using ReactiveUI.SourceGenerators;
using DynamicData;

namespace AVOReactiveUI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string name;

        public string Name
        {
            get => name;
            set => this.RaiseAndSetIfChanged(ref name, value);
        }

        private string time;

        public string Time
        {
            get => time;
            set => this.RaiseAndSetIfChanged(ref time, value);
        }

        [Reactive]
        public Guid Id { set; get; } = Guid.NewGuid();

        private string username;

        public string Username
        {
            get => username;
            set => this.RaiseAndSetIfChanged(ref username, value);
        }

        //[Reactive]
        //public string Username { get; set; }

        private string textGuid;

        public string TextGuid
        {
            get => textGuid;
            set => this.RaiseAndSetIfChanged(ref textGuid, value);
        }

        private SourceList<string> items = new SourceList<string>();
        public SourceList<string> Items => items;

        public ReactiveCommand<Unit, Unit> ButtonCommand;
        public ReactiveCommand<Guid, Unit> ShowGuidCommand { get; }
        public ReactiveCommand<Unit, Unit> AddItemCommand { get; }

        public MainViewModel()
        {
            Time = DateTime.Now.ToString();
            ButtonCommand = ReactiveCommand.Create(() =>
            {
                //if (RxApp.MainThreadScheduler is AvaloniaScheduler)
                //{
                //    Debug.WriteLine("调度器已正确绑定到 Avalonia UI 线程");
                //}
                //else
                //{
                //    throw new InvalidOperationException("ReactiveUI 调度器未初始化");
                //}
                Time = DateTime.Now.ToString();
            });
            ShowGuidCommand = ReactiveCommand.Create<Guid>(guid =>
            {
                TextGuid = guid.ToString();
                Username = "lllllllllll";
            });
            Items.Connect().OnItemAdded(item =>
            {
                Debug.WriteLine($"Item added: {item}");
            }).Subscribe();
            AddItemCommand = ReactiveCommand.Create(() =>
            {
                Items.Add($"Item {Items.Count + 1}");
            });
        }
    }
}