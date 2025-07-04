using Avalonia;
using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AvaloniaPrsimSimple.Controls
{
    public class StackPanelEx : StackPanel
    {
        /// <summary>
        /// Defines the spacing between items when there are 3 or more.
        /// </summary>
        public static readonly StyledProperty<double> SpacingProperty =
            AvaloniaProperty.Register<StackPanelEx, double>(nameof(Spacing), 4d);

        /// <summary>
        /// Gets or sets the spacing between items when there are 3 or more.
        /// </summary>
        public double Spacing
        {
            get => GetValue(SpacingProperty);
            set => SetValue(SpacingProperty, value);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var children = Children;
            int childCount = children.Count;
            double spacing = Spacing;

            if (childCount == 0)
            {
                return finalSize;
            }

            if (childCount == 1)
            {
                // Case 1: One item, centered
                var child = children[0];
                var childSize = child.DesiredSize;
                double x = (finalSize.Width - childSize.Width) / 2;
                double y = (finalSize.Height - childSize.Height) / 2;
                child.Arrange(new Rect(x, y, childSize.Width, childSize.Height));
            }
            else if (childCount == 2)
            {
                // Case 2: Two items, top and bottom
                var firstChild = children[0];
                var secondChild = children[1];
                var firstChildSize = firstChild.DesiredSize;
                var secondChildSize = secondChild.DesiredSize;

                double y1 = 0;
                double y2 = finalSize.Height - secondChildSize.Height;

                firstChild.Arrange(new Rect((finalSize.Width - firstChildSize.Width) / 2, y1, firstChildSize.Width, firstChildSize.Height));
                secondChild.Arrange(new Rect((finalSize.Width - secondChildSize.Width) / 2, y2, secondChildSize.Width, secondChildSize.Height));
            }
            else
            {
                double totalChildrenHeight = 0;
                foreach (var child in children)
                {
                    totalChildrenHeight += child.DesiredSize.Height;
                }

                double availableSpace;
                double currentY;

                if (childCount == 3)
                {
                    // Case 3: Three items with fixed spacing
                    availableSpace = finalSize.Height - totalChildrenHeight;
                    double gap = spacing;
                    currentY = 0;

                    for (int i = 0; i < childCount; i++)
                    {
                        var child = children[i];
                        var childSize = child.DesiredSize;
                        child.Arrange(new Rect((finalSize.Width - childSize.Width) / 2, currentY, childSize.Width, childSize.Height));
                        currentY += childSize.Height + gap;
                    }
                }
                else // More than 3 items
                {
                    // Case 4: More than three items, evenly distributed with fixed spacing
                    availableSpace = finalSize.Height - totalChildrenHeight - (spacing * (childCount - 1));
                    double gap = spacing + availableSpace / (childCount - 1);
                    currentY = 0;

                    for (int i = 0; i < childCount; i++)
                    {
                        var child = children[i];
                        var childSize = child.DesiredSize;
                        child.Arrange(new Rect((finalSize.Width - childSize.Width) / 2, currentY, childSize.Width, childSize.Height));
                        currentY += childSize.Height + gap;
                    }
                }
            }

            return finalSize;
        }
    }
}
