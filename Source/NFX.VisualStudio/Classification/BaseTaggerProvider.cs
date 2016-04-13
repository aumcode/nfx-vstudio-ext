using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;

namespace NFX.VisualStudio
{
  internal abstract class BaseTaggerProvider
  {
    private Dictionary<string, Dictionary<string, ITagger<ITag>>> m_Dic;
    protected Dictionary<string, Dictionary<string, ITagger<ITag>>> Dic
    {
      get
      {
        if (m_Dic != null) return m_Dic;
        m_Dic = new Dictionary<string, Dictionary<string, ITagger<ITag>>>();
        m_Dic.Add(typeof(IClassificationTag).Name, new Dictionary<string, ITagger<ITag>>());
        m_Dic.Add(typeof(IErrorTag).Name, new Dictionary<string, ITagger<ITag>>());

        return m_Dic;
      }
    }

    protected ITagger<T> GetTagger<T>(ITextBuffer buffer, Func<string, ITagger<ITag>> getter) where T : ITag
    {
      var t = typeof(T).Name;
      ITextDocument textDocument;
      buffer.Properties.TryGetProperty(typeof(ITextDocument), out textDocument);
      var docName = textDocument.FilePath;

      ITagger<ITag> tagger = null;
      if (Dic[t].ContainsKey(docName))
      {
        tagger = Dic[t][docName];
      }
      else
      {
        tagger = getter(docName);
        Dic[t][docName] = tagger;
      }

      return tagger as ITagger<T>;
    }
  }
}
