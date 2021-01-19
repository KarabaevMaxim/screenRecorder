namespace Application.Configuration
{
  public record Config
  {
    public string OutputFolderName { get; init; }
    
    public bool EnableMicrophone { get; init; }
    
    public bool EnableSounds { get; init; }
    
    public bool IsHardwareEncodingEnabled { get; init; }
    
    /// <summary>
    /// Low latency mode provides faster encoding, but can reduce quality.
    /// </summary>
    public bool IsLowLatencyEnabled { get; init; }
    
    /// <summary>
    /// Fast start writes the mp4 header at the beginning of the file, to facilitate streaming.
    /// </summary>
    public bool IsMp4FastStartEnabled { get; init; }
    
    public int Framerate { get; init; }
    
    /// <summary>
    /// May contains values 0-100.
    /// </summary>
    public int Quality { get; init; }
    
    public bool IsMousePointerEnabled { get; init; }
    
    public bool IsMouseClicksDetected { get; init; }
  }
}