using System;
using ScreenRecorderLib;

namespace Appl.Record
{
  public class RecordService
  {
    public event Action<string>? Completed;
    public event Action<string>? Failed;
    public event Action<RecorderStatus>? StatusChanged;
    public event Action? SnapshotSaved;
    
    private Recorder? _recorder;
    
    public bool IsRecording { get; private set; }
    
    public void CreateRecorder(RecordProps props)
    {
      if (_recorder != null)
        return;

      var options = new RecorderOptions
      {
        AudioOptions = new AudioOptions
        {
          IsAudioEnabled = true,
          IsOutputDeviceEnabled = true,
          IsInputDeviceEnabled = true,
          AudioOutputDevice = props.AudioOutputDevice,
          AudioInputDevice = props.AudioInputDevice
        }
      };
      _recorder = Recorder.CreateRecorder(options);
      _recorder = Recorder.CreateRecorder();
      _recorder.OnRecordingComplete += (sender, args) => Completed?.Invoke(args.FilePath);
      _recorder.OnRecordingFailed += (sender, args) => Failed?.Invoke(args.Error);
      _recorder.OnStatusChanged += (sender, args) => StatusChanged?.Invoke(args.Status);
      _recorder.OnSnapshotSaved += (sender, args) => SnapshotSaved?.Invoke();
    }

    public void ReleaseRecorder()
    {
      StopRecord();
      _recorder?.Dispose();
      _recorder = null;
    }

    public void StartRecord(string outputFileName)
    {
      if (_recorder == null || IsRecording)
        return;
      
      _recorder.Record(outputFileName);
      IsRecording = true;
    }

    public void Pause() => _recorder?.Pause();

    public void Resume() => _recorder?.Resume();

    public void StopRecord()
    {
      if (!IsRecording)
        return;

      _recorder?.Stop();
    }
  }
}