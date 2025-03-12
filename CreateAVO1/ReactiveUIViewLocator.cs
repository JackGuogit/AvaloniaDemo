using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVOReactvieUI
{
    public class ReactiveUIViewLocator: IViewLocator
    {
        public IViewFor? ResolveView<T>(T viewModel, string? contract = null)
        {
            var viewModelName = viewModel!.GetType().FullName!;
            var viewName = viewModelName
                     .Replace("MainWindowViewModel", "MainWindow")
                .Replace("ViewModel", "View")
                .Replace("ViewModels", "Views"); // 处理命名空间

            var viewType = Type.GetType(viewName);
            if (viewType != null && typeof(IViewFor).IsAssignableFrom(viewType))
            {
                return Activator.CreateInstance(viewType) as IViewFor;
            }

            throw new InvalidOperationException($"未找到实现 IViewFor 的视图：{viewName}");
        }
    }
}
