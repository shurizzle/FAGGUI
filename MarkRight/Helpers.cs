using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MarkRight.Helpers
{
  public static class StringHelpers
  {
    public static List<string> SplitWithStringSeparator(this string line, string separator)
    {
      return line.Split(new string[] { separator }, StringSplitOptions.None).ToList();
    }
  }
}
