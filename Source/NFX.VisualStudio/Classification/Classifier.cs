using System.Collections.Generic;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using NFX.CodeAnalysis;
using NFX.CodeAnalysis.Source;
using NFX.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.Text;
using NFX.CodeAnalysis.Laconfig;
using NFX.Environment;
using System.Text;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.Utilities;

namespace NFX.VisualStudio
{
  internal abstract class Classifier
  {

    protected Classifier(
      IClassificationTypeRegistryService typeService,
      ITextBufferFactoryService bufferFactoryService,
      IBufferTagAggregatorFactoryService tagAggregatorFactoryService,
      TaskManager taskManager)
    {
      m_BufferFactoryService = bufferFactoryService;
      m_TagAggregatorFactoryService = tagAggregatorFactoryService;

      m_TaskManger = taskManager;

      m_NfxTypes = new Dictionary<NfxTokenTypes, IClassificationType>();
      m_NfxTypes.Add(NfxTokenTypes.Laconf, typeService.GetClassificationType(Constants.LACONF_TOKEN_NAME));
      m_NfxTypes.Add(NfxTokenTypes.Expression, typeService.GetClassificationType(Constants.EXPRESSION_TOKEN_NAME));
      m_NfxTypes.Add(NfxTokenTypes.Statement, typeService.GetClassificationType(Constants.EXPRESSION_TOKEN_NAME));
      m_NfxTypes.Add(NfxTokenTypes.ExpressionBrace, typeService.GetClassificationType(Constants.EXPRESSION_BRACE_TOKEN_NAME));
      m_NfxTypes.Add(NfxTokenTypes.KeyWord, typeService.GetClassificationType(Constants.KEYWORD_TOKEN_NAME));
      m_NfxTypes.Add(NfxTokenTypes.Error, typeService.GetClassificationType(Constants.ERROR_TOKEN_NAME));
      m_NfxTypes.Add(NfxTokenTypes.Brace, typeService.GetClassificationType(Constants.BRACE_TOKEN_NAME));
      m_NfxTypes.Add(NfxTokenTypes.Literal, typeService.GetClassificationType(Constants.LITERAL_TOKEN_NAME));
      m_NfxTypes.Add(NfxTokenTypes.Comment, typeService.GetClassificationType(Constants.COMMENT_TOKEN_NAME));
      m_NfxTypes.Add(NfxTokenTypes.Special, typeService.GetClassificationType(Constants.SPECIAL_TOKEN_NAME));
      m_NfxTypes.Add(NfxTokenTypes.Area, typeService.GetClassificationType(Constants.AREA_TOKEN_NAME));
      m_NfxTypes.Add(NfxTokenTypes.ExpressionArea, typeService.GetClassificationType(Constants.EXPRESSION_AREA_TOKEN_NAME));
      m_NfxTypes.Add(NfxTokenTypes.StatementArea, typeService.GetClassificationType(Constants.STATEMENT_AREA_TOKEN_NAME));
    }

    protected readonly IDictionary<NfxTokenTypes, IClassificationType> m_NfxTypes;
    protected readonly ITextBufferFactoryService m_BufferFactoryService;
    protected readonly IBufferTagAggregatorFactoryService m_TagAggregatorFactoryService;
    protected ITextSnapshot m_Snapshot;
    private readonly TaskManager m_TaskManger;

    private static List<string> m_ContextCSharpTokens = new List<string>
    {
      "string",
      "get",
      "set",
    };

    protected void FindPropTags(List<ITagSpan<IClassificationTag>> tags, IContentType contetType, string textSpan, int bufferStartPosition)
    {
      var buffer = m_BufferFactoryService.CreateTextBuffer(textSpan,
                  contetType);
      var snapshotSpan = new SnapshotSpan(buffer.CurrentSnapshot, new Span(0, textSpan.Length));
      var ns =
        new NormalizedSnapshotSpanCollection(new List<SnapshotSpan>
        {
                    snapshotSpan
        });

      var ta = m_TagAggregatorFactoryService.CreateTagAggregator<IClassificationTag>(buffer);
      if (ta != null)
      {
        var ts = ta.GetTags(ns);
        foreach (var mappingTagSpan in ts)
        {
          var anchor =
            (SnapshotSpan)mappingTagSpan.Span.GetType()
              .GetField("_anchor", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                                   | BindingFlags.Static)
              .GetValue(mappingTagSpan.Span);
          tags.Add(
            new TagSpan<IClassificationTag>(
              new SnapshotSpan(m_Snapshot, bufferStartPosition + anchor.Start, anchor.Length + (contetType.TypeName == "css" ? 0 : 1)),
              mappingTagSpan.Tag));
        }
      }
    }

    protected void FindAdditionalsTokens(List<ITagSpan<IClassificationTag>> tags, NfxTokenTypes type, string text, int start, int length,
      params string[] additionalTokens)
    {
      var j = start;
      var word = new StringBuilder();
      var o = start + length;
      while (j < o)
      {
        var c = text[j];
        if (char.IsLetter(c))
        {
          word.Append(c);
        }
        else if (word.Length > 0)
        {
          Find(word, tags, type, j, additionalTokens);
          word = new StringBuilder();
        }
        if (word.Length > 0 && j + 1 == o)
          Find(word, tags, type, j, additionalTokens);
        j++;
      }
    }

    private void Find(StringBuilder word, List<ITagSpan<IClassificationTag>> tags, NfxTokenTypes type,
     int currenPosition, params string[] additionalTokens)
    {
      if (m_ContextCSharpTokens.Any(word.Compare) ||
        (additionalTokens != null && additionalTokens.Any(word.Compare)))
      {
        var w = word.ToString();
        tags.Add(CreateTagSpan(currenPosition - w.Length, w.Length, type));
      }
    }

    protected List<ITagSpan<IErrorTag>> GetLaconicTags(
      ref List<ITagSpan<IClassificationTag>> classifierTags,
      string src,
      int startPosition = 0)
    {
      var ml = new MessageList();
      var lxr = new LaconfigLexer(new StringSource(src), ml);
      var cfg = new LaconicConfiguration();
      var ctx = new LaconfigData(cfg);
      var p = new LaconfigParser(ctx, lxr, ml);
      p.Parse();
      var errorTags = new List<ITagSpan<IErrorTag>>();
      m_TaskManger.Refresh();
      foreach (var message in ml)
      {
        m_TaskManger.AddError(message);
        var start = message.Token == null ? 0 :
          message.Token.StartPosition.CharNumber > 4
            ? message.Token.StartPosition.CharNumber - 5 : 0;

        var length = message.Token == null ? src.Length - 1 :
          src.Length - start > 10 ? 10 : src.Length - start;
        errorTags.Add(CreateTagSpan(start, length));
      }
      for (var i = 0; i < lxr.Tokens.Count; i++)
      {
        NfxTokenTypes? curType = null;
        var token = lxr.Tokens[i];
        if (token.IsComment)
          curType = NfxTokenTypes.Comment;
        else if (token.IsIdentifier)
          curType = NfxTokenTypes.KeyWord;
        else if (token.IsSymbol || token.IsOperator)
          curType = NfxTokenTypes.Brace;
        else if (token.IsLiteral)
          curType = NfxTokenTypes.Literal;

        if (curType.HasValue)
          classifierTags.Add(CreateTagSpan(startPosition + token.StartPosition.CharNumber - 1, token.Text.Length, curType.Value));
      }
      return errorTags;
    }

    protected void FindCSharpTokens(List<ITagSpan<IClassificationTag>> tags, string text, int sourceStart, int length)
    {
      var ml = new MessageList();
      var lxr = new CSLexer(new StringSource(text.Substring(sourceStart, length)), ml);
      lxr.AnalyzeAll();
      for (var i = 0; i < lxr.Tokens.Count; i++)
      {
        NfxTokenTypes? curType = null;
        var token = lxr.Tokens[i];
        if (token.IsComment)
          curType = NfxTokenTypes.Comment;
        else if (token.IsKeyword)
          curType = NfxTokenTypes.KeyWord;
        else if (token.IsLiteral)
          curType = NfxTokenTypes.Literal;
        else if (token.IsSymbol || token.IsOperator)
          curType = NfxTokenTypes.Brace;

        if (curType.HasValue)
          tags.Add(CreateTagSpan(sourceStart + token.StartPosition.CharNumber - 1, token.Text.Length, curType.Value));
      }
    }

    protected TagSpan<IClassificationTag> CreateTagSpan(int startIndex, int length, NfxTokenTypes type)
    {
      var tokenSpan = new SnapshotSpan(m_Snapshot, new Span(startIndex, length));
      return
        new TagSpan<IClassificationTag>(tokenSpan,
          new ClassificationTag(m_NfxTypes[type]));
    }

    protected TagSpan<IErrorTag> CreateTagSpan(int startIndex, int length)
    {
      var tokenSpan = new SnapshotSpan(m_Snapshot, new Span(startIndex, length));
      return
        new TagSpan<IErrorTag>(tokenSpan, new ErrorTag());
    }
  }
}