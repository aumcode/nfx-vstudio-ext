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

    [Export(typeof(ClassificationTypeDefinition))]
    [Name(Constants.GROUP_1_TOKEN_NAME)]
    internal static ClassificationTypeDefinition Group1Token { get; set; }

    [Export(typeof(ClassificationTypeDefinition))]
    [Name(Constants.GROUP_2_TOKEN_NAME)]
    internal static ClassificationTypeDefinition Group2Token { get; set; }

    [Export(typeof(ClassificationTypeDefinition))]
    [Name(Constants.GROUP_3_TOKEN_NAME)]
    internal static ClassificationTypeDefinition Group3Token { get; set; }

    [Export(typeof(ClassificationTypeDefinition))]
    [Name(Constants.GROUP_4_TOKEN_NAME)]
    internal static ClassificationTypeDefinition Group4Token { get; set; }

    [Export(typeof(ClassificationTypeDefinition))]
    [Name(Constants.GROUP_5_TOKEN_NAME)]
    internal static ClassificationTypeDefinition Group5Token { get; set; }

    [Export(typeof(ClassificationTypeDefinition))]
    [Name(Constants.GROUP_6_TOKEN_NAME)]
    internal static ClassificationTypeDefinition Group6Token { get; set; }

    [Export(typeof(ClassificationTypeDefinition))]
    [Name(Constants.GROUP_7_TOKEN_NAME)]
    internal static ClassificationTypeDefinition Group7Token { get; set; }
  }
}

internal static class Constants
{
  internal const string NFX = "Nfx";

  internal const string LACONF_TOKEN_NAME = "LaconfToken";
  internal const string EXPRESSION_TOKEN_NAME = "ExpressionToken";
  internal const string EXPRESSION_BRACE_TOKEN_NAME = "ExpressionBraceTokenName";
  internal const string KEYWORD_TOKEN_NAME = "KeywordTokenName";
  internal const string BRACE_TOKEN_NAME = "BraceTokenName";
  internal const string LITERAL_TOKEN_NAME = "LiteralTokenName";
  internal const string COMMENT_TOKEN_NAME = "CommentTokenName";
  internal const string SPECIAL_TOKEN_NAME = "SpecialTokenName";
  internal const string AREA_TOKEN_NAME = "AreaToken";
  internal const string STATEMENT_AREA_TOKEN_NAME = "StatementAreaToken";
  internal const string EXPRESSION_AREA_TOKEN_NAME = "ExpressionAreaToken";

  internal const string GROUP_1_TOKEN_NAME = "Group1Token";
  internal const string GROUP_2_TOKEN_NAME = "Group2Token";
  internal const string GROUP_3_TOKEN_NAME = "Group3Token";
  internal const string GROUP_4_TOKEN_NAME = "Group4Token";
  internal const string GROUP_5_TOKEN_NAME = "Group5Token";
  internal const string GROUP_6_TOKEN_NAME = "Group6Token";
  internal const string GROUP_7_TOKEN_NAME = "Group7Token";
}