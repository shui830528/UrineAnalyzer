using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UrineAnalyzer
{
  /// <summary>
  /// SampleImageWindow.xaml 的交互逻辑
  /// </summary>
  public partial class SampleImageWindow : ModernWindow
  {
    private int sampleID;
    public int SampleID { get { return sampleID; } set { sampleID = value; } }
    private string imageFile = string.Empty;
    public string ImageFile { get { return imageFile; } set { imageFile = value; } }

    private double imageWidth;
    private double imageHeight;
    public SampleImageWindow()
    {
      InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      textBlock_SampleID.Text = sampleID.ToString();
      BitmapImage imagesouce = new BitmapImage();
      imagesouce = new BitmapImage(new Uri(imageFile));
      imageWidth = imagesouce.Width;
      imageHeight = imagesouce.Height;
      image.Width = imageWidth;
      image.Height = imageHeight;
      image.Source = imagesouce.Clone();
      ImageZoom();
    }

    private void button_Close_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }
    private void ImageZoom()
    {
      if (checkBox.IsChecked == true)
      {
        image.Width = 700;
        image.Height = imageHeight;
      }
      else
      {
        image.Width = imageWidth;
        image.Height = imageHeight;
      }
    }
    private void checkBox_Click(object sender, RoutedEventArgs e)
    {
      ImageZoom();
    }
  }
}
