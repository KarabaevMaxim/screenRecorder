using System.Windows;
using System.Windows.Controls;
using Appl.Record;

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
    }

    private void RecordServiceOnCompleted(string path)
    {
      Dispatcher.Invoke(() =>
      {
        _recordService.ReleaseRecorder();
        MessageBox.Show(this, $"Видео сохранено: {path}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
      });
    }

    private void RecordServiceOnFailed(string error)
    {
      Dispatcher.Invoke(() =>
      {
        _recordService.ReleaseRecorder();
        MessageBox.Show(this, error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
        ((Button) sender).Content = "Начать запись";
        _recordService.StopRecord();
        _recordService.ReleaseRecorder();
      }
      else
      {
        ((Button) sender).Content = "Завершить запись";
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