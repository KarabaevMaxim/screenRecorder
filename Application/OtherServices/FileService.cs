using System;
using System.IO;
using Application.Configuration;

namespace Application.OtherServices
{
  public class FileService
  {
    private readonly ConfigService _configService;

    public string VideoFileName =>
      Path.Combine(_configService.Config.OutputFolderName, $"{DateTime.Now:dd-MM-yyyy_hh-mm-ss}.mp4");

    public void CreateOutputFolder()
    {
      if (!Directory.Exists(_configService.Config.OutputFolderName))
        Directory.CreateDirectory(_configService.Config.OutputFolderName);
    }
    
    public FileService(ConfigService configService) => _configService = configService;
  }
}