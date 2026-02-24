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
  /// MessageBoxEx.xaml 的交互逻辑
  /// </summary>
  public partial class MessageBoxEx : ModernWindow
  {
    private MessageBoxResult messageBoxResult;
    public MessageBoxResult Result { get { return messageBoxResult; } set { messageBoxResult = value; } }
    private MessageBoxImage messageBoxImage;
    public MessageBoxImage Image { get { return messageBoxImage; } set { messageBoxImage = value; } }
    private MessageBoxButton messageBoxButton;
    public MessageBoxButton Button { get { return messageBoxButton; } set { messageBoxButton = value; } }
    private string message;
    public string Message { get { return message; } set { message = value; } }
    private string caption;
    public string Caption { get { return caption; } set { caption = value; } }
    public MessageBoxEx()
    {
      InitializeComponent();
    }
    public static MessageBoxResult ShowEx(Window owner, string message, string caption, MessageBoxButton button, MessageBoxImage icon)
    {
      MessageBoxEx messageBoxEx = new MessageBoxEx();
      messageBoxEx.Owner = owner;
      messageBoxEx.Message = message;
      messageBoxEx.Caption = caption;
      messageBoxEx.Button = button;
      messageBoxEx.Image = icon;
      if (messageBoxEx.ShowDialog() == false)
      {
        return MessageBoxResult.None;
      }
      return messageBoxEx.Result;
    } 
    private void ShowImage(string imageFileName)
    {
      BitmapImage image = new BitmapImage(new Uri("\\Image\\"+imageFileName, UriKind.Relative));
      image_icon.Source = image;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      textBlock_Info.Text = message;
      switch (messageBoxButton)
      {
        case MessageBoxButton.OKCancel:
          button_1.Content = "确定";
          button_2.Content = "取消";
          button_1.Visibility = Visibility.Visible;
          button_2.Visibility = Visibility.Visible;
          button_3.Visibility = Visibility.Hidden;
          button_3.Width = 0;
          button_1.Focus();
          break;
        case MessageBoxButton.YesNo:
          button_1.Content = "是";
          button_2.Content = "否";
          button_1.Visibility = Visibility.Visible;
          button_2.Visibility = Visibility.Visible;
          button_3.Visibility = Visibility.Hidden;
          button_3.Width = 0;
          button_1.Focus();
          break;
        case MessageBoxButton.YesNoCancel:
          button_1.Content = "是";
          button_2.Content = "否";
          button_3.Content = "取消";
          button_1.Visibility = Visibility.Visible;
          button_2.Visibility = Visibility.Visible;
          button_3.Visibility = Visibility.Visible;
          button_1.Focus();
          break;
        default:
          button_3.Content = "确定";
          button_1.Visibility = Visibility.Hidden;
          button_1.Width = 0;
          button_2.Visibility = Visibility.Hidden;
          button_2.Width = 0;
          button_3.Visibility = Visibility.Visible;
          button_3.Focus();
          break;
      }
      switch(messageBoxImage)
      {
        case MessageBoxImage.Error:
          ShowImage("Error.png");
          break;
        case MessageBoxImage.Information:
          ShowImage("information.png");
          break;
        case MessageBoxImage.Warning:
          ShowImage("Warning.png");
          break;
        case MessageBoxImage.Question:
          ShowImage("question.png");
          break;
        default:
          ShowImage("information.png");
          break;
      }
    }

    private void button_1_Click(object sender, RoutedEventArgs e)
    {
      if(button_1.Content as string == "确定")
      {
        messageBoxResult = MessageBoxResult.OK;
      }
      if (button_1.Content as string == "是")
      {
        messageBoxResult = MessageBoxResult.Yes;
      }
      DialogResult = true;
    }

    private void button_2_Click(object sender, RoutedEventArgs e)
    {
      if (button_2.Content as string == "取消")
      {
        messageBoxResult = MessageBoxResult.Cancel;
      }
      if (button_2.Content as string == "否")
      {
        messageBoxResult = MessageBoxResult.No;
      }
      DialogResult = true;
    }

    private void button_3_Click(object sender, RoutedEventArgs e)
    {
      if (button_3.Content as string == "否")
      {
        messageBoxResult = MessageBoxResult.No;
      }
      if (button_3.Content as string == "确定")
      {
        messageBoxResult = MessageBoxResult.OK;
      }
      DialogResult = true;
    }
  }
}
