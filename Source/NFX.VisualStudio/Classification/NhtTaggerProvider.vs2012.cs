using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace NFX.VisualStudio
{
  [ContentType(Constants.NFX)]
  [TagType(typeof(IClassificationTag))]
  [TagType(typeof(IErrorTag))]
  [Export(typeof(ITaggerProvider))]
  internal sealed class NhtTaggerProvider : DisposableObject, ITaggerProvider
  {
    [Export]
    [BaseDefinition("html")]
    [Name(Constants.NFX)]
    internal static ContentTypeDefinition NfxContentType { get; set; }

    [Export]
    [FileExtension(".nht")]
    [ContentType(Constants.NFX)]
    internal static FileExtensionToContentTypeDefinition NfxFileType { get; set; }

    [Import]
    internal IClassificationTypeRegistryService ClassificationTypeRegistry { get; set; }

    [Import]
    internal IBufferTagAggregatorFactoryService TagAggregatorFactoryService { get; set; }

    [Import]
    internal ITextBufferFactoryService BufferFactory { get; set; }

    [Import]
    internal IContentTypeRegistryService ContentTypeRegistryService { get; set; }

    [Import]
    internal SVsServiceProvider ServiceProvider { get; set; }

    internal TaskManager TaskManager;

    public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
    {
      TaskManager = new TaskManager(ServiceProvider);
      return new NhtTagger(ClassificationTypeRegistry, BufferFactory, TagAggregatorFactoryService, ContentTypeRegistryService, TaskManager) as ITagger<T>;
    }
    protected override void Destructor()
    {
      DisposeAndNull(ref TaskManager);
      base.Destructor();
    }
  }
}