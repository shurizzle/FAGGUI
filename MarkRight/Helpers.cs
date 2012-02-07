using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;

namespace MarkRight.Helpers
{
  public static class StringHelpers
  {
    public static List<string> SplitWithStringSeparator(this string line, string separator)
    {
      return line.Split(new string[] { separator }, StringSplitOptions.None).ToList();
    }
  }

  public static class XamlHelpers
  {
    static public MouseWheelEventHandler Bubbler = new MouseWheelEventHandler((object sender, MouseWheelEventArgs e) =>
      {
        e.Handled = true;
        var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
        eventArg.RoutedEvent = UIElement.MouseWheelEvent;
        eventArg.Source = sender;
        ((sender as Control).Parent as UIElement).RaiseEvent(eventArg);
      });

    public static void AddMarkedUpText(this StackPanel sp, string markedUpText)
    {
      Border border = new Border();
      border.BorderThickness = new Thickness(2);
      //border.BorderBrush = new SolidColorBrush(Colors.Blue);
      border.CornerRadius = new CornerRadius(8);
      border.Padding = new Thickness(3);
      FlowDocumentScrollViewer fdsv = new FlowDocumentScrollViewer();
      FlowDocument fd = FlowDocumentParser.Parse(markedUpText);
      fdsv.Document = fd;
      fdsv.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
      fdsv.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
      border.Background = new SolidColorBrush(Colors.YellowGreen);
      border.Child = fdsv;
      fd.PreviewMouseWheel += Bubbler;
      fdsv.PreviewMouseWheel += Bubbler;
      sp.Children.Add(border);
    }
  }
}
