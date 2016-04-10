using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;

namespace NFX.VisualStudio
{
  internal sealed class NhtTagger : Classifier, ITagger<IClassificationTag>
  {
    internal NhtTagger(
      IClassificationTypeRegistryService typeService,
      ITextBufferFactoryService bufferFactory,
      IBufferTagAggregatorFactoryService tagAggregatorFactoryService,
      IContentTypeRegistryService contentTypeRegistryService,
      TaskManager taskManager)
      : base(typeService, bufferFactory, tagAggregatorFactoryService, taskManager)
    {
      m_CssContentType = contentTypeRegistryService.GetContentType("css");
      m_JavaScripContentType = contentTypeRegistryService.GetContentType("JavaScript");
    }

    public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

    private List<ITagSpan<IClassificationTag>> m_Oldtags;
    readonly IContentType m_CssContentType;
    readonly IContentType m_JavaScripContentType;

    public IEnumerable<ITagSpan<IClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
    {
      var tags = new List<ITagSpan<IClassificationTag>>();

      if (spans.Count < 1)
        return tags;
      var newSpanshot = spans[0].Snapshot;
      if (m_Snapshot == newSpanshot)
        return m_Oldtags;

      m_Snapshot = newSpanshot;                                     
      var text = new SnapshotSpan(m_Snapshot, new Span(0, m_Snapshot.Length)).GetText();

      var k = 0;
      while (k < text.Length)        //#[] - area
      {
        if (text[k] == '#')
        {
          if (text.Length - k > 1 &&
              text[k + 1] == '[' &&
              text[k] == '#' &&
              (k == 0 || text[k - 1] != '#'))
          {
            var o = k + 1;
            while (o < text.Length)
            {
              if (text[o] == ']')
              {
                tags.Add(CreateTagSpan(k, 2, NfxTokenTypes.ExpressionBrace)); //#[ 
                tags.Add(CreateTagSpan(o, 1, NfxTokenTypes.ExpressionBrace)); //]

                var j = k + 2;
                FindAdditionalsTokens(tags, NfxTokenTypes.KeyWord, text, j, o - j + 1);
                tags.Add(CreateTagSpan(j, o - j, NfxTokenTypes.Area));
                break;
              }
              o++;
            }
          }
        }

        if (text[k] == '@')    //@[statement]
        {
          if (text.Length - k > 1 &&
              text[k] == '@' &&
              text[k + 1] == '[' &&
              (k == 0 || text[k - 1] != '@'))
          {
            var o = k + 1;
            while (o < text.Length)
            {
              if (text[o] == ']')
              {
                tags.Add(CreateTagSpan(k, 2, NfxTokenTypes.ExpressionBrace)); //#[ 
                tags.Add(CreateTagSpan(o, 1, NfxTokenTypes.ExpressionBrace)); //]

                var j = k + 2;
                FindCSharpTokens(tags, text, j, o - j);
                FindAdditionalsTokens(tags, NfxTokenTypes.KeyWord, text, j, o - j + 1);
                tags.Add(CreateTagSpan(j, o - j, NfxTokenTypes.StatementArea));
                break;
              }
              o++;
            }
          }
        }

        if (text[k] == '?') //Experession ?[]
        {
          if (text.Length - k > 1
            && text[k + 1] == '[' &&
            text[k] == '?'
            && (k == 0 || text[k - 1] != '?'))
          {
            var o = k + 1;
            while (o < text.Length)
            {
              if (text[o] == ']')
              {
                tags.Add(CreateTagSpan(k, 2, NfxTokenTypes.ExpressionBrace)); //?[
                tags.Add(CreateTagSpan(o, 1, NfxTokenTypes.ExpressionBrace)); //]

                var j = k + 2;

                FindCSharpTokens(tags, text, j, o - j);
                FindAdditionalsTokens(tags, NfxTokenTypes.KeyWord, text, j, o - j + 1);
                tags.Add(CreateTagSpan(j, o - j, NfxTokenTypes.ExpressionArea));
                break;
              }
              o++;
            }
          }
        }

        if (text[k] == '#' && text[k] == '#' && (k == 0 || text[k - 1] != '#'))     //class section
        {
          if (text.Length - k >= CLASS_AREA_FULL.Length &&
              text.Substring(k, CLASS_AREA_FULL.Length) == CLASS_AREA_FULL) //#[class]
          {
            var j = k + CLASS_AREA_FULL.Length;
            var o = text.IndexOf("#[", j, StringComparison.OrdinalIgnoreCase);
            FindCSharpTokens(tags, text, j, o > -1 ? o - j : text.Length - j);
            FindAdditionalsTokens(tags, NfxTokenTypes.KeyWord, text, j, o > -1 ? o - j : text.Length - j);
          }
        }

        if (text[k] == '#' && text[k] == '#' && (k == 0 || text[k - 1] != '#'))       //laconic config
        {
          if (text.Length - k >= LACONFIG_START.Length &&
              text.Substring(k, LACONFIG_START.Length) == LACONFIG_START) //#<laconf>
          {
            var o = k + 1;
            while (o < text.Length)
            {
              if (text.Length - o >= LACONFIG_END.Length && text[o] == '#' &&
                  text.Substring(o, LACONFIG_END.Length) == LACONFIG_END) //#<laconf>
              {
                tags.Add(CreateTagSpan(k, LACONFIG_START.Length, NfxTokenTypes.Laconf)); //#<laconf>
                tags.Add(CreateTagSpan(o, LACONFIG_END.Length, NfxTokenTypes.Laconf)); //#<laconf>

                var j = k + LACONFIG_START.Length;
                GetLaconicTags(ref tags, text.Substring(j, o - j), j);
                break;
              }
              o++;
            }
          }
        }

        if (text[k] == '<')                //styles
        {
          if (text.Length - k > STYLE_START.Length &&
              text.Substring(k, STYLE_START.Length) == STYLE_START)
          {
            var o = k + STYLE_START.Length;
            while (o < text.Length)
            {
              if (text.Length - o >= STYLE_END.Length &&
                  text.Substring(o, STYLE_END.Length) == STYLE_END)
              {
                var tt = text.Substring(k + STYLE_START.Length, o - k - STYLE_START.Length);

                FindPropTags(tags, m_CssContentType, tt, k + STYLE_START.Length);
                break;
              }
              o++;
            }
          }
        }

        if (text[k] == '<')                            //scripts
        {
          if (text.Length - k > SCRIPT_START.Length &&
              text.Substring(k, SCRIPT_START.Length) == SCRIPT_START)
          {
            var o = k + SCRIPT_START.Length;
            while (o < text.Length)
            {
              if (text.Length - o >= SCRIP_END.Length &&
                  text.Substring(o, SCRIP_END.Length) == SCRIP_END)
              {
                var tt = text.Substring(k + SCRIPT_START.Length, o - k - SCRIPT_START.Length);
                FindPropTags(tags, m_JavaScripContentType, tt, k + STYLE_START.Length);
                break;
              }
              o++;
            }
          }
        }

        k++;
      }
      SynchronousUpdate();
      m_Oldtags = tags;
      return tags;
    }

    void SynchronousUpdate()
    {
      lock (ts_LockObject)
      {
        var t = TagsChanged;
        if (t != null)
          TagsChanged.Invoke(this, new SnapshotSpanEventArgs(new SnapshotSpan(m_Snapshot, 0, m_Snapshot.Length)));
      }
    }
  }

  internal sealed class NhtErrorTagger : Classifier,  ITagger<IErrorTag>
  {
    internal NhtErrorTagger(
      IClassificationTypeRegistryService typeService,
      ITextBufferFactoryService bufferFactory,
      IBufferTagAggregatorFactoryService tagAggregatorFactoryService,
      IContentTypeRegistryService contentTypeRegistryService,
      TaskManager taskManager)
      : base(typeService, bufferFactory, tagAggregatorFactoryService, taskManager) {  }
                                       
    public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

    private List<ITagSpan<IErrorTag>> m_ErrorTags; 

    public IEnumerable<ITagSpan<IErrorTag>> GetTags(NormalizedSnapshotSpanCollection spans)
    {
      var errorTags = new List<ITagSpan<IErrorTag>>();

      if (spans.Count < 1)
        return errorTags;
      var newSpanshot = spans[0].Snapshot;
      if (m_Snapshot == newSpanshot)
        return m_ErrorTags;

      m_Snapshot = newSpanshot;
      var text = new SnapshotSpan(m_Snapshot, new Span(0, m_Snapshot.Length)).GetText();

      var k = 0;
      while (k < text.Length)        //#[] - area
      {       

        if (text[k] == '#' && text[k] == '#' && (k == 0 || text[k - 1] != '#'))       //laconic config
        {
          if (text.Length - k >= LACONFIG_START.Length &&
              text.Substring(k, LACONFIG_START.Length) == LACONFIG_START) //#<laconf>
          {
            var o = k + 1;
            while (o < text.Length)
            {
              if (text.Length - o >= LACONFIG_END.Length && text[o] == '#' &&
                  text.Substring(o, LACONFIG_END.Length) == LACONFIG_END) //#<laconf>
              {                                                                                       
                var j = k + LACONFIG_START.Length;
                GetErrorLaconicTags(ref errorTags, text.Substring(j, o - j), j);
                break;
              }
              o++;
            }
          }
        }       

        k++;
      }
      SynchronousUpdate();
      m_ErrorTags =  errorTags;
      return m_ErrorTags;
    }

    void SynchronousUpdate()
    {
      lock (ts_LockObject)
      {
        var t = TagsChanged;
        if (t != null)
          TagsChanged.Invoke(this,
            new SnapshotSpanEventArgs(new SnapshotSpan(m_Snapshot, 0, m_Snapshot.Length)));
      }
    }
  }
}