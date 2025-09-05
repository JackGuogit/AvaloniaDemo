using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AvaloniaPrsimSimple.ViewModels
{
    public class TestExStackPanelViewModel:ViewModelBase
    {


        
        private ObservableCollection<string> _items;
        public ObservableCollection<string> StackPanelItems
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged(nameof(StackPanelItems));
            }
        }

        public ICommand AddItemCommand { get; }
        public ICommand RemoveItemCommand { get; }

        public TestExStackPanelViewModel()
        {

            AddItemCommand= new RelayCommand(() =>
            {
                var newItem = $"Item {StackPanelItems.Count + 1}";
                StackPanelItems.Add(newItem);
                OnPropertyChanged(nameof(StackPanelItems));
            });


            RemoveItemCommand = new RelayCommand(() =>
            {
                if (StackPanelItems.Any())
                {
                    StackPanelItems.RemoveAt(StackPanelItems.Count - 1);
                    OnPropertyChanged(nameof(StackPanelItems));
                }
            });


            StackPanelItems = new ObservableCollection<string>
            {

            };
        }


    }
}
