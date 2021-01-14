using System.Runtime.InteropServices;

namespace ScreenRecorder
{
  public static class NativeMethods
  {
    [DllImport("winmn.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi, SetLastError = true,
      ExactSpelling = true)]
    public static extern int MciSendString(string lpstrCommand, string lpstrReturnString, int uReturnLength,
      int hwndCallback);
  }
}