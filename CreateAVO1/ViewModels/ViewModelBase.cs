using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using System.Collections.Generic;

namespace AVOReactiveUI.ViewModels;

public class ViewModelBase : ReactiveObject, IActivatableViewModel, IValidatableViewModel
{
    public ViewModelActivator Activator => new ViewModelActivator();

    public IValidationContext ValidationContext => new ValidationContext();

    //public ValidationContext ValidationContext { get; } = new ValidationContext();

    //IValidationContext IValidatableViewModel.ValidationContext => ValidationContext;
}