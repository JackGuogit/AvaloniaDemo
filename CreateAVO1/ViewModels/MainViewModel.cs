using AVOReactiveUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using System.Reactive;

namespace AVOReactiveUI.ViewModels
{
    public class MainViewModel: ViewModelBase
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

        
        public string Username { set;get;}

        public ReactiveCommand<Unit, Unit> ButtonCommand;

        public MainViewModel()
        {
            Time = DateTime.Now.ToString();
            ButtonCommand = ReactiveCommand.Create(() =>
            {
                Time=DateTime.Now.ToString();
            });
            
        }

    }
}
