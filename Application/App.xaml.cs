using System.Windows;
using Application.Configuration;

namespace Application
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : System.Windows.Application
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