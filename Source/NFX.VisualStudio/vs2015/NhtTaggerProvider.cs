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
  internal sealed class NhtTaggerProvider : ITaggerProvider
  {
    [Export]
    [BaseDefinition("htmlx")]
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

    public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
    {
      return new NhtTagger(ClassificationTypeRegistry, BufferFactory, TagAggregatorFactoryService, ContentTypeRegistryService) as ITagger<T>;
    } 
  }
}       