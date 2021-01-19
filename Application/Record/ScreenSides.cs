namespace Application.Record
{
  public record ScreenSides
  {
    public int Top { get; init; }
    
    public int Bottom { get; init; }
    
    public int Left { get; init; }
    
    public int Right { get; init; }
  }
}