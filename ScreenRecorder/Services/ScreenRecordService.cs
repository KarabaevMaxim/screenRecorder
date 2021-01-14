using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ScreenRecorder.Services
{
  public class ScreenRecordService : IScreenRecordService
  {
    private readonly Config _config;
    private readonly IFileService _fileService;
    private readonly List<string> _inputImageSequence = new();
    private readonly Stopwatch _stopwatch = new();

    public void StartRecord(Rectangle bounds)
    {
      _fileService.CreateTempFolder();
      
      _stopwatch.Start();

      using Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);
      using (Graphics g = Graphics.FromImage(bitmap))
      {
        //Add screen to bitmap:
        g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
      }

      var screenShotName = $"screenshot-{_inputImageSequence.Count}.png";
      bitmap.Save(Path.Combine(_config.TempFolderFullName, screenShotName), ImageFormat.Png);
      _inputImageSequence.Add(screenShotName);
    }
    

    
    private void SaveVideo(int width, int height, int frameRate)
    {
      
      
      using (VideoFileWriter vFWriter = new VideoFileWriter())
      {
        //Create new video file:
        vFWriter.Open(outputPath + "//" + videoName, width, height, frameRate, VideoCodec.MPEG4);
 
        //Make each screenshot into a video frame:
        foreach (string imageLocation in inputImageSequence)
        {
          Bitmap imageFrame = System.Drawing.Image.FromFile(imageLocation) as Bitmap;
          vFWriter.WriteVideoFrame(imageFrame);
          imageFrame.Dispose();
        }
 
        //Close:
        vFWriter.Close();
      }
    }
    
    private void SaveAudio()
    {
      string audioPath = "save recsound " + outputPath + "//" + audioName;
      NativeMethods.record(audioPath, "", 0, 0);
      NativeMethods.record("close recsound", "", 0, 0);
    }

    public void CleanUp() => _fileService.CleanTempFolder();
    
    public string GetElapsed() => 
      $"{_stopwatch.Elapsed.Hours:D2}:{_stopwatch.Elapsed.Minutes:D2}:{_stopwatch.Elapsed.Seconds:D2}";

    public ScreenRecordService(Config config, IFileService fileService)
    {
      _config = config;
      _fileService = fileService;
    }
  }
}