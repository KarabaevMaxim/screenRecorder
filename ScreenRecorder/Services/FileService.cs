using System.IO;

namespace ScreenRecorder.Services
{
  public class FileService : IFileService
  {
    private readonly Config _config;

    public void CreateTempFolder()
    {
      if (!Directory.Exists(_config.TempFolderFullName))
        Directory.CreateDirectory(_config.TempFolderFullName);
    }

    public void CleanTempFolder()
    {
      if (Directory.Exists(_config.TempFolderFullName))
        ClearFolder(_config.TempFolderFullName);

    }
    
    public void ClearFolder(string folder) => Directory.Delete(folder, true);

    public void DeleteFiles(string folder, string exceptFile)
    {
      var files = Directory.GetFiles(folder);

      foreach (var file in files)
      {
        if (file != exceptFile)
        {
          File.Delete(file);
        }
      }
    }

    public FileService(Config config) => _config = config;
  }

  public interface IFileService
  {
    void CreateTempFolder();

    void CleanTempFolder();
    
    void ClearFolder(string folder);

    void DeleteFiles(string folder, string exceptFile);
  }
}