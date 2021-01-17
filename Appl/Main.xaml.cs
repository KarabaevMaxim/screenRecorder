using System;
using System.Windows;
using System.Windows.Controls;
using Appl.OtherServices;
using Appl.Record;
using ScreenRecorderLib;

namespace Appl
{
  public partial class Main : Window
  {
    private readonly RecordService _recordService;
    private readonly FileService _fileService;
    private bool _isPaused;
    
    public Main()
    {
      InitializeComponent();
      _recordService = new RecordService();
      _fileService = new FileService(App.ConfigService);

      StartStopBtn.Click += StartStopBtnOnClick;
      PauseResumeBtn.Click += PauseResumeBtnOnClick;
      _recordService.Completed += RecordServiceOnCompleted;
      _recordService.Failed += RecordServiceOnFailed;
      _recordService.StatusChanged += RecordServiceOnStatusChanged;

      StartStopBtn.Content = LocalizationService.StartRecording;
      PauseResumeBtn.Content = LocalizationService.Pause;
      StatusLbl.Content = LocalizationService.NotRecording;
    }

    private void RecordServiceOnCompleted(string path)
    {
      Dispatcher.Invoke(() =>
      {
        _recordService.ReleaseRecorder();
        MessageBox.Show(this, $"Video saved: {path}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
            break;
          case RecorderStatus.Recording:
            StartStopBtn.Content = LocalizationService.StopRecording;
            StatusLbl.Content = LocalizationService.Recording;
            break;
          case RecorderStatus.Paused:
            PauseResumeBtn.Content = LocalizationService.Resume;
            StatusLbl.Content = LocalizationService.Paused;
            break;
          case RecorderStatus.Finishing:
            StatusLbl.Content = LocalizationService.Saving;
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
      {
        ((Button) sender).Content = "Продолжить";
        _recordService.Pause();
      }
      else
      {
        ((Button) sender).Content = "Приостановить";
        _recordService.Resume();
      }
    }

    private void StartStopBtnOnClick(object sender, RoutedEventArgs e)
    {
      if (_recordService.IsRecording)
      {
        _recordService.StopRecord();
        // _recordService.ReleaseRecorder();
      }
      else
      {
        _recordService.CreateRecorder(new RecordProps
        {
          IsAudioEnabled = App.ConfigService.Config.EnableMicrophone && App.ConfigService.Config.EnableSounds,
          IsInputDeviceEnabled = App.ConfigService.Config.EnableMicrophone,
          IsOutputDeviceEnabled = App.ConfigService.Config.EnableSounds,
          AudioInputDevice = "",
          AudioOutputDevice = ""
        });
        
        _fileService.CreateOutputFolder();
        _recordService.StartRecord(_fileService.VideoFileName);
      }
    }
  }
}