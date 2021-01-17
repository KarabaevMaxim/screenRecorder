namespace Appl.Record
{
  public record RecordProps
  {
    public bool IsAudioEnabled { get; init; }
    
    public bool IsOutputDeviceEnabled { get; init; }
    
    public bool IsInputDeviceEnabled { get; init; }
    
    public string AudioOutputDevice { get; init; }
    
    public string AudioInputDevice { get; init; }
  }
}