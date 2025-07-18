using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVOReactiveUI.Behavior
{
    public class ClosePopupBehavior : Behavior<Button>
    {
        public Popup? TargetPopup { get; set; }

        protected override void OnAttached()
        {
            AssociatedObject.Click += (_, __) =>
            {
                if (TargetPopup is not null)
                    TargetPopup.IsOpen = false;
            };
        }
    }
}
