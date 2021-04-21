using System;
using Application.Configuration;
using ScreenRecorderLib;

namespace Application.Record
{
  public class RecordService
  {
    private readonly ConfigService _configService;
    public event Action<string>? Completed;
    public event Action<string>? Failed;
    public event Action<RecorderStatus>? StatusChanged;
    public event Action? SnapshotSaved;
    
    private Recorder? _recorder;
    
    public bool IsRecording { get; private set; }
    
    public RecordService CreateRecorder(RecordProps props)
    {
      if (_recorder != null)
        return this;
      
      var options = new RecorderOptions
      {
        RecorderApi = RecorderApi.DesktopDuplication,
        IsHardwareEncodingEnabled = _configService.Config.IsHardwareEncodingEnabled,
        IsLowLatencyEnabled = _configService.Config.IsLowLatencyEnabled,
        IsMp4FastStartEnabled = _configService.Config.IsMp4FastStartEnabled,
        DisplayOptions = new DisplayOptions
        {
          Top = props.Sides.Top,
          Bottom = props.Sides.Bottom,
          Left = props.Sides.Left,
          Right = props.Sides.Right,
          //WindowHandle = props.WndHandle // RecorderApi = RecorderApi.WindowsGraphicsCapture
        },
        AudioOptions = new AudioOptions
        {
          IsAudioEnabled = _configService.Config.EnableSounds || _configService.Config.EnableMicrophone,
          IsOutputDeviceEnabled = _configService.Config.EnableSounds,
          IsInputDeviceEnabled = _configService.Config.EnableMicrophone,
          AudioOutputDevice = props.AudioOutputDevice,
          AudioInputDevice = props.AudioInputDevice,
          Bitrate = AudioBitrate.bitrate_128kbps,
          Channels = AudioChannels.Stereo
        },
        VideoOptions = new VideoOptions
        {
          BitrateMode = BitrateControlMode.Quality,
          Quality = _configService.Config.Quality,
          Framerate = _configService.Config.Framerate,
          IsFixedFramerate = false,
          EncoderProfile = H264Profile.Main
        },
        MouseOptions = new MouseOptions
        {
          IsMousePointerEnabled = _configService.Config.IsMousePointerEnabled,
          IsMouseClicksDetected = _configService.Config.IsMouseClicksDetected,
          MouseClickDetectionColor = "#FFFF00",
          MouseRightClickDetectionColor = "#FFFF00",
          MouseClickDetectionMode = MouseDetectionMode.Hook
        }
      };
      _recorder = Recorder.CreateRecorder(options);
      _recorder.OnRecordingComplete += (sender, args) => Completed?.Invoke(args.FilePath);
      _recorder.OnRecordingFailed += (sender, args) => Failed?.Invoke(args.Error);
      _recorder.OnStatusChanged += (sender, args) => StatusChanged?.Invoke(args.Status);
      _recorder.OnSnapshotSaved += (sender, args) => SnapshotSaved?.Invoke();
      return this;
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

    public RecordService(ConfigService configService)
    {
      _configService = configService;
    }
  }
}