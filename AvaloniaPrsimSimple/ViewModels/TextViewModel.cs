namespace AvaloniaPrsimSimple.ViewModels;

public class TextViewModel:ViewModelBase
{
    private string _text = "Welcome to Avalonia!";
    public string Text {
        get
        {
            return _text;
        }
        set
        {
            _text = value;
            OnPropertyChanged(nameof(Text));
        }
        
    }

}