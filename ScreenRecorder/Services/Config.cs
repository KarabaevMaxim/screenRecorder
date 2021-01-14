namespace ScreenRecorder.Services
{
  public class Config
  {
    public const string AudioName = "mic.wav";
    public const string VideoName = "video.mp4";
    public const string FinalName = "finalVideo.mp4";
    
    public string TempFolderFullName { get; set; }
    
    public string OutputFOlderFullName { get; set; }
  }
}