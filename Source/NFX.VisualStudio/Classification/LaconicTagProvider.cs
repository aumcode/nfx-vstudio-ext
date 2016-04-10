using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using EnvDTE;

namespace NFX.VisualStudio
{
  [ContentType("Laconic")]
  [TagType(typeof(IClassificationTag))]
  [Export(typeof(ITaggerProvider))]
  internal sealed class LaconicTagProvider : ITaggerProvider
  {
    [Export]
    [BaseDefinition("code")]
    [Name("Laconic")]
    internal static ContentTypeDefinition NfxContentType { get; set; }

    [Export]
    [FileExtension(".laconf")]
    [ContentType("Laconic")]
    internal static FileExtensionToContentTypeDefinition LaconfFileType { get; set; }

    [Export]
    [FileExtension(".lac")]
    [ContentType("Laconic")]
    internal static FileExtensionToContentTypeDefinition LacFileType { get; set; }

    [Export]
    [FileExtension(".lacon")]
    [ContentType("Laconic")]
    internal static FileExtensionToContentTypeDefinition LaconFileType { get; set; }

    [Export]
    [FileExtension(".laconfig")]
    [ContentType("Laconic")]
    internal static FileExtensionToContentTypeDefinition LaconfigFileType { get; set; }

    [Export]
    [FileExtension(".rschema")]
    [ContentType("Laconic")]
    internal static FileExtensionToContentTypeDefinition RschemaFileType { get; set; }

    [Export]
    [FileExtension(".acmb")]
    [ContentType("Laconic")]
    internal static FileExtensionToContentTypeDefinition AcmbFileType { get; set; }

    [Import]
    internal SVsServiceProvider ServiceProvider { get; set; }

    [Import]
    internal IClassificationTypeRegistryService ClassificationTypeRegistry { get; set; }

    [Import]
    internal IBufferTagAggregatorFactoryService TagAggregatorFactoryService { get; set; }

    [Import]
    internal ITextBufferFactoryService BufferFactory { get; set; }

    public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
    { 
      var taskManager = new TaskManager(ServiceProvider);
      return new LaconicClassifier(ClassificationTypeRegistry, BufferFactory, TagAggregatorFactoryService, taskManager) as ITagger<T>;
    }
  }

  [ContentType("Laconic")]
  [TagType(typeof(IErrorTag))]
  [Export(typeof(ITaggerProvider))]
  internal sealed class LaconicErrorTagProvider : ITaggerProvider
  {
    [Export]
    [BaseDefinition("code")]
    [Name("Laconic")]
    internal static ContentTypeDefinition NfxContentType { get; set; }

    [Export]
    [FileExtension(".laconf")]
    [ContentType("Laconic")]
    internal static FileExtensionToContentTypeDefinition LaconfFileType { get; set; }

    [Export]
    [FileExtension(".lac")]
    [ContentType("Laconic")]
    internal static FileExtensionToContentTypeDefinition LacFileType { get; set; }

    [Export]
    [FileExtension(".lacon")]
    [ContentType("Laconic")]
    internal static FileExtensionToContentTypeDefinition LaconFileType { get; set; }

    [Export]
    [FileExtension(".laconfig")]
    [ContentType("Laconic")]
    internal static FileExtensionToContentTypeDefinition LaconfigFileType { get; set; }

    [Export]
    [FileExtension(".rschema")]
    [ContentType("Laconic")]
    internal static FileExtensionToContentTypeDefinition RschemaFileType { get; set; }

    [Export]
    [FileExtension(".acmb")]
    [ContentType("Laconic")]
    internal static FileExtensionToContentTypeDefinition AcmbFileType { get; set; }

    [Import]
    internal SVsServiceProvider ServiceProvider { get; set; }

    [Import]
    internal IClassificationTypeRegistryService ClassificationTypeRegistry { get; set; }

    [Import]
    internal IBufferTagAggregatorFactoryService TagAggregatorFactoryService { get; set; }

    [Import]
    internal ITextBufferFactoryService BufferFactory { get; set; }

    public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
    {
      var taskManager = new TaskManager(ServiceProvider);
      return new LaconicClassifier(ClassificationTypeRegistry, BufferFactory, TagAggregatorFactoryService, taskManager) as ITagger<T>;
    }
  }


  internal sealed class LaconicClassifier : Classifier, ITagger<IClassificationTag>
  {                                                   
    internal LaconicClassifier(
      IClassificationTypeRegistryService typeService,
      ITextBufferFactoryService bufferFactory,
      IBufferTagAggregatorFactoryService tagAggregatorFactoryService,
      TaskManager taskManager)
      : base(typeService, bufferFactory, tagAggregatorFactoryService, taskManager)
    {
    }

    public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
    private List<ITagSpan<IClassificationTag>> m_Oldtags;

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
      GetLaconicTags(ref tags, text);
      SynchronousUpdate(m_Snapshot);

      m_Oldtags = tags;
      return tags;
    }

    void SynchronousUpdate(ITextSnapshot snapshotSpan)
    {
      lock (ts_LockObject)
      {
        var t = TagsChanged;
        if (t != null)
          TagsChanged.Invoke(this, new SnapshotSpanEventArgs(new SnapshotSpan(snapshotSpan, 0, snapshotSpan.Length)));
      }
    }
  }


  internal sealed class LaconicErrorClassifier : Classifier, ITagger<IErrorTag>
  { 
    internal LaconicErrorClassifier(
      IClassificationTypeRegistryService typeService,
      ITextBufferFactoryService bufferFactory,
      IBufferTagAggregatorFactoryService tagAggregatorFactoryService,
      TaskManager taskManager)
      : base(typeService, bufferFactory, tagAggregatorFactoryService, taskManager)
    {
    }

    public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

    private List<ITagSpan<IErrorTag>> m_ErrorTags = new List<ITagSpan<IErrorTag>>();
    /// <summary>
    /// Search the given span for any instances of classified tags
    /// </summary>
    public IEnumerable<ITagSpan<IErrorTag>> GetTags(NormalizedSnapshotSpanCollection spans)
    {
      var tags = new List<ITagSpan<IErrorTag>>();

      if (spans.Count < 1)
        return tags;
      var newSpanshot = spans[0].Snapshot;
      if (m_Snapshot == newSpanshot)
        return m_ErrorTags;

      m_Snapshot = newSpanshot;

      var text = new SnapshotSpan(m_Snapshot, new Span(0, m_Snapshot.Length)).GetText();
      GetErrorLaconicTags(ref tags, text);
      SynchronousUpdate(m_Snapshot);

      m_ErrorTags = tags;
      return tags;
    }

    void SynchronousUpdate(ITextSnapshot snapshotSpan)
    {
      lock (ts_LockObject)
      {
        var t = TagsChanged;
        if (t != null)
          t.Invoke(this, new SnapshotSpanEventArgs(new SnapshotSpan(snapshotSpan, 0, snapshotSpan.Length)));
      }
    }
  }
}