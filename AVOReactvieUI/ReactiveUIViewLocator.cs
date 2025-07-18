using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVOReactiveUI
{
    public class ReactiveUIViewLocator: IViewLocator
    {
        public IViewFor? ResolveView<T>(T viewModel, string? contract = null)
        {

            //IViewFor? viewFor = ViewLocator.Current.ResolveView(viewModel);


            var viewModelName = viewModel!.GetType().FullName!;
            
            var viewName = viewModelName
                     .Replace("MainWindowViewModel", "MainWindow")
                .Replace("ViewModel", "View")
                .Replace("ViewModels", "Views"); // 处理命名空间

            var viewType = Type.GetType(viewName);

            //var v = Locator.Current.GetService(viewType);
            if (viewType != null && typeof(IViewFor).IsAssignableFrom(viewType))
            {

                //IEnumerable<object> enumerable = Locator.Current.GetServices(typeof(IViewFor<object>));
                //object? v2 = enumerable;
                //object? v3 = Locator.Current.GetService(typeof(IViewFor), "MainView");

                return Activator.CreateInstance(viewType) as IViewFor;
            }


            throw new InvalidOperationException($"未找到实现 IViewFor 的视图：{viewName}");
        }
    }
}
