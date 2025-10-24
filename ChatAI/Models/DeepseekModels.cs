using System.ComponentModel;

namespace ChatAI.Models
{
    public class ChatMessage : INotifyPropertyChanged
    {
        private string _content = string.Empty;

        public string Role { get; set; } = string.Empty;
        public string Content 
        { 
            get => _content;
            set
            {
                if (_content != value)
                {
                    _content = value;
                    OnPropertyChanged(nameof(Content));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}