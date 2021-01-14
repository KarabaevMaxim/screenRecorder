using System.Drawing;

namespace ScreenRecorder
{
  public interface IScreenRecordService
  {
    void StartRecord(Rectangle bounds);
  }
}