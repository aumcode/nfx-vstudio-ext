using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using NFX.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace NFX.VisualStudio
{

  internal class ErrorHelper : IServiceProvider
  {
    private Guid m_Guid = Guid.NewGuid();

    public object GetService(Type serviceType)
    {
      return Package.GetGlobalService(serviceType);
    }

    public ErrorListProvider GetErrorListProvider()
    {
      var provider = new ErrorListProvider(this);
      provider.ProviderName = m_Guid.ToString();
      provider.ProviderGuid = m_Guid;
      return provider;
    }
  }


  internal class TaskManager
  {
    private static ErrorListProvider m_ErrorListProvider;
    private static ErrorListProvider ErrorListProvider
    {
      get
      {
        return m_ErrorListProvider ?? (m_ErrorListProvider = new ErrorHelper().GetErrorListProvider());
      }
    }

    public static void AddError(Message message, string docName)
    {
      AddTask(message, TaskErrorCategory.Error, docName);
    }

    public static void Refresh(string docName)
    { 
      var tasks = new List<Task>();
      for (int i = 0; i < ErrorListProvider.Tasks.Count; i++)
      {
        if (ErrorListProvider.Tasks[i].Document.Equals(docName, StringComparison.OrdinalIgnoreCase))
          tasks.Add(ErrorListProvider.Tasks[i]);
      }

      for (int i = 0; i < tasks.Count; i++)
      {
        ErrorListProvider.Tasks.Remove(tasks[i]);
      }
    }

    private static void AddTask(Message message, TaskErrorCategory category, string docName)
    {
      if (ErrorListProvider == null) return;

      var error = new ErrorTask
      {
        Category = TaskCategory.CodeSense,
        ErrorCategory = category,
        Text = message.From.MessageCodeToString(message.Code) +
        ((!string.IsNullOrWhiteSpace(message.Text)) ? "\"" + message.Text + "\"" : string.Empty) +
        ((message.AssociatedException != null) ? message.AssociatedException.ToMessageWithType() : string.Empty),

        Column = message.Position.ColNumber - 1,
        Line = message.Position.LineNumber - 1,
        Document = docName
      };

      if (message.Token != null)
      {
        error.Column = message.Token.StartPosition.ColNumber - 1;
        error.Line = message.Token.StartPosition.LineNumber - 1;
      }
      if (docName.IsNotNullOrEmpty())
        error.Navigate += NavigateDocument;

      ErrorListProvider.Tasks.Add(error);
      ErrorListProvider.Show();
    }

    public static void NavigateDocument(object sender, EventArgs e)
    {
      Task task = sender as Task;
      if (task == null) return;

      var openDoc =
          Package.GetGlobalService(typeof(IVsUIShellOpenDocument))
                  as IVsUIShellOpenDocument;
      if (openDoc == null)
      {
        return;
      }
      IVsWindowFrame frame;
      Microsoft.VisualStudio.OLE.Interop.IServiceProvider sp;
      IVsUIHierarchy hier;
      uint itemid;
      var logicalView = VSConstants.LOGVIEWID_Code;
      if (ErrorHandler.Failed(
          openDoc.OpenDocumentViaProject(task.Document, ref logicalView, out sp, out hier, out itemid, out frame))
          || frame == null)
      {
        return;
      }
      object docData;
      frame.GetProperty((int)__VSFPROPID.VSFPROPID_DocData, out docData);

      var buffer = docData as VsTextBuffer;
      if (buffer == null)
      {
        var bufferProvider = docData as IVsTextBufferProvider;
        if (bufferProvider != null)
        {
          IVsTextLines lines;
          ErrorHandler.ThrowOnFailure(bufferProvider.GetTextBuffer(out lines));
          buffer = lines as VsTextBuffer;
          Debug.Assert(buffer != null, "IVsTextLines does not implement IVsTextBuffer");
          if (buffer == null)
          {
            return;
          }
        }
      }
      var mgr = Package.GetGlobalService(typeof(VsTextManagerClass))
           as IVsTextManager;
      if (mgr == null)
      {
        return;
      }
      mgr.NavigateToLineAndColumn(buffer, ref logicalView, task.Line, task.Column, task.Line, task.Column);
    }
  }
}
