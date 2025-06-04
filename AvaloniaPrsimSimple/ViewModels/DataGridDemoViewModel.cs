using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaPrsimSimple.ViewModels
{
    public class DataGridDemoViewModel:ViewModelBase
    {

        private string _title = "DataGrid Demo";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        private List<object> _items=new List<object>();
        public List<object> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }
        public DataGridDemoViewModel()
        {
            Items.Add(new People("John Doe", 30, "123 Main St"));
            Items.Add(new People("Jane Smith", 25, "456 Elm St"));
            Items.Add(new People("Alice Johnson", 28, "789 Oak St"));
            Items.Add(new People("Bob Brown", 35, "321 Pine St"));
            Items.Add(new People("Charlie White", 22, "654 Maple St"));
            Items.Add(new People("Diana Green", 40, "987 Cedar St"));
            Items.Add(new People("Ethan Blue", 29, "159 Spruce St"));
        }
    }

    public class People
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public People(string name, int age, string address)
        {
            Name = name;
            Age = age;
            Address = address;
        }
    }
}
