using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MarkRight.Helpers;
using fag.api;
using fag.api.types;

namespace FAGGUI
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private Base api;
    private ScrollChangedEventHandler scrollChanged_flows;
    private FlowDocumentScrollViewer loader_fdsv;

    public MainWindow()
    {
      api = new Base();
      scrollChanged_flows = new ScrollChangedEventHandler(scrollViewer1_ScrollChanged_flows);
      InitializeComponent();
      {
        FlowDocument doc = new FlowDocument();
        doc.Blocks.Add(new BlockUIContainer(new Image() { Source = FAGGUI.Properties.Resources.LoaderImage.ToBitmapImage() }));
        
        loader_fdsv = new FlowDocumentScrollViewer()
        {
          HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
          Document = doc,
          VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
          Background = new SolidColorBrush(Colors.White)
        };
      }
      scrollViewer1.ScrollChanged += scrollChanged_flows;
      LoadFlows();
    }

    private void scrollViewer1_ScrollChanged_flows(object sender, ScrollChangedEventArgs e)
    {
      UIElement el = contentPanel.Children[contentPanel.Children.Count - 1];
      GeneralTransform gt = el.TransformToVisual(contentPanel);

      Point topLeft = gt.Transform(new Point(0, 0));
      if (topLeft.Y < e.VerticalOffset + e.ViewportHeight)
      {
        MessageBox.Show("YAY!");
        scrollViewer1.ScrollChanged -= scrollChanged_flows;
      }
    }

    public void LoadFlow(int id)
    {
      contentPanel.Children.Clear();
      Drop[] drops;

      try
      {
        drops = api.GetFlow(id).GetDrops(10);
      }
      catch (WebException e)
      {
        MessageBox.Show(e.Message, "Communication error", MessageBoxButton.OK, MessageBoxImage.Stop);
        return;
      }

      contentPanel.AddDrops(drops);
    }

    private void expressionBox_PreviewKeyUp(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Return)
      {
        e.Handled = true;
        LoadFlows();
      }
    }

    private void LoadFlows()
    {
      contentPanel.Children.Clear();
      string exp = expressionBox.Text;

      if (exp.Length < 1)
        exp = "*";

      Flow[] flows;

      try
      {
        flows = api.GetFlows(exp);
      }
      catch(WebException e)
      {
        MessageBox.Show(e.Message, "Communication error", MessageBoxButton.OK, MessageBoxImage.Stop);
        return;
      }

      contentPanel.AddFlows(api.GetFlows(exp), this);
      contentPanel.Children.Add(loader_fdsv);
    }
  }
}
