using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.Converters;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Styling;
using AvaloniaPrsimSimple.Helpers;
using System;
using System.Threading;

namespace AvaloniaPrsimSimple.Views;

public partial class AnimationDemoView : UserControl
{

    CancellationTokenSource cts ;

    Animation anim = new Animation() { Duration = TimeSpan.FromSeconds(2), IterationCount = IterationCount.Infinite };


    public AnimationDemoView()
    {
        InitializeComponent();
        this.Loaded += AnimationDemoView_Loaded;
    }

    private void AnimationDemoView_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        //var colorAnimation = new Animation() { Duration = TimeSpan.FromSeconds(3),IterationCount=IterationCount.Infinite };

        //var startKeyFrame = new KeyFrame() { Cue = new Cue(0) };
        //startKeyFrame.Setters.Add(new Setter(Border.BackgroundProperty, Brushes.Red));

        //var midKeyFrame = new KeyFrame() { Cue = new Cue(0.5) };
        //midKeyFrame.Setters.Add(new Setter(Border.BackgroundProperty, Brushes.Green));

        //var endKeyFrame = new KeyFrame() { Cue = new Cue(1) };
        //endKeyFrame.Setters.Add(new Setter(Border.BackgroundProperty, Brushes.Blue));

        //colorAnimation.Children.Add(startKeyFrame);
        //colorAnimation.Children.Add(midKeyFrame);
        //colorAnimation.Children.Add(endKeyFrame);

        //colorAnimation.RunAsync(PART_Border);



        
        var gradientBrush = new LinearGradientBrush()
        {
            StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
            EndPoint = new RelativePoint(1, 1, RelativeUnit.Relative)
        };

        //var color0 = ColorToHexConverter.ParseHexString("#5ddcff", AlphaComponentPosition.Leading)!.Value;
        //var color1 = ColorToHexConverter.ParseHexString("#3c67e3", AlphaComponentPosition.Leading)!.Value;
        //var color2 = ColorToHexConverter.ParseHexString("#4e00c2", AlphaComponentPosition.Leading)!.Value;

        //var gradientStop0 = new GradientStop(color0, 0);
        //var gradientStop1 = new GradientStop(color1, 0.43);
        //var gradientStop2 = new GradientStop(color2, 1);

        //gradientBrush.GradientStops.AddRange([gradientStop0, gradientStop1, gradientStop2]);

        //PART_Border.BorderBrush = gradientBrush;




        var keyframe0 = new KeyFrame() { Cue = new Cue(0) };
        keyframe0.Setters.Add(new Setter(LinearGradientBrushHelper.RotateAngleProperty, 0d));
        var keyframe1 = new KeyFrame() { Cue = new Cue(1) };
        keyframe1.Setters.Add(new Setter(LinearGradientBrushHelper.RotateAngleProperty, Math.PI * 2));

        anim.Children.Add(keyframe0);
        anim.Children.Add(keyframe1);






    }

    private void StopAnimation(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        cts.Cancel();
    }

    private void StartAnimation(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        cts = new CancellationTokenSource();


        anim.RunAsync(PART_Border, cts.Token);

    }
}