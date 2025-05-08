using CommunityToolkit.Mvvm.Input;

namespace AvaloniaPrsimSimple.ViewModels;

public partial class ControlsCaseViewModel:ViewModelBase
{
    
    private ViewModelBase _selectedViewModel;

    public ViewModelBase SelectedViewModel
    {
        get => _selectedViewModel;
        set
        {
            _selectedViewModel = value;
            OnPropertyChanged(nameof(SelectedViewModel));
        }
    }





    [RelayCommand]
    public void Select(ViewModelBase viewModel)
    {
        SelectedViewModel = viewModel;
    }
}