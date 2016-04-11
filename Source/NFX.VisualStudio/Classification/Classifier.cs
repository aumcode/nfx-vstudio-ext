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
using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace NFX.VisualStudio
{
  internal abstract class Classifier
  {
    public const string CONFIG_START = "#<conf>";
    public const string CONFIG_END = "#</conf>";
    public const string LACONFIG_START = "#<laconf>";
    public const string LACONFIG_END = "#</laconf>";
    public const string CLASS_AREA_FULL = "#[class]";
    public const string STYLE_START = "<style>";
    public const string STYLE_END = "</style>";
    public const string SCRIPT_START = "<script>";
    public const string SCRIP_END = "</script>";

    protected Classifier(
      IClassificationTypeRegistryService typeService,
      ITextBufferFactoryService bufferFactoryService,
      IBufferTagAggregatorFactoryService tagAggregatorFactoryService)
    {
      m_BufferFactoryService = bufferFactoryService;
      m_TagAggregatorFactoryService = tagAggregatorFactoryService;

      m_NfxTypes = new Dictionary<NfxTokenTypes, IClassificationType>();
      m_NfxTypes.Add(NfxTokenTypes.Laconf, typeService.GetClassificationType(Constants.LACONF_TOKEN_NAME));
      m_NfxTypes.Add(NfxTokenTypes.Expression, typeService.GetClassificationType(Constants.EXPRESSION_TOKEN_NAME));
      m_NfxTypes.Add(NfxTokenTypes.Statement, typeService.GetClassificationType(Constants.EXPRESSION_TOKEN_NAME));
      m_NfxTypes.Add(NfxTokenTypes.ExpressionBrace, typeService.GetClassificationType(Constants.EXPRESSION_BRACE_TOKEN_NAME));
      m_NfxTypes.Add(NfxTokenTypes.KeyWord, typeService.GetClassificationType(Constants.KEYWORD_TOKEN_NAME));
      m_NfxTypes.Add(NfxTokenTypes.Brace, typeService.GetClassificationType(Constants.BRACE_TOKEN_NAME));
      m_NfxTypes.Add(NfxTokenTypes.Literal, typeService.GetClassificationType(Constants.LITERAL_TOKEN_NAME));
      m_NfxTypes.Add(NfxTokenTypes.Comment, typeService.GetClassificationType(Constants.COMMENT_TOKEN_NAME));
      m_NfxTypes.Add(NfxTokenTypes.Special, typeService.GetClassificationType(Constants.SPECIAL_TOKEN_NAME));
      m_NfxTypes.Add(NfxTokenTypes.Area, typeService.GetClassificationType(Constants.AREA_TOKEN_NAME));
      m_NfxTypes.Add(NfxTokenTypes.ExpressionArea, typeService.GetClassificationType(Constants.EXPRESSION_AREA_TOKEN_NAME));
      m_NfxTypes.Add(NfxTokenTypes.StatementArea, typeService.GetClassificationType(Constants.STATEMENT_AREA_TOKEN_NAME));
      m_NfxTypes.Add(NfxTokenTypes.Group1, typeService.GetClassificationType(Constants.GROUP_1_TOKEN_NAME));
      m_NfxTypes.Add(NfxTokenTypes.Group2, typeService.GetClassificationType(Constants.GROUP_2_TOKEN_NAME));
      m_NfxTypes.Add(NfxTokenTypes.Group3, typeService.GetClassificationType(Constants.GROUP_3_TOKEN_NAME));
      m_NfxTypes.Add(NfxTokenTypes.Group4, typeService.GetClassificationType(Constants.GROUP_4_TOKEN_NAME));
      m_NfxTypes.Add(NfxTokenTypes.Group5, typeService.GetClassificationType(Constants.GROUP_5_TOKEN_NAME));
      m_NfxTypes.Add(NfxTokenTypes.Group6, typeService.GetClassificationType(Constants.GROUP_6_TOKEN_NAME));
      m_NfxTypes.Add(NfxTokenTypes.Group7, typeService.GetClassificationType(Constants.GROUP_7_TOKEN_NAME));


      var _vsObject = (DTE)Package.GetGlobalService(typeof(DTE));
      Window _editorWindow = null;

      m_windowEvents = _vsObject.Events.get_WindowEvents(_editorWindow);
      m_windowEvents.WindowClosing += OnWindowClosing;
    }

    protected readonly IDictionary<NfxTokenTypes, IClassificationType> m_NfxTypes;
    protected readonly ITextBufferFactoryService m_BufferFactoryService;
    protected readonly IBufferTagAggregatorFactoryService m_TagAggregatorFactoryService;
    protected ITextSnapshot m_Snapshot;
    protected object ts_LockObject = new object();

    private WindowEvents m_windowEvents;
    private string m_DocName;
    private string DocName
    {
      get
      {
        if (m_DocName.IsNotNullOrEmpty()) return m_DocName;

        var _vsObject = (DTE)Package.GetGlobalService(typeof(DTE));
        m_DocName = _vsObject.ActiveWindow.Document.FullName;

        return m_DocName;
      }
    }

    void OnWindowClosing(Window Window)
    {
      if (Window.Caption.Equals(DocName))
        TaskManager.Refresh(DocName);
    }

    protected void FindPropTags(List<ITagSpan<IClassificationTag>> tags, IContentType contetType, string textSpan, int bufferStartPosition)
    {
      var buffer = m_BufferFactoryService.CreateTextBuffer(textSpan,
                  contetType);
      var snapshotSpan = new SnapshotSpan(buffer.CurrentSnapshot, new Span(0, textSpan.Length));

      var ta = m_TagAggregatorFactoryService.CreateTagAggregator<IClassificationTag>(buffer);
      if (ta != null)
      {
        var ts = ta.GetTags(snapshotSpan);
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

    protected void FindAdditionalsTokens(List<ITagSpan<IClassificationTag>> tags, NfxTokenTypes type, string text, int start, int length)
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
          Find(word, tags, type, j);
          word = new StringBuilder();
        }
        if (word.Length > 0 && j + 1 == o)
          Find(word, tags, type, j);
        j++;
      }
    }

    private void Find(StringBuilder word, List<ITagSpan<IClassificationTag>> tags, NfxTokenTypes type,
     int currenPosition, params string[] additionalTokens)
    {
      if (Configurator.GetGroupKeywords(type.ToString()).Any(word.Compare))
      {
        var w = word.ToString();
        tags.Add(CreateTagSpan(currenPosition - w.Length, w.Length, type));
      }
    }

    protected void GetLaconicTags(
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

        if (!curType.HasValue ||
          (curType != NfxTokenTypes.Comment && curType != NfxTokenTypes.Literal))
        {
          if (Configurator.GetGroupKeywords(NfxTokenTypes.Group1.ToString()).Contains(token.Text))
            curType = NfxTokenTypes.Group1;

          if (Configurator.GetGroupKeywords(NfxTokenTypes.Group2.ToString()).Contains(token.Text))
            curType = NfxTokenTypes.Group2;

          if (Configurator.GetGroupKeywords(NfxTokenTypes.Group3.ToString()).Contains(token.Text))
            curType = NfxTokenTypes.Group3;

          if (Configurator.GetGroupKeywords(NfxTokenTypes.Group4.ToString()).Contains(token.Text))
            curType = NfxTokenTypes.Group4;

          if (Configurator.GetGroupKeywords(NfxTokenTypes.Group5.ToString()).Contains(token.Text))
            curType = NfxTokenTypes.Group5;

          if (Configurator.GetGroupKeywords(NfxTokenTypes.Group6.ToString()).Contains(token.Text))
            curType = NfxTokenTypes.Group6;

          if (Configurator.GetGroupKeywords(NfxTokenTypes.Group7.ToString()).Contains(token.Text))
            curType = NfxTokenTypes.Group7;
        }

        if (curType.HasValue)
          classifierTags.Add(CreateTagSpan(startPosition + token.StartPosition.CharNumber - 1, token.EndPosition.CharNumber - token.StartPosition.CharNumber + 1, curType.Value));
      }
    }

    protected void GetErrorLaconicTags(
      ref List<ITagSpan<IErrorTag>> tags,
      string src,
      int startPosition = 0)
    {
      var ml = new MessageList();
      var lxr = new LaconfigLexer(new StringSource(src), ml);
      var cfg = new LaconicConfiguration();
      var ctx = new LaconfigData(cfg);
      var p = new LaconfigParser(ctx, lxr, ml);
      p.Parse();
      TaskManager.Refresh(DocName);
      foreach (var message in ml)
      {
        TaskManager.AddError(message, DocName);


        var start = message.Token == null ?
          message.Position.CharNumber :
          message.Token.StartPosition.CharNumber > 4 ?
            message.Token.StartPosition.CharNumber - 5 :
            0;

        var length = message.Token == null ?
          src.Length - 1 - start :
          src.Length - start > 10 ?
            10 :
            src.Length - start;

        tags.Add(CreateTagSpan(start, length));
      }
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