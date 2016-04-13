using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using System.Collections.Generic;

namespace NFX.VisualStudio
{
  [ContentType("Laconic")]
  [TagType(typeof(IClassificationTag))]
  [TagType(typeof(IErrorTag))]
  [Export(typeof(ITaggerProvider))]
  internal sealed class LaconicTagProvider : BaseTaggerProvider, ITaggerProvider
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
    [FileExtension(".rschema")]
    [ContentType("Laconic")]
    internal static FileExtensionToContentTypeDefinition RschemaFileType { get; set; }

    [Export]
    [FileExtension(".acmb")]
    [ContentType("Laconic")]
    internal static FileExtensionToContentTypeDefinition AcmbFileType { get; set; }

    [Import]
    internal IClassificationTypeRegistryService ClassificationTypeRegistry { get; set; }

    [Import]
    internal IBufferTagAggregatorFactoryService TagAggregatorFactoryService { get; set; }

    [Import]
    internal ITextBufferFactoryService BufferFactory { get; set; }     

    public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
    {  
      return GetTagger<T>(buffer, (docName) => { return new LaconicClassifier(ClassificationTypeRegistry, BufferFactory, TagAggregatorFactoryService, docName); });    
    }
  }
}