using System;

namespace Application.Record
{
  public record RecordProps
  {
    public string AudioOutputDevice { get; init; }
    
    public string AudioInputDevice { get; init; }
    
    public ScreenSides Sides { get; init; }
    
    public IntPtr WndHandle { get; init; }
  }
}