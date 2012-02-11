using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarkRight;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.IO;
using fag.api.types;

namespace FAGGUI
{
  static public class DrawingHelpers
  {
    static public BitmapImage ToBitmapImage(this System.Drawing.Bitmap bitmap)
    {
      MemoryStream ms = new MemoryStream();
      bitmap.Save(ms, ImageFormat.Gif);
      ms.Position = 0;
      BitmapImage bi = new BitmapImage();
      bi.BeginInit();
      bi.StreamSource = ms;
      bi.EndInit();
      return bi;
    }
  }

  static public class XamlHelpers
  {
    static public MouseWheelEventHandler Bubbler = new MouseWheelEventHandler((object sender, MouseWheelEventArgs e) =>
      {
        e.Handled = true;
        var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
        eventArg.RoutedEvent = UIElement.MouseWheelEvent;
        eventArg.Source = sender;
        ((sender as Control).Parent as UIElement).RaiseEvent(eventArg);
      });
    static public SizeChangedEventHandler Resizer = new SizeChangedEventHandler((object sender, SizeChangedEventArgs e) =>
      {
        e.Handled = true;
        (sender as FlowDocumentScrollViewer).Document.PageWidth = e.NewSize.Width;
      });


    public static FlowDocument AddDrop(this StackPanel sp, Drop drop)
    {
      FlowDocumentParser fdp = new FlowDocumentParser() { TextAlignment = TextAlignment.Right };

      fdp.AddBold(drop.Author.Name + " at " + 
        String.Format("{0:dd/MM/yyyy HH:mm}", drop.UpdatedAt));
      fdp = new FlowDocumentParser(fdp.Doc);
      fdp.AddMarkUp(drop.Content);

      FlowDocumentScrollViewer fdsv = new FlowDocumentScrollViewer()
      {
        HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
        Document = fdp.Doc,
        VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
        Foreground = new SolidColorBrush(Colors.White)
      };

      Border border = new Border()
      {
        BorderThickness = new Thickness(0, 5, 0, 5),
        CornerRadius = new CornerRadius(10),
        Padding = new Thickness(3),
        Background = new SolidColorBrush(Colors.Black),
        Cursor = Cursors.Hand,
        Child = fdsv
      };

      fdp.Doc.PreviewMouseWheel += Bubbler;
      fdsv.PreviewMouseWheel += Bubbler;
      fdsv.SizeChanged += Resizer;

      sp.Children.Add(border);

      return fdp.Doc;
    }

    static public void AddDrops(this StackPanel sp, Drop[] drops)
    {
      for (int i = 0; i < drops.Length; i++)
        sp.AddDrop(drops[i]);
    }

    public static FlowDocument AddFlow(this StackPanel sp, Flow flow, MainWindow win)
    {
      FlowDocumentParser fdp = new FlowDocumentParser() { TextAlignment = TextAlignment.Left };
      fdp.Doc.Cursor = Cursors.Hand;

      fdp.AddBold(flow.Title);
      fdp.AddNewLine();
      fdp.AddBold("by " + flow.Author.Name + " at " + 
        String.Format("{0:dd/MM/yyyy HH:mm}", flow.UpdatedAt));
      fdp.AddNewLine();
      fdp.AddBold("Tags: ");
      fdp.AddPart(String.Join(", ", flow.Tags));

      FlowDocumentScrollViewer fdsv = new FlowDocumentScrollViewer()
      {
        HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
        Document = fdp.Doc,
        VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
        Foreground = new SolidColorBrush(Colors.White),
        Cursor = Cursors.Hand
      };

      Border border = new Border()
      {
        BorderThickness = new Thickness(2, 5, 2, 5),
        CornerRadius = new CornerRadius(10),
        Padding = new Thickness(3),
        Background = new SolidColorBrush(Colors.Black),
        Cursor = Cursors.Hand,
        Child = fdsv
      };

      border.PreviewMouseDown += new MouseButtonEventHandler((object sender, MouseButtonEventArgs e) =>
      {
        e.Handled = true;
        win.LoadFlow(flow.ID);
      });

      fdp.Doc.PreviewMouseWheel += Bubbler;
      fdsv.PreviewMouseWheel += Bubbler;
      fdsv.SizeChanged += Resizer;

      sp.Children.Add(border);

      return fdp.Doc;
    }

    static public void AddFlows(this StackPanel sp, Flow[] flows, MainWindow win)
    {
      for (int i = 0; i < flows.Length; i++)
        sp.AddFlow(flows[i], win);
    }
  }
}
