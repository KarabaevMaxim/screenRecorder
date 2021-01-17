using System;
using System.Threading.Tasks;
using ScreenRecorderLib;

namespace DidiTaxi.Application.Services
{
  public class RecordService : IDisposable
  {
    public event Action<string>? Completed;
    public event Action<string>? Failed;
    public event Action? StatusChanged;
    
    private readonly Recorder _recorder;

    public RecordService()
    {
      _recorder = Recorder.CreateRecorder();
      _recorder.OnRecordingComplete += OnRecordingComplete;
      _recorder.OnRecordingFailed += OnRecordingFailed;
      _recorder.OnStatusChanged += OnStatusChanged;
    }
    
    public Task CreateRecording() =>
      Task.Run(() =>
      {
        _recorder.Record("test.mp4");
      });

    public void EndRecording() => _recorder.Stop();

    private void OnRecordingComplete(object sender, RecordingCompleteEventArgs e) => Completed?.Invoke(e.FilePath);

    private void OnRecordingFailed(object sender, RecordingFailedEventArgs e) => Failed?.Invoke(e.Error);

    private void OnStatusChanged(object sender, RecordingStatusEventArgs e) => StatusChanged?.Invoke();

    public void Dispose() => _recorder.Dispose();
  }
}
