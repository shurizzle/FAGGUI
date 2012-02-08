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
using fag.api.types;

namespace FAGGUI
{
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
      Border border = new Border();
      border.BorderThickness = new Thickness(0, 0, 0, 10);
      border.CornerRadius = new CornerRadius(10);
      border.Padding = new Thickness(3);
      FlowDocumentScrollViewer fdsv = new FlowDocumentScrollViewer();
      FlowDocumentParser fdp = new FlowDocumentParser();
      fdp.TextAlignment = TextAlignment.Right;
      fdp.AddBold(drop.Author.Name + " at " + 
        String.Format("{0:dd/MM/yyyy HH:mm}", drop.UpdatedAt));
      fdp = new FlowDocumentParser(fdp.Doc);
      fdp.AddMarkUp(drop.Content);
      fdsv.Document = fdp.Doc;
      fdsv.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
      fdsv.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
      border.Background = new SolidColorBrush(Colors.YellowGreen);
      border.Child = fdsv;
      fdsv.Document.PreviewMouseWheel += Bubbler;
      fdsv.PreviewMouseWheel += Bubbler;
      fdsv.SizeChanged += Resizer;
      sp.Children.Add(border);
      return fdp.Doc;
    }

    static public void AddDrops(this StackPanel sp, Drop[] drops)
    {
      for (int i = 0; i < drops.Length; i++)
      {
        sp.AddDrop(drops[i]);
      }
    }
  }
}
