using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Windows;
using AForge.Controls;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Controls;
using System.Runtime.InteropServices;

namespace Control
{
  public class Snapshot
  {
    private FilterInfoCollection videoDevices;
    private VideoCaptureDevice videoDevice;
    private VideoCapabilities[] videoCapabilities;
    private VideoSourcePlayer videoSourcePlayer = null;
    private string imageFilePath;
    private bool isExisit = true;
    private List<System.Drawing.Size> videoResolutions = new List<System.Drawing.Size>();
    public List<System.Drawing.Size> VideoResolutions { get { return videoResolutions; } }
    public VideoSourcePlayer VideoSourcePlayer { get { return videoSourcePlayer; } set { videoSourcePlayer = value; } }
    private System.Drawing.Size videoSize = System.Drawing.Size.Empty;
    public System.Drawing.Size VideoSize { get { return videoSize; } set { videoSize = value; } }
    private static Label videoState = null;
    public static Label VideoState { get { return videoState; } set { videoState = value; } }

    [DllImport("TestStrip.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ClippingPictures(StringBuilder imageFile);
    public Snapshot()
    {
      imageFilePath = System.AppDomain.CurrentDomain.BaseDirectory + "image";

      if (!Directory.Exists(imageFilePath))
      {
        try
        {
          Directory.CreateDirectory(imageFilePath);
        }
        catch (System.Exception e)
        {
          throw new System.Exception(e + "创建目录失败！");
        }
      }

      GetVideoDevices();
    }
    public List<string> GetVideoDevices()
    {
      List<string> videoDeviceNames = new List<string>();
      videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
      if (videoDevices.Count != 0)
      {
        foreach (FilterInfo device in videoDevices)
        {
          videoDeviceNames.Add(device.Name);
        }
      }
      else
      {
        isExisit = false;
        videoState.Content = "没有发现摄像机！";
      }
      return videoDeviceNames;
    }

    public void CreateVideoCaptureDevice(int deviceIndex)
    {
      if (videoDevices.Count != 0)
      {
        videoDevice = new VideoCaptureDevice(videoDevices[deviceIndex].MonikerString);
        EnumeratedSupportedFrameSizes(videoDevice, ref videoResolutions);
      }
    }

    private void EnumeratedSupportedFrameSizes(VideoCaptureDevice videoDevice, ref List<System.Drawing.Size> videoResolutions)
    {
      videoCapabilities = videoDevice.VideoCapabilities;
      foreach (VideoCapabilities capabilty in videoCapabilities)
      {
        if (videoResolutions.IndexOf(capabilty.FrameSize) < 0)
        {
          videoResolutions.Add(capabilty.FrameSize);
        }
      }
      if (videoCapabilities.Length == 0)
      {
        videoState.Content = "摄像机不支持！";
      }
    }

    public void Connect(int deviceIndex)
    {
      if(!isExisit)
      {
        return;
      }
      CreateVideoCaptureDevice(deviceIndex);
      if (videoDevice != null)
      {
        if ((videoCapabilities != null) && (videoCapabilities.Length != 0))
        {
          if (videoSize == System.Drawing.Size.Empty)
          {
            videoSize = videoResolutions[videoResolutions.Count - 1];
          }
          videoDevice.DesiredFrameSize = videoSize;
        }
        if (videoSourcePlayer == null)
        {
          videoSourcePlayer = new VideoSourcePlayer();
        }
        videoSourcePlayer.VideoSource = videoDevice;
        videoSourcePlayer.Start();
        if (videoDevice.IsRunning && videoSourcePlayer.IsRunning)
        {
          videoState.Content = "摄像机已连接！";
        }
      }
    }
    public string CaptureImage()
    {
      string strFileName = string.Empty;
      if (isExisit)
      {
        Bitmap bitmap = videoSourcePlayer.GetCurrentVideoFrame();
        if (bitmap != null)
        {
          strFileName = imageFilePath + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
          bitmap.Save(strFileName, ImageFormat.Jpeg);
          bitmap.Dispose();
          bitmap = null;
          StringBuilder imageFile = new StringBuilder(strFileName);
          ClippingPictures(imageFile);
        }
      }
      return strFileName;
    }
    public void Disconnect()
    {
      if (videoSourcePlayer != null && videoSourcePlayer.VideoSource != null)
      {
        videoSourcePlayer.SignalToStop();
        videoSourcePlayer.WaitForStop();
        videoSourcePlayer.VideoSource = null;
      }
    }
  }
}
