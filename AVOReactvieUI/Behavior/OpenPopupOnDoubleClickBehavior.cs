using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVOReactiveUI.Behavior
{
    public class OpenPopupOnDoubleClickBehavior : Behavior<TextBox>
    {
        public Popup? Popup { get; set; }

        protected override void OnAttached()
        {
            AssociatedObject.AddHandler(InputElement.PointerPressedEvent, OnPointerPressed, RoutingStrategies.Tunnel);
        }

        private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (e.ClickCount == 2 && Popup is not null)
            {
                Popup.IsOpen = true;
                if (Popup.FindControl<TextBox>("InputBox") is { } input)
                {
                    input.Text = (AssociatedObject.Text ?? "");
                    input.Focus();
                    input.SelectionStart = 0;
                    input.SelectionEnd = input.Text.Length;
                }
            }
        }
    }

}
