using Akavache;
using Splat;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace AvaloniaCross.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public string Greeting { get; } = "Welcome to Avalonia!";

        public MainViewModel()
        {
            //IAkavacheInstance? akavacheInstance = Splat.Locator.Current.GetService<IAkavacheInstance>();

            //IBlobCache? localMachine = akavacheInstance.LocalMachine;
            //localMachine.InsertObject("sss", new
            //{
            //    sss = "sss",
            //    bbb = "aaa"
            //}).Wait();
            ////localMachine.Vacuum();
            //localMachine.Flush().Wait();
            //CacheDatabase.Shutdown().Wait();
            //IObservable<dynamic?> observable = localMachine.GetObject<dynamic>("sss");

            Task.Run(async () => await SaveDataAsync()).Wait();
        }

        public async Task SaveDataAsync()
        {
            // ✅ 使用Dictionary代替匿名对象
            var data = new Dictionary<string, object>
            {
                ["Name"] = "Test2",
                ["Value"] = 123
            };

            await CacheDatabase.LocalMachine.InsertObject("test3_key", data);
            await CacheDatabase.LocalMachine.Flush();

            // 反序列化为Dictionary
            var retrieved = await CacheDatabase.LocalMachine.GetObject<Dictionary<string, object>>("test3_key");
            Console.WriteLine($"Retrieved: {retrieved["Name"]}, {retrieved["Value"]}");
        }
    }
}