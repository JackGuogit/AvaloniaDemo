using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows.Input;

namespace AvaloniaPrsimSimple.ViewModels;

public class TextViewModel:ViewModelBase
{

    public ICommand ChangeTextCommand { get; }
    private bool _isTextVisible = true;
    public bool IsTextVisible
    {
        get => _isTextVisible;
        set
        {
            _isTextVisible = value;
            OnPropertyChanged(nameof(IsTextVisible));
        }
    }
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

    public TextViewModel()
    {
        ChangeTextCommand = new RelayCommand(ChangeText);
    }
    int i = 0;
    private void ChangeText()
    {
        Text = "sssss" + i++;
        IsTextVisible = !IsTextVisible;
    }
}