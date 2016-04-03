using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace NFX.VisualStudio
{
  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX+ Constants.LACONF_TOKEN_NAME)]
  [Name(Constants.LACONF_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.High)]
  internal sealed class NfxLaconfToken : ClassificationFormatDefinition
  {
    public NfxLaconfToken()
    {
      DisplayName = "Laconfig";
      ForegroundColor = Colors.Red;
      BackgroundColor = Colors.Yellow;
    }
  }

  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX + Constants.EXPRESSION_TOKEN_NAME)]
  [Name(Constants.EXPRESSION_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.High)]
  internal sealed class ExpressionToken : ClassificationFormatDefinition
  {
    public ExpressionToken()
    {
      DisplayName = "Expression";
      ForegroundColor = Colors.DarkViolet;
    }
  }

  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX + Constants.EXPRESSION_BRACE_TOKEN_NAME)]
  [Name(Constants.EXPRESSION_BRACE_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.High)]
  internal sealed class ExpressionBraceToken : ClassificationFormatDefinition
  {
    public ExpressionBraceToken()
    {
      DisplayName = "Expression Brace";
      ForegroundColor = Colors.DarkViolet;
      BackgroundColor = Colors.Yellow;
    }
  }

  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX + Constants.KEYWORD_TOKEN_NAME)]
  [Name(Constants.KEYWORD_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.High, After = Priority.High)]
  internal sealed class KeyWordBraceToken : ClassificationFormatDefinition
  {
    public KeyWordBraceToken()
    {
      DisplayName = "Keyword Brace";
      ForegroundColor = Colors.Blue;
    }
  }


  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX + Constants.ERROR_TOKEN_NAME)]
  [Name(Constants.ERROR_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.High)]
  internal sealed class ErrorToken : ClassificationFormatDefinition
  {
    public ErrorToken()
    {
      DisplayName = "Error";
      IsItalic = true;
      IsBold = true;
    }
  }

  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX + Constants.BRACE_TOKEN_NAME)]
  [Name(Constants.BRACE_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.High)]
  internal sealed class BraceToken : ClassificationFormatDefinition
  {
    public BraceToken()
    {
      DisplayName = "Brace";
      ForegroundColor = Colors.Chocolate;
    }
  }

  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX + Constants.LITERAL_TOKEN_NAME)]
  [Name(Constants.LITERAL_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.High)]
  internal sealed class LiteralToken : ClassificationFormatDefinition
  {
    public LiteralToken()
    {
      DisplayName = "Literal";
      ForegroundColor = Colors.Sienna;
    }
  }

  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX + Constants.COMMENT_TOKEN_NAME)]
  [Name(Constants.COMMENT_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.High)]
  internal sealed class CommentToken : ClassificationFormatDefinition
  {
    public CommentToken()
    {
      DisplayName = "Comment";
      ForegroundColor = Colors.Green;
    }
  }

  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX + Constants.SPECIAL_TOKEN_NAME)]
  [Name(Constants.SPECIAL_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.High)]
  internal sealed class SpecialToken : ClassificationFormatDefinition
  {
    public SpecialToken()
    {
      DisplayName = "Special word";
      ForegroundColor = Colors.Blue;
      IsBold = true;
    }
  }

  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX + Constants.AREA_TOKEN_NAME)]
  [Name(Constants.AREA_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.High)]
  internal sealed class AreaToken : ClassificationFormatDefinition
  {
    public AreaToken()
    {
      DisplayName = "Area";
      BackgroundColor = Colors.DarkOrange;
      BackgroundOpacity = 0.2D;
    }
  }

  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX + Constants.STATEMENT_AREA_TOKEN_NAME)]
  [Name(Constants.STATEMENT_AREA_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.High)]
  internal sealed class StatementAreaToken : ClassificationFormatDefinition
  {
    public StatementAreaToken()
    {
      DisplayName = "Statement Area";
      BackgroundColor = Colors.BlueViolet;
      BackgroundOpacity = 0.2D;
    }
  }



  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX + Constants.EXPRESSION_AREA_TOKEN_NAME)]
  [Name(Constants.EXPRESSION_AREA_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.High)]
  internal sealed class ExpressionAreaToken : ClassificationFormatDefinition
  {
    public ExpressionAreaToken()
    {
      DisplayName = "Expression area";
      BackgroundColor = Colors.Green;
      BackgroundOpacity = 0.2D;
    }
  }
}