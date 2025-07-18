using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVOReactiveUI.Behavior
{
    public class CopyTextBehavior : Behavior<Button>
    {
        public TextBox? Source { get; set; }
        public TextBox? Target { get; set; }

        protected override void OnAttached()
        {
            AssociatedObject.Click += (_, __) =>
            {
                if (Source is not null && Target is not null)
                    Target.Text = Source.Text;
            };
        }
    }
}
