using Akavache;
using Splat;
using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

namespace AvaloniaCross.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public string Greeting { get; } = "Welcome to Avalonia!";

        public MainViewModel()
        {
            IAkavacheInstance? akavacheInstance = Splat.Locator.Current.GetService<IAkavacheInstance>();

            IBlobCache? localMachine = akavacheInstance.LocalMachine;
            localMachine.InsertObject("sss", new
            {
                sss = "sss",
                bbb = "aaa"
            });
            //localMachine.Vacuum();
            IObservable<System.Reactive.Unit> observable1 = localMachine.Flush();



            IObservable<dynamic?> observable = localMachine.GetObject<dynamic>("sss");
        }
    }
}