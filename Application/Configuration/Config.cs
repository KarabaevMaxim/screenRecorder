namespace Application.Configuration
{
  public record Config
  {
    public string OutputFolderName { get; init; }
    
    public bool EnableMicrophone { get; init; }
    
    public bool EnableSounds { get; init; }
  }
}