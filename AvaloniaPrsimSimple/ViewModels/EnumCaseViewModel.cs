using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Collections;
using Avalonia.Data.Converters;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaPrsimSimple.ViewModels;

public partial class EnumCaseViewModel:ViewModelBase
{
    private LightStatus _light1Status= LightStatus.Blue;
    private LightStatus _light2Status= LightStatus.Green; 
    private LightStatus _light3Status= LightStatus.Red;


    
    public LightStatus Light1Status
    {
        get => _light1Status;
        set
        {
            _light1Status = value;
            OnPropertyChanged(nameof(Light1Status));
        }
    }

    public LightStatus Light2Status
    {
        get => _light2Status;
        set
        {
            _light2Status = value;
            OnPropertyChanged(nameof(Light2Status));
        }
    }

    public LightStatus Light3Status
    {
        get => _light3Status;
        set
        {
            _light3Status = value;
            OnPropertyChanged(nameof(Light3Status));
        }
    }

    private bool _isRunning = false; // 标志位，表示操作是否正在运行
    [ObservableProperty]
    private string _buttonText = "Start"; // 按钮文本
    
    private CancellationTokenSource? _cancellationTokenSource;
    [RelayCommand]
    private async Task ChangeLightStatusAsync()
    {
        Debug.WriteLine("ChangeLightStatusAsync 被调用");
        Debug.WriteLine($"当前线程 ID: {Environment.CurrentManagedThreadId}");

        if (_isRunning)
        {
            // 如果正在运行，则停止操作
            _isRunning = false;
            _cancellationTokenSource?.Cancel(); // 取消操作

            Light2Status = LightStatus.Green; // 设置中间灯为绿色
            Debug.WriteLine("操作已停止");
            return;
        }

        // 开始操作
        _isRunning = true;
   
        Debug.WriteLine("操作已开始");

        _cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = _cancellationTokenSource.Token;

        try
        {
            while (_isRunning)
            {
                Light1Status = LightStatus.Red;
                Debug.WriteLine("Light1 点亮");
                await Task.Delay(1000, cancellationToken); // 异步等待 1 秒
                if (cancellationToken.IsCancellationRequested) break;
                Light1Status = LightStatus.Off;
                Debug.WriteLine("Light1 关闭");

                Light2Status = LightStatus.Blue;
                Debug.WriteLine("Light2 点亮");
                await Task.Delay(1000, cancellationToken); // 异步等待 1 秒
                if (cancellationToken.IsCancellationRequested) break;
                Light2Status = LightStatus.Off;
                Debug.WriteLine("Light2 关闭");

                Light3Status = LightStatus.Red;
                Debug.WriteLine("Light3 点亮");
                await Task.Delay(1000, cancellationToken); // 异步等待 1 秒
                if (cancellationToken.IsCancellationRequested) break;
                Light3Status = LightStatus.Off;
                Debug.WriteLine("Light3 关闭");
            }
        }
        catch (TaskCanceledException)
        {
            Debug.WriteLine("操作已取消");
        }
        finally
        {
            if (!_isRunning)
            {
                Light2Status = LightStatus.Green;
                Debug.WriteLine("中间灯设置为绿色");
            }
        }
        // Light1Status = LightStatus.Green;
        // await Task.Delay(1000); // 异步等待 1 秒
        // Light1Status = LightStatus.Off;
        //
        // Light2Status = LightStatus.Green;
        // await Task.Delay(1000); // 异步等待 1 秒
        // Light2Status = LightStatus.Off;
        //
        // Light3Status = LightStatus.Green;
        // await Task.Delay(1000); // 异步等待 1 秒
        // Light3Status = LightStatus.Off;
    }

    [RelayCommand]
    private async Task CancelOperationAsync()
    {
        Debug.WriteLine("CancelOperationAsync 被调用");
        Debug.WriteLine($"当前线程 ID: {Environment.CurrentManagedThreadId}");

        // 取消操作
        _isRunning = false;
        _cancellationTokenSource?.Cancel(); // 取消操作

        Light1Status= LightStatus.Off;
        Light3Status= LightStatus.Off;
        Light2Status = LightStatus.Green; // 设置中间灯为绿色
        Debug.WriteLine("操作已停止");
    }
    
}
public enum LightStatus
{
    Off,
    Red,
    Green,
    Blue
}
public class LightStatusToBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is LightStatus status)
        {
            switch (status)
            {
                case LightStatus.Off:
                    return Brushes.Gray;
                case LightStatus.Red:
                    return Brushes.Red;
                case LightStatus.Green:
                    return Brushes.Green;
                case LightStatus.Blue:
                    return Brushes.Blue;
            }
        }
        return Brushes.Gray;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}