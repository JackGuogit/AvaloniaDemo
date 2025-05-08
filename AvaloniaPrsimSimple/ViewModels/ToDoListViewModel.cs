using CommunityToolkit.Mvvm.Input;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AvaloniaPrsimSimple.ViewModels
{
    public partial class ToDoListViewModel: ViewModelBase
    {
        
        public ObservableCollection<TodoItem> TodoItems { get; set; } = new ObservableCollection<TodoItem>();
        public ICommand DeleteCommand { get; private set; }
        public string newTodoItem;
        public string NewToDoItem 
        { 
            get => newTodoItem; 
            set 
            {
                newTodoItem = value;
                OnPropertyChanged(nameof(NewToDoItem));
            } 
        }
        private void DeleteItem(TodoItem item)
        {
            if (item != null)
            {
                TodoItems.Remove(item);
            }
        }
        [RelayCommand]
        public void AddToDoItem ()
        {
            var item = new TodoItem { Content = NewToDoItem };

            if (string.IsNullOrEmpty(NewToDoItem))
            {
                return;
            }
            NewToDoItem=string.Empty;

            item.DeleteCommand = new DelegateCommand<TodoItem>(DeleteItem);

            TodoItems.Add(item);
        }


        [RelayCommand]
        public void DeleteToDoItem (TodoItem item)
        {
            if (item != null)
            {
                TodoItems.Remove(item);
            }
        }

    }

    public class TodoItem: ViewModelBase
    {
        public string Content { get; set; }
        public bool IsCheck { get; set; }

        public ICommand DeleteCommand { get; set; }
    }

}
