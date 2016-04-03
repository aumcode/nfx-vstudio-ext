using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace NFX.VisualStudio
{
  internal static class NfxClassificationDefinition
  {
    [Export(typeof(ClassificationTypeDefinition))]
    [Name(Constants.LACONF_TOKEN_NAME)]
    internal static ClassificationTypeDefinition LaconfToken { get; set; }

    [Export(typeof(ClassificationTypeDefinition))]
    [Name(Constants.EXPRESSION_TOKEN_NAME)]
    internal static ClassificationTypeDefinition ExpressionToken { get; set; }

    [Export(typeof(ClassificationTypeDefinition))]
    [Name(Constants.EXPRESSION_BRACE_TOKEN_NAME)]
    internal static ClassificationTypeDefinition ExpressionBraceToken { get; set; }

    [Export(typeof(ClassificationTypeDefinition))]
    [Name(Constants.KEYWORD_TOKEN_NAME)]
    internal static ClassificationTypeDefinition KeywordToken { get; set; }

    [Export(typeof(ClassificationTypeDefinition))]
    [Name(Constants.ERROR_TOKEN_NAME)]
    internal static ClassificationTypeDefinition ErrorToken { get; set; }

    [Export(typeof(ClassificationTypeDefinition))]
    [Name(Constants.BRACE_TOKEN_NAME)]
    internal static ClassificationTypeDefinition BraceToken { get; set; }

    [Export(typeof(ClassificationTypeDefinition))]
    [Name(Constants.LITERAL_TOKEN_NAME)]
    internal static ClassificationTypeDefinition LiteralToken { get; set; }

    [Export(typeof(ClassificationTypeDefinition))]
    [Name(Constants.COMMENT_TOKEN_NAME)]
    internal static ClassificationTypeDefinition CommentToken { get; set; }

    [Export(typeof(ClassificationTypeDefinition))]
    [Name(Constants.SPECIAL_TOKEN_NAME)]
    internal static ClassificationTypeDefinition SpecialToken { get; set; }

    [Export(typeof(ClassificationTypeDefinition))]
    [Name(Constants.AREA_TOKEN_NAME)]
    internal static ClassificationTypeDefinition AreaToken { get; set; }

    [Export(typeof(ClassificationTypeDefinition))]
    [Name(Constants.STATEMENT_AREA_TOKEN_NAME)]
    internal static ClassificationTypeDefinition StatementAreaToken { get; set; }

    [Export(typeof(ClassificationTypeDefinition))]
    [Name(Constants.EXPRESSION_AREA_TOKEN_NAME)]
    internal static ClassificationTypeDefinition ExpressionAreaToken { get; set; }
  }
}

internal static class Constants
{
  internal const string NFX = "Nfx";

  internal const string LACONF_TOKEN_NAME = "LaconfToken";
  internal const string EXPRESSION_TOKEN_NAME = "ExpressionToken";
  internal const string EXPRESSION_BRACE_TOKEN_NAME = "ExpressionBraceTokenName";
  internal const string KEYWORD_TOKEN_NAME = "KeywordTokenName";
  internal const string ERROR_TOKEN_NAME = "ErrorTokenName";
  internal const string BRACE_TOKEN_NAME = "BraceTokenName";
  internal const string LITERAL_TOKEN_NAME = "LiteralTokenName";
  internal const string COMMENT_TOKEN_NAME = "CommentTokenName";
  internal const string SPECIAL_TOKEN_NAME = "SpecialTokenName";
  internal const string AREA_TOKEN_NAME = "AreaToken";
  internal const string STATEMENT_AREA_TOKEN_NAME = "StatementAreaToken";
  internal const string EXPRESSION_AREA_TOKEN_NAME = "ExpressionAreaToken";
}