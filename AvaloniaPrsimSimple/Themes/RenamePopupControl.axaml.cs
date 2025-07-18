using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using System;

namespace AvaloniaPrsimSimple;

public class RenamePopupControl : TemplatedControl
{
    public static readonly StyledProperty<bool> IsOpenProperty =
        AvaloniaProperty.Register<RenamePopupControl, bool>(nameof(IsOpen));

    public static readonly StyledProperty<string> CurrentNameProperty =
        AvaloniaProperty.Register<RenamePopupControl, string>(nameof(CurrentName));

    public event Action<string>? RenameConfirmed;

    public bool IsOpen
    {
        get => GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    public string CurrentName
    {
        get => GetValue(CurrentNameProperty);
        set => SetValue(CurrentNameProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        var confirmButton = e.NameScope.Find<Button>("PART_Confirm");
        var cancelButton = e.NameScope.Find<Button>("PART_Cancel");
        var inputBox = e.NameScope.Find<TextBox>("PART_Input");

        confirmButton.Click += (_, __) =>
        {
            RenameConfirmed?.Invoke(inputBox.Text ?? "");
            IsOpen = false;
        };

        cancelButton.Click += (_, __) =>
        {
            IsOpen = false;
        };
    }
}
