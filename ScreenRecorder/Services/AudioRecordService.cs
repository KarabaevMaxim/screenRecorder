namespace ScreenRecorder.Services
{
  public class AudioRecordService : IAudioRecordService
  {
    public void RecordAudio()
    {
      NativeMethods.MciSendString("open new Type waveaudio Alias recsound", "", 0, 0);
      NativeMethods.MciSendString("record recsound", "", 0, 0);
    }
  }

  public interface IAudioRecordService
  {
  }
}