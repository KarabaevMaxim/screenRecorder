using System.Windows;
using Appl.Configuration;

namespace Appl
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    public static ConfigService ConfigService { get; private set; } = null!;

    protected override async void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
      ConfigService = new ConfigService();
      await ConfigService.ReadSettingsAsync();
    }
  }
}