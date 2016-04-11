using System;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System.Text;

namespace NFX.VisualStudio
{
  internal class DteHelper
  {
    public static OutputWindowPane GetOutputWindow(SVsServiceProvider sVsServiceProvider)
    {
      var dte = (DTE)sVsServiceProvider.GetService(typeof(DTE));
      var window = dte.Windows.Item(EnvDTE.Constants.vsWindowKindOutput);
      var ow = (OutputWindow)window.Object;

      for (uint i = 1; i <= ow.OutputWindowPanes.Count; i++)
      {
        if (ow.OutputWindowPanes.Item(i).Name.Equals("NfxPane", StringComparison.CurrentCultureIgnoreCase))
        {
          return ow.OutputWindowPanes.Item(i);
        }
      }
      return ow.OutputWindowPanes.Add("NfxPane");
    }
  }
 

  public static class Helper
  {
    public static bool Compare(this StringBuilder builder, string value)
    {
      if (builder == null || value == null)
        return false;

      if (builder.Length != value.Length)
        return false;

      for (var i = 0; i < builder.Length; i++)
      {
        if (!builder[i].Equals(value[i]))
          return false;
      }
      return true;
    }
  }
}