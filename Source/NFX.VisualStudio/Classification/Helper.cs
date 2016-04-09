using System;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using NFX.CodeAnalysis;
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
  internal class TaskManager : DisposableObject
  {
    private static ErrorListProvider m_ErrorListProvider;

    public TaskManager(SVsServiceProvider serviceProvider)
    {
      m_ErrorListProvider = new ErrorListProvider(serviceProvider);
    }

    public void AddError(Message message)
    {
      AddTask(message, TaskErrorCategory.Error);
    }

    public void AddWarning(Message message)
    {
      AddTask(message, TaskErrorCategory.Warning);
    }

    public void AddMessage(Message message)
    {
      AddTask(message, TaskErrorCategory.Message);
    }

    public void Refresh()
    {
      if(m_ErrorListProvider != null)
      m_ErrorListProvider.Tasks.Clear();
    }

    private void AddTask(Message message, TaskErrorCategory category)
    {
      if(m_ErrorListProvider == null)
      return;
      var error = new ErrorTask
      {
        Category = TaskCategory.User,
        ErrorCategory = category,
        Text = message.From.MessageCodeToString(message.Code) +
        ((!string.IsNullOrWhiteSpace(message.Text)) ? "\"" + message.Text + "\"" : string.Empty) +
        ((message.AssociatedException != null) ? message.AssociatedException.ToMessageWithType() : string.Empty),

        Column = message.Position.ColNumber,
        Line = message.Position.LineNumber,
        Document = message.SourceCodeReference.SourceName
      };

      if (message.Token != null)
      {
        error.Column = message.Token.StartPosition.ColNumber;
        error.Line = message.Token.StartPosition.LineNumber;
      }

      m_ErrorListProvider.Tasks.Add(error);
    }

    protected override void Destructor()
    {
      if (m_ErrorListProvider != null && m_ErrorListProvider.Tasks != null)
        m_ErrorListProvider.Tasks.Clear();
      DisposeAndNull(ref m_ErrorListProvider);
      base.Destructor();
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