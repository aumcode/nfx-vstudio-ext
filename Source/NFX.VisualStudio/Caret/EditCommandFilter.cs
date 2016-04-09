using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using NFX;
using System;
using System.Linq;

namespace NFX.VisualStudio.Caret
{
  class EditCommandFilter : IOleCommandTarget
  {
    public EditCommandFilter(IWpfTextView tv)
    {
      m_textView = tv;
    }

    private bool m_returnPressed = false;
    private IWpfTextView m_textView;
    private object ts_LockObject = new object();

    internal bool Added { get; set; }
    internal IOleCommandTarget NextTarget { get; set; }

    public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
    {
      var res = NextTarget.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
      if (m_returnPressed)
      {
        lock (ts_LockObject)
        {
          if (m_returnPressed)
          {
            var caret = m_textView.Caret;

            var curLine = m_textView.TextViewLines.GetTextViewLineContainingBufferPosition(caret.Position.BufferPosition);
            var idx = m_textView.TextViewLines.GetIndexOfTextLine(curLine);
            var prevLine = m_textView.TextViewLines[idx - 1];

            if (caret != null && curLine != null)
            {
              var text = m_textView.TextSnapshot.GetText();
              var prevText = text.Substring(prevLine.Extent.Span.Start, prevLine.Extent.Span.Length);
              var spaceCount = 0;
              var tabsCount = 0;
              var prevLength = prevText.Length;

              var i = 0;
              while (true)
              {
                if (i == prevLength ||
                  (prevText[i] != ' ' && prevText[i] != '\t'))
                  break;
                if (prevText[i] == ' ')
                  spaceCount++;
                if (prevText[i] == '\t')
                  tabsCount++;

                i++;
              }

              if (spaceCount > 0 || tabsCount > 0)
              {
                using (var edit = m_textView.TextBuffer.CreateEdit())
                {
                  edit.Insert(curLine.Start.Position, "{0}{1}".Args(new string('\t', tabsCount), new string(' ', spaceCount)));
                  edit.Apply();
                }
              }
            }
            m_returnPressed = false;
          }
        }
      }
      return res;
    }

    public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
    {
      if (pguidCmdGroup == typeof(VSConstants.VSStd2KCmdID).GUID)
      {
        for (int i = 0; i < cCmds; i++)
        {
          if (prgCmds[i].cmdID == (uint)VSConstants.VSStd2KCmdID.RETURN)
          {

            m_returnPressed = true;
          }
        }
      }
      return NextTarget.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
    }
  }
}
