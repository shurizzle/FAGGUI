using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace MarkRight
{
  public class FlowDocumentParser
  {
    private FlowDocument doc;
    private Paragraph paragraph;
    private TextAlignment _alignment;

    public int FontSize { get; set; }
    public string FontFamily { get; set; }
    public TextAlignment TextAlignment
    {
      get
      {
        return _alignment;
      }

      set
      {
        _alignment = value;
        paragraph.TextAlignment = value;
      }
    }
    public Paragraph Paragraph
    {
      get
      {
        return paragraph;
      }
    }

    public FlowDocumentParser()
      : this(new FlowDocument())
    {
      doc.PagePadding = new Thickness(0);
    }

    public FlowDocumentParser(FlowDocument _doc)
    {
      doc = _doc;
      paragraph = new Paragraph();
      FontSize = 12;
      FontFamily = "Arial";
      TextAlignment = TextAlignment.Left;
      doc.Blocks.Add(paragraph);
    }

    public FlowDocument Doc
    {
      get
      {
        return doc;
      }
    }

    public Run CreatePart(string text)
    {
      Run part = new Run(text);
      part.FontSize = FontSize;
      part.FontFamily = new FontFamily(FontFamily);
      return part;
    }

    public Run CreatePart(string text, FontStyle style, FontWeight weight)
    {
      Run part = CreatePart(text);
      part.FontStyle = style;
      part.FontWeight = weight;
      return part;
    }

    public Run CreatePart(string text, FontStyle style)
    {
      Run part = CreatePart(text);
      part.FontStyle = style;
      return part;
    }

    public Run CreatePart(string text, FontWeight weight)
    {
      Run part = CreatePart(text);
      part.FontWeight = weight;
      return part;
    }

    public void AddNewLine()
    {
      paragraph.Inlines.Add(new LineBreak());
    }

    public Run AddPart(string text)
    {
      Run part = CreatePart(text);
      paragraph.Inlines.Add(part);
      return part;
    }

    public Run AddPart(string text, FontStyle style, FontWeight weight)
    {
      Run part = CreatePart(text, style, weight);
      paragraph.Inlines.Add(part);
      return part;
    }

    public Run AddPart(string text, FontStyle style)
    {
      Run part = AddPart(text);
      part.FontStyle = style;
      return part;
    }

    public Run AddPart(string text, FontWeight weight)
    {
      Run part = AddPart(text);
      part.FontWeight = weight;
      return part;
    }

    public Run AddBold(string text)
    {
      return AddPart(text, FontWeights.Bold);
    }

    public Run AddItalic(string text)
    {
      return AddPart(text, FontStyles.Italic);
    }

    public Run AddItalicBold(string text)
    {
      return AddPart(text, FontStyles.Italic, FontWeights.Bold);
    }

    public Hyperlink CreateLink(string name, string linkUrl)
    {
      Run part = CreatePart(name);
      part.Foreground = new SolidColorBrush(Colors.Blue);
      part.Cursor = Cursors.Hand;
      Hyperlink link = new Hyperlink(part);
      link.NavigateUri = new Uri(linkUrl);
      link.Foreground = new SolidColorBrush(Colors.Blue);
      link.RequestNavigate += new System.Windows.Navigation.RequestNavigateEventHandler(link_RequestNavigate);
      return link;
    }

    public Hyperlink CreateLink(string name, System.Windows.Navigation.RequestNavigateEventHandler handler)
    {
      Run part = CreatePart(name);
      part.Foreground = new SolidColorBrush(Colors.Blue);
      part.Cursor = Cursors.Hand;
      Hyperlink link = new Hyperlink(part);
      link.Foreground = new SolidColorBrush(Colors.Blue);
      link.RequestNavigate += handler;
      return link;
    }

    public Hyperlink AddLink(string name, string linkUrl)
    {
      Hyperlink link = CreateLink(name, linkUrl);
      paragraph.Inlines.Add(link);
      return link;
    }

    public Hyperlink AddLink(string name, System.Windows.Navigation.RequestNavigateEventHandler handler)
    {
      Hyperlink link = CreateLink(name, handler);
      paragraph.Inlines.Add(link);
      return link;
    }

    public Hyperlink AddLink(string linkUrl)
    {
      return AddLink(linkUrl, linkUrl);
    }

    private void AddText(string text, bool bold, bool italic)
    {
      if (text.Length < 1)
        return;

      if (bold)
        if (italic)
          AddItalicBold(text);
        else
          AddBold(text);
      else if (italic)
        AddItalic(text);
      else
        AddPart(text);
    }

    static private Regex URIRe = new Regex(@"(?<!\[\s*)((http[s]?|ftp):\/\/)([^:\/\s]+)(:[0-9]*)?((?:\/[\w:;%\.\-]*)+)?(\?(([\w:;%\.\-\[\]]+)=([\w:;%\.\-\[\]]*)))?(#(\S+))?", RegexOptions.IgnoreCase);

    static string _TransformLink(Match match)
    {
      return "[" + match.Value + "]";
    }

    public void AddMarkUp(string text)
    {
      StringBuilder sb = new StringBuilder();
      bool bold = false;
      bool italic = false;


      text = URIRe.Replace(text, new MatchEvaluator(_TransformLink));

      for (int i = 0; i < text.Length; i++)
      {
        switch (text[i])
        {
          case '*':
            if (bold)
            {
              AddText(sb.ToString(), bold, italic);
              bold = false;
              sb.Clear();
            }
            else
            {
              AddText(sb.ToString(), bold, italic);
              bold = true;
              sb.Clear();
            }
            break;
          case '_':
            if (italic)
            {
              AddText(sb.ToString(), bold, italic);
              italic = false;
              sb.Clear();
            }
            else
            {
              AddText(sb.ToString(), bold, italic);
              italic = true;
              sb.Clear();
            }
            break;
          case '>':
            bold = italic = false;
            AddText(sb.ToString(), bold, italic);
            sb.Clear();

            if (i == 0 || (i > 0 && text[i - 1] == '\n'))
            {
              for (; i < text.Length && text[i] != '\n'; i++)
                sb.Append(text[i]);
              Run part = AddPart(sb.ToString());
              part.Foreground = new SolidColorBrush(Colors.Green);
              AddText("\n", false, false);
              sb.Clear();
            }
            else
              sb.Append('>');
            break;
          case '[':
            AddText(sb.ToString(), bold, italic);
            sb.Clear();

            for (; i < text.Length && text[i] == ' '; i++) ;
            int start = ++i;
            for (; i < text.Length && text[i] != ']'; i++) ;
            string linkUrl = text.Substring(start, i - start);
            start = i;
            for (; i < text.Length && text[i] == ' '; i++) ;

            if (text[++i] == '(')
            {
              for (; i < text.Length && text[i] == ' '; i++) ;
              start = ++i;
              for (; i < text.Length && text[i] != ')'; i++) ;
              string name = text.Substring(start, i - start);
              AddLink(name, linkUrl);
            }
            else
            {
              i = start;
              AddLink(linkUrl, linkUrl);
            }
            break;
          case '\\':
            int next = i + 1;
            if (next < text.Length && (text[next] == '*' || text[next] == '_' || text[next] == '['))
              sb.Append(text[++i]);
            else
              sb.Append('\\');
            break;
          case '\n':
            AddText(sb.ToString(), bold, italic);
            sb.Clear();
            AddNewLine();
            break;
          default:
            sb.Append(text[i]);
            break;
        }
      }
      AddText(sb.ToString(), bold, italic);
    }

    private void link_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
    {
      Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
      e.Handled = true;
    }

    static public FlowDocument Parse(string text)
    {
      FlowDocumentParser res = new FlowDocumentParser();
      res.AddMarkUp(text);
      return res.Doc;
    }
  }
}
