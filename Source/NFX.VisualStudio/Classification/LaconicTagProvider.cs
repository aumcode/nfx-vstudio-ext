using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace NFX.VisualStudio
{
  [ContentType("Laconic")]
  [TagType(typeof(IClassificationTag))]
  [TagType(typeof(IErrorTag))]
  [Export(typeof(ITaggerProvider))]
  internal sealed class LaconicTagProvider : DisposableObject, ITaggerProvider
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

    private TaskManager m_TaskManager;

    public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
    {
      m_TaskManager = new TaskManager(ServiceProvider);
      return new LaconicClassifier(ClassificationTypeRegistry, BufferFactory, TagAggregatorFactoryService, m_TaskManager) as ITagger<T>;
    }

    protected override void Destructor()
    {
      DisposeAndNull(ref m_TaskManager);
      base.Destructor();
    }
  }


  internal sealed class LaconicClassifier : Classifier, ITagger<IClassificationTag>, ITagger<IErrorTag>
  {
    public const string LACONFIG_START = "#<laconf>";
    public const string LACONFIG_END = "#</laconf>";

    internal LaconicClassifier(
      IClassificationTypeRegistryService typeService,
      ITextBufferFactoryService bufferFactory,
      IBufferTagAggregatorFactoryService tagAggregatorFactoryService,
      TaskManager taskManager)
      : base(typeService, bufferFactory, tagAggregatorFactoryService, taskManager)
    {
    }

    public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
    object ts_LockObject = new object();

    private List<ITagSpan<IClassificationTag>> m_Oldtags;
    private static List<ITagSpan<IErrorTag>> m_ErrorTags;

    IEnumerable<ITagSpan<IErrorTag>> ITagger<IErrorTag>.GetTags(NormalizedSnapshotSpanCollection spans)
    {

      if (spans.Count > 0)
      {
        var newSpanshot = spans[0].Snapshot;
        if (m_Snapshot != newSpanshot)
        {
          m_Snapshot = newSpanshot;

          lock (ts_LockObject)
          {
            var t = TagsChanged;
            if (t != null)
              TagsChanged.Invoke(this,
              new SnapshotSpanEventArgs(new SnapshotSpan(newSpanshot, 0, newSpanshot.Length)));
          }
        }
      }
      return m_ErrorTags ?? new List<ITagSpan<IErrorTag>>();

    }
    /// <summary>
    /// Search the given span for any instances of classified tags
    /// </summary>
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
      var errorTags = GetLaconicTags(ref tags, text);
      lock (ts_LockObject)
      {
        m_ErrorTags = errorTags;
      }
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
}