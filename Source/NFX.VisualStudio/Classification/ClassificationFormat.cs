using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace NFX.VisualStudio
{
  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX + Constants.LACONF_TOKEN_NAME)]
  [Name(Constants.LACONF_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.High, After = Priority.High)]
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
  [Order(Before = Priority.High, After = Priority.High)]
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
  [Order(Before = Priority.High, After = Priority.High)]
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
  [Order(Before = Priority.High, After = Priority.High)]
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
  [Order(Before = Priority.High, After = Priority.High)]
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
  [Order(Before = Priority.High, After = Priority.High)]
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
  [Order(Before = Priority.High, After = Priority.High)]
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
  [Order(Before = Priority.High, After = Priority.High)]
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
  [Order(Before = Priority.High, After = Priority.High)]
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
  [Order(Before = Priority.High, After = Priority.High)]
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
  [Order(Before = Priority.High, After = Priority.High)]
  internal sealed class ExpressionAreaToken : ClassificationFormatDefinition
  {
    public ExpressionAreaToken()
    {
      DisplayName = "Expression area";
      BackgroundColor = Colors.Green;
      BackgroundOpacity = 0.2D;
    }
  }

  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX + Constants.GROUP_1_TOKEN_NAME)]
  [Name(Constants.GROUP_1_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.High, After = Priority.High)]
  internal sealed class Group1Token : ClassificationFormatDefinition
  {
    public Group1Token()
    {
      ForegroundColor = Color.FromRgb(0, 0, 0xff);
    }
  }

  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX + Constants.GROUP_2_TOKEN_NAME)]
  [Name(Constants.GROUP_2_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.High, After = Priority.High)]
  internal sealed class Group2Token : ClassificationFormatDefinition
  {
    public Group2Token()
    {
      ForegroundColor = Color.FromRgb(0, 0x80, 0xc0);//0080C0
    }
  }

  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX + Constants.GROUP_3_TOKEN_NAME)]
  [Name(Constants.GROUP_3_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.High, After = Priority.High)]
  internal sealed class Group3Token : ClassificationFormatDefinition
  {
    public Group3Token()
    {
      ForegroundColor = Color.FromRgb(0, 0x80, 0xc0);//0080C0
    }
  }

  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX + Constants.GROUP_4_TOKEN_NAME)]
  [Name(Constants.GROUP_4_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.High, After = Priority.High)]
  internal sealed class Group4Token : ClassificationFormatDefinition
  {
    public Group4Token()
    {
      ForegroundColor = Color.FromRgb(0xbf, 0x00, 0x00);//BF0000
    }
  }

  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX + Constants.GROUP_5_TOKEN_NAME)]
  [Name(Constants.GROUP_5_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.High, After = Priority.High)]
  internal sealed class Group5Token : ClassificationFormatDefinition
  {
    public Group5Token()
    {
      ForegroundColor = Color.FromRgb(0xd5, 0x6a, 0x00);//D56A00
    }
  }

  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX + Constants.GROUP_6_TOKEN_NAME)]
  [Name(Constants.GROUP_6_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.High, After = Priority.High)]
  internal sealed class Group6Token : ClassificationFormatDefinition
  {
    public Group6Token()
    {
      ForegroundColor = Color.FromRgb(0x44, 0x88, 0x00);//448800
    }
  }

  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX + Constants.GROUP_7_TOKEN_NAME)]
  [Name(Constants.GROUP_7_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.High, After = Priority.High)]
  internal sealed class Group7Token : ClassificationFormatDefinition
  {
    public Group7Token()
    {
      ForegroundColor = Color.FromRgb(0xac, 0x33, 0x69);//AC3369
    }
  }
}