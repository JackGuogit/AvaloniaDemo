using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Styling;
using ImTools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
namespace AvaloniaPrsimSimple.Controls
{
    public class StackPanelEx : StackPanel
    {


        protected override Size ArrangeOverride(Size finalSize)
        {
            Avalonia.Controls.Controls children = Children;
            int childCount = children.Count;
            Debug.WriteLine(childCount);


            if (childCount > 3)
            {
                Debug.WriteLine(finalSize.ToString());

                return base.ArrangeOverride(finalSize);
            }

            if (childCount == 0)
            {
                return finalSize;
            }

            else if (childCount == 1)
            {
                // Case 1: One item, centered

                Control child = children[0];
                double minHeight = child.MinHeight;

                var childSize = child.DesiredSize;
                double x = (finalSize.Width - childSize.Width) / 2;
                double y = (finalSize.Height - childSize.Height) / 2;

                Rect rect = new Rect(x, y, childSize.Width, childSize.Height);
                child.Arrange(rect);
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


                secondChild.Arrange(new Rect((finalSize.Width - secondChildSize.Width) / 2, y2, secondChildSize.Width, secondChildSize.Height));


                if (firstChild.RenderTransform is not TranslateTransform transform)
                {
                    transform = new TranslateTransform();
                    firstChild.RenderTransform = transform;
                }

                var animation = new Animation
                {
                    Duration = TimeSpan.FromSeconds(0.1),
                    Children =
                    {
                        new KeyFrame
                        {
                            Setters =
                            {

                                new Setter(TranslateTransform.YProperty, firstChild.Bounds.Y)
                            },
                            Cue = new Cue(0)
                        },

                        new KeyFrame
                        {
                            Setters =
                            {
                                new Setter(TranslateTransform.YProperty, y1)
                            },
                            Cue = new Cue(1)
                        }
                    }
                };
                firstChild.Arrange(new Rect((finalSize.Width - firstChildSize.Width) / 2, y1, firstChildSize.Width, firstChildSize.Height));
                animation.RunAsync(firstChild);


            }
            else if (childCount == 3)
            {
                double totalChildrenHeight = 0;
                foreach (var child in children)
                {
                    totalChildrenHeight += child.DesiredSize.Height;
                }

                double availableSpace;
                double currentY;
                // Case 3: Three items with fixed spacing
                availableSpace = finalSize.Height - totalChildrenHeight;

                double gap = availableSpace / (childCount - 1);

                currentY = 0;







                for (int i = 1; i < childCount-1; i++)
                {
                    var child = children[i];
                    var childSize = child.DesiredSize;

                    currentY += childSize.Height+ gap;



                    if (child.RenderTransform is not TranslateTransform transform)
                    {
                        transform = new TranslateTransform();
                        child.RenderTransform = transform;
                    }

                    var animation = new Animation
                    {
                        Duration = TimeSpan.FromSeconds(0.8),
                        Children =
                        {
                            new KeyFrame
                            {
                                Setters =
                                {

                                                                       new Setter(TranslateTransform.YProperty, currentY)
                                },
                                Cue = new Cue(0)
                            },

                            new KeyFrame
                            {
                                Setters =
                                {
 
                                                                        new Setter(TranslateTransform.YProperty, currentY-gap-childSize.Height)
                                },
                                Cue = new Cue(1)
                            }
                        }
                    };
                    child.Arrange(new Rect((finalSize.Width - childSize.Width) / 2, currentY, childSize.Width, childSize.Height));
                    animation.RunAsync(child);
                }

                var childLast = children.Last();
                var childLastSize = childLast.DesiredSize;
                currentY += childLastSize.Height + gap;
                childLast.Arrange(new Rect((finalSize.Width - childLastSize.Width) / 2, currentY, childLastSize.Width, childLastSize.Height));

            }

            else
            {


                return base.ArrangeOverride(finalSize);
            }

            return finalSize;
        }





    }
}
