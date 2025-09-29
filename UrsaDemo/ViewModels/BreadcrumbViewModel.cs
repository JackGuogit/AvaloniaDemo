using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Ursa.Controls;

namespace UrsaDemo.ViewModels
{
    public class BreadcrumbViewModel : ViewModelBase
    {
        private ObservableCollection<BreadcrumbItem> _breadcrumbItems = new ObservableCollection<BreadcrumbItem>();

        public ObservableCollection<BreadcrumbItem> BreadcrumbItems
        {
            get => _breadcrumbItems;
            set => this.RaiseAndSetIfChanged(ref _breadcrumbItems, value);
        }

        public ICommand ActiveCommand { get; set; }

        public BreadcrumbViewModel()
        {
            ActiveCommand = ReactiveCommand.Create<object>((o) =>
            { 
                IEnumerable<BreadcrumbItem> enumerable = BreadcrumbItems.Where(b => b.Equals(o));
                
                // Handle breadcrumb item click
                Debug.WriteLine($"Breadcrumb '{o}' clicked.");
            });
            BreadcrumbItems.Add(new BreadcrumbItem("Home", "M10 20v-6h4v6h5v-8h3L12 3 2 12h3v8z", ActiveCommand, false));
            BreadcrumbItems.Add(new BreadcrumbItem("Category", "M3 13h2v-2H3v2zm0 4h2v-2H3v2zm0-8h2V7H3v2zm4 4h14v-2H7v2zm0 4h14v-2H7v2zm0-8h14V7H7v2z", ActiveCommand, false));
            BreadcrumbItems.Add(new BreadcrumbItem("Subcategory", "M12 2C8.13 2 5 5.13 5 9c0 5.25 7 13 7 13s7-7.75 7-13c0-3.87-3.13-7-7-7zm0 9.5c-1.38 0-2.5-1.12-2.5-2.5s1.12-2.5 2.5-2.5 2.5 1.12 2.5 2.5-1.12 2.5-2.5 2.5z", ActiveCommand, true));
        }

        private void GotoCommand()
        {
        }
    }

    public class BreadcrumbItem : ViewModelBase
    {
        public string Text { get; set; }
        public string Icon { get; set; }
        public bool IsActive { get; set; }
        public ICommand ActiveCommand { get; set; }
        private bool _isReadOnly = default!;

        public bool IsReadOnly
        {
            get => _isReadOnly;
            set
            {
                this.RaiseAndSetIfChanged(ref _isReadOnly, value);
            }
        }

        public BreadcrumbItem(string text, string icon, ICommand command, bool isActive = false)
        {
            Text = text;
            Icon = icon;
            IsActive = isActive;
            ActiveCommand = command;
        }
    }
}