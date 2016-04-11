using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using NFX.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace NFX.VisualStudio
{
  internal class TaskManager
  {
    private static ErrorListProvider m_ErrorListProvider;
    private static Guid m_Guid = Guid.NewGuid();
    public static void InitTaskManager(SVsServiceProvider serviceProvider)
    {
      if (m_ErrorListProvider != null)
        return;

      var _vsObject = (DTE)Package.GetGlobalService(typeof(DTE));

      m_ErrorListProvider = new ErrorListProvider(serviceProvider);
      m_ErrorListProvider.ProviderGuid = m_Guid;
      m_ErrorListProvider.ProviderName = m_Guid.ToString();
    }

    public static void AddError(Message message, string docName)
    {
      AddTask(message, TaskErrorCategory.Error, docName);
    }

    public static void AddWarning(Message message, string docName)
    {
      AddTask(message, TaskErrorCategory.Warning, docName);
    }

    public static void AddMessage(Message message, string docName)
    {
      AddTask(message, TaskErrorCategory.Message, docName);
    }

    public static void Refresh(string docName)
   {
      if (m_ErrorListProvider == null || m_ErrorListProvider.Tasks == null) return;

      var tasks = new List<Task>();
      for (int i = 0; i < m_ErrorListProvider.Tasks.Count; i++)
      {
        if (m_ErrorListProvider.Tasks[i].Document.Equals(docName, StringComparison.OrdinalIgnoreCase))
          tasks.Add(m_ErrorListProvider.Tasks[i]);
      }

      for (int i = 0; i < tasks.Count; i++)
      {
        m_ErrorListProvider.Tasks.Remove(tasks[i]);
      }
    }

    private static void AddTask(Message message, TaskErrorCategory category, string docName)
    {
      if (m_ErrorListProvider == null) return;

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

      m_ErrorListProvider.Tasks.Add(error);
      m_ErrorListProvider.Show();
    }

    static void NavigateDocument(object sender, EventArgs e)
    {
      Task task = sender as Task;
      if (task == null)
      {
        throw new ArgumentException("sender");
      }
      //use the helper class to handle the navigation
      OpenDocumentAndNavigateTo(task.Document, task.Line, task.Column);
    }

    public static void OpenDocumentAndNavigateTo(string path, int line, int column)
    {
      IVsUIShellOpenDocument openDoc =
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
      Guid logicalView = VSConstants.LOGVIEWID_Code;
      if (ErrorHandler.Failed(
          openDoc.OpenDocumentViaProject(path, ref logicalView, out sp, out hier, out itemid, out frame))
          || frame == null)
      {
        return;
      }
      object docData;
      frame.GetProperty((int)__VSFPROPID.VSFPROPID_DocData, out docData);

      // Get the VsTextBuffer  
      VsTextBuffer buffer = docData as VsTextBuffer;
      if (buffer == null)
      {
        IVsTextBufferProvider bufferProvider = docData as IVsTextBufferProvider;
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
      // Finally, perform the navigation.  
      IVsTextManager mgr = Package.GetGlobalService(typeof(VsTextManagerClass))
           as IVsTextManager;
      if (mgr == null)
      {
        return;
      }
      mgr.NavigateToLineAndColumn(buffer, ref logicalView, line, column, line, column);
    }
  }
}
