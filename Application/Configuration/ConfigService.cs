using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Application.Configuration
{
  public class ConfigService
  {
    private const string ConfigFileName = "Config.cfg";

    public Config Config { get; private set; } = null!;

    public async Task ReadSettingsAsync()
    {
      if (!File.Exists(ConfigFileName))
        CreateDefaultConfig();
      
      var text = await File.ReadAllTextAsync(ConfigFileName);
      Config = JsonConvert.DeserializeObject<Config>(text);
    }

    private void CreateDefaultConfig()
    {
      Config = new Config
      {
        OutputFolderName = "Video",
        EnableMicrophone = false,
        EnableSounds = true
      };

      var json = JsonConvert.SerializeObject(Config);
      File.WriteAllTextAsync(ConfigFileName, json);
    }
  }
}