using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using Application.OtherServices;
using Application.Record;
using ScreenRecorderLib;

namespace Application.UI
{
  public partial class Main : Window
  {
    private readonly RecordService _recordService;
    private readonly FileService _fileService;
    private readonly DispatcherTimer _timer;
    private readonly TimeSpan _timerInterval = TimeSpan.FromSeconds(1);
    private bool _isPaused;
    private TimeSpan _timeElapsed;
    
    private TimeSpan TimeElapsed
    {
      get => _timeElapsed;
      set
      {
        _timeElapsed = value;
        TimerLbl.Content = _timeElapsed.ToString("hh':'mm':'ss");
      }
    }
    
    public Main()
    {
      InitializeComponent();
      _recordService = new RecordService(App.ConfigService);
      _fileService = new FileService(App.ConfigService);
      _timer = new DispatcherTimer();
      _timer.Tick += TimerOnTick;
      _timer.Interval = _timerInterval;

      StartStopBtn.Click += StartStopBtnOnClick;
      PauseResumeBtn.Click += PauseResumeBtnOnClick;
      _recordService.Completed += RecordServiceOnCompleted;
      _recordService.Failed += RecordServiceOnFailed;
      _recordService.StatusChanged += RecordServiceOnStatusChanged;

      StartStopBtn.Content = LocalizationService.StartRecording;
      PauseResumeBtn.Content = LocalizationService.Pause;
      StatusLbl.Content = LocalizationService.NotRecording;
    }

    private void TimerOnTick(object? sender, EventArgs e) => TimeElapsed += _timerInterval;

    private void RecordServiceOnCompleted(string path)
    {
      Dispatcher.Invoke(() =>
      {
        _recordService.ReleaseRecorder();
        var fullPath = Path.GetFullPath(path);
        Process.Start("explorer.exe", $"/select,\"{fullPath}\"");
      });
    }

    private void RecordServiceOnFailed(string error)
    {
      Dispatcher.Invoke(() =>
      {
        _recordService.ReleaseRecorder();
        MessageBox.Show(this, error, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      });
    }

    private void RecordServiceOnStatusChanged(RecorderStatus status)
    {
      Dispatcher.Invoke(() =>
      {
        switch (status)
        {
          case RecorderStatus.Idle:
            StartStopBtn.Content = LocalizationService.StartRecording;
            StatusLbl.Content = LocalizationService.NotRecording;
            _timeElapsed = TimeSpan.Zero;
            break;
          case RecorderStatus.Recording:
            StartStopBtn.Content = LocalizationService.StopRecording;
            StatusLbl.Content = LocalizationService.Recording;
            _timer.Start();
            break;
          case RecorderStatus.Paused:
            PauseResumeBtn.Content = LocalizationService.Resume;
            StatusLbl.Content = LocalizationService.Paused;
            _timer.Stop();
            break;
          case RecorderStatus.Finishing:
            StatusLbl.Content = LocalizationService.Saving;
            _timer.Stop();
            break;
          default:
            throw new ArgumentOutOfRangeException(nameof(status), status, null);
        }
      });
    }

    private void PauseResumeBtnOnClick(object sender, RoutedEventArgs e)
    {
      _isPaused = !_isPaused;

      if (_isPaused)
        _recordService.Pause();
      else
        _recordService.Resume();
    }

    private async void StartStopBtnOnClick(object sender, RoutedEventArgs e)
    {
      if (_recordService.IsRecording)
      {
        _recordService.StopRecord();
        _timer.Stop();
      }
      else
      {
        _fileService.CreateOutputFolder();
        await App.ConfigService.ReadSettingsAsync();
        _recordService.CreateRecorder(new RecordProps
        {
          AudioInputDevice = string.Empty,
          AudioOutputDevice = string.Empty, 
          Sides = new ScreenSides
          {
            Top = 0,
            Bottom = (int)SystemParameters.PrimaryScreenHeight,
            Left = 0,
            Right = (int)SystemParameters.PrimaryScreenWidth
          }
        })
          .StartRecord(_fileService.VideoFileName);
      }
    }
  }
}