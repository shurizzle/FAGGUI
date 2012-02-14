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
using System.Windows.Threading;
using System.Threading;
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
    public delegate void Loader();
    public delegate void FlowLoader(int id);
    public delegate void FlowsWriter(Flow[] flows);
    public delegate void DropWriter(Drop drops);

    private Base api;
    private ScrollChangedEventHandler scrollChanged_flows;
    private ScrollChangedEventHandler scrollChanged_drops;
    private FlowDocumentScrollViewer loader_fdsv;
    private string lastExpression;
    private int lastOffset;
    private List<Drop> currentDrops;

    #region Constructor
    public MainWindow()
    {
      api = new Base();
      scrollChanged_flows = new ScrollChangedEventHandler(scrollViewer1_ScrollChanged_flows);
      scrollChanged_drops = new ScrollChangedEventHandler(scrollViewer1_ScrollChanged_drops);
      InitializeComponent();
      {
        UI.GIFImageControl img = new UI.GIFImageControl()
        {
          GIFSource = "Resources/ajax-loader.gif",
          MaxHeight = 100,
          MaxWidth = 100
        };
        img.PreviewMouseWheel += FAGGUI.XamlHelpers.Bubbler;

        BlockUIContainer buic = new BlockUIContainer(img);
        buic.PreviewMouseWheel += FAGGUI.XamlHelpers.Bubbler;

        FlowDocument doc = new FlowDocument() { TextAlignment = TextAlignment.Center };
        doc.PreviewMouseWheel += FAGGUI.XamlHelpers.Bubbler;
        doc.Blocks.Add(buic);
        
        loader_fdsv = new FlowDocumentScrollViewer()
        {
          HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
          Document = doc,
          VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
          Background = new SolidColorBrush(Colors.White)
        };

        loader_fdsv.PreviewMouseWheel += FAGGUI.XamlHelpers.Bubbler;
        loader_fdsv.SizeChanged += FAGGUI.XamlHelpers.Resizer;
      }
      lastExpression = expressionBox.Text;
      lastOffset = 0;
      LoadFlows();
    }
    #endregion

    #region Scrollers
    private void scrollViewer1_ScrollChanged_flows(object sender, ScrollChangedEventArgs ev)
    {
      if (contentPanel.Children.Contains(loader_fdsv))
      {
        Point topLeft = loader_fdsv.TransformToVisual(contentPanel).Transform(new Point(0, 0));

        if (topLeft.Y <= (ev.VerticalOffset + ev.ViewportHeight))
        {
          scrollViewer1.ScrollChanged -= scrollChanged_flows;
          LoadFlows();
        }
      }
    }

    private void scrollViewer1_ScrollChanged_drops(object sender, ScrollChangedEventArgs ev)
    {
      if (contentPanel.Children.Contains(loader_fdsv))
      {
        Point topLeft = loader_fdsv.TransformToVisual(contentPanel).Transform(new Point(0, 0));

        if (topLeft.Y < (ev.VerticalOffset + ev.ViewportHeight))
        {
          scrollViewer1.ScrollChanged -= scrollChanged_drops;
          LoadDrops();
        }
      }
    }
    #endregion

    #region Infinite Drops
    public void _LoadFlow(int id)
    {
      try
      {
        currentDrops = api.GetFlow(id).Drops.ToList();
      }
      catch (WebException e)
      {
        MessageBox.Show(e.Message, "Communication error", MessageBoxButton.OK, MessageBoxImage.Stop);
        return;
      }

      LoadDrops();
    }

    public void LoadFlow(int id)
    {
      contentPanel.Children.Clear();
      contentPanel.Children.Add(loader_fdsv);
      scrollViewer1.ScrollChanged -= scrollChanged_flows;

      FlowLoader loader = new FlowLoader(_LoadFlow);
      loader.BeginInvoke(id, null, null);
    }

    private void _LoadDrops()
    {
      int max = Math.Min(10, currentDrops.Count);
      Drop[] drops = currentDrops.GetRange(0, max).ToArray();
      currentDrops.RemoveRange(0, max);

      for (int i = 0; i < drops.Length; i++)
      {
        drops[i].Populate();
        loader_fdsv.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DropWriter(WriteDrops), drops[i]).Wait();
      }
      loader_fdsv.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Loader(WriteDropsEnd));
    }

    private void WriteDropsEnd()
    {
      if (currentDrops.Count > 0)
        scrollViewer1.ScrollChanged += scrollChanged_drops;
      else
        contentPanel.Children.Remove(loader_fdsv);
    }

    private void WriteDrops(Drop drop)
    {
      contentPanel.Children.Remove(loader_fdsv);
      contentPanel.AddDrop(drop);
      contentPanel.Children.Add(loader_fdsv);
    }

    private void LoadDrops()
    {
      scrollViewer1.ScrollChanged -= scrollChanged_drops;
      Loader loader = new Loader(_LoadDrops);
      loader.BeginInvoke(null, null);
    }

    private void expressionBox_PreviewKeyUp(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Return)
      {
        e.Handled = true;
        lastExpression = ((TextBox)sender).Text;
        lastOffset = 0;
        LoadFlows();
      }
    }
    #endregion

    #region Infinite Flows
    private void _LoadFlows()
    {
      if (lastExpression.Length < 1)
        lastExpression = "*";

      Flow[] flows;

      try
      {
        flows = api.GetFlows(lastExpression, lastOffset, lastOffset + 10);
      }
      catch(WebException e)
      {
        MessageBox.Show(e.Message, "Communication error", MessageBoxButton.OK, MessageBoxImage.Stop);
        return;
      }

      loader_fdsv.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new FlowsWriter(_FlowsWrite), flows);
    }

    private void _FlowsWrite(Flow[] flows)
    {
      contentPanel.AddFlows(flows, this);
      contentPanel.Children.Remove(loader_fdsv);

      if (flows.Length == 10)
      {
        lastOffset += 10;
        contentPanel.Children.Add(loader_fdsv);

        scrollViewer1.ScrollChanged += scrollChanged_flows;
      }
      else
        scrollViewer1.ScrollChanged -= scrollChanged_flows;
    }

    private void LoadFlows()
    {
      Loader loader = new Loader(_LoadFlows);
      loader.BeginInvoke(null, null);
    }
    #endregion
  }
}
