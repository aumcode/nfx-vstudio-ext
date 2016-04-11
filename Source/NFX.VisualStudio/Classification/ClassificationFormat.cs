using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System.Globalization;
using System.Threading;

namespace NFX.VisualStudio
{
  public abstract class NFXClassificationFormatDef : ClassificationFormatDefinition
  {
    protected NFXClassificationFormatDef()
    { 
      Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

      BackgroundColor = Configurator.ParseColor(Configurator.GetFormatAttr(this, Configurator.CONFIG_BACKCOLOR_ATTR));
      ForegroundColor = Configurator.ParseColor(Configurator.GetFormatAttr(this, Configurator.CONFIG_FORECOLOR_ATTR));
      IsBold =  Configurator.GetFormatAttr(this, Configurator.CONFIG_BOLD_ATTR).AsBool();
      IsItalic = Configurator.GetFormatAttr(this, Configurator.CONFIG_ITALIC_ATTR).AsBool();
      BackgroundOpacity = Configurator.GetFormatAttr(this, Configurator.CONFIG_BACKOPACITYCOLOR_ATTR).AsNullableDouble(null);
    }
  }

  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX + Constants.LACONF_TOKEN_NAME)]
  [Name(Constants.LACONF_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.High)]
  internal sealed class NfxLaconfToken : NFXClassificationFormatDef
  {
    public NfxLaconfToken() : base() { }
  }

  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX + Constants.EXPRESSION_TOKEN_NAME)]
  [Name(Constants.EXPRESSION_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.High)]
  internal sealed class ExpressionToken : NFXClassificationFormatDef
  {
    public ExpressionToken() : base() { }
  }

  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX + Constants.EXPRESSION_BRACE_TOKEN_NAME)]
  [Name(Constants.EXPRESSION_BRACE_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.High)]
  internal sealed class ExpressionBraceToken : NFXClassificationFormatDef
  {
    public ExpressionBraceToken() : base() { }
  }

  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX + Constants.KEYWORD_TOKEN_NAME)]
  [Name(Constants.KEYWORD_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.High)]
  internal sealed class KeyWordBraceToken : NFXClassificationFormatDef
  {
    public KeyWordBraceToken() : base() { }
  }

  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX + Constants.BRACE_TOKEN_NAME)]
  [Name(Constants.BRACE_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.High)]
  internal sealed class BraceToken : NFXClassificationFormatDef
  {
    public BraceToken() : base() { }
  }

  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX + Constants.LITERAL_TOKEN_NAME)]
  [Name(Constants.LITERAL_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.High)]
  internal sealed class LiteralToken : NFXClassificationFormatDef
  {
    public LiteralToken() : base() { }
  }

  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX + Constants.COMMENT_TOKEN_NAME)]
  [Name(Constants.COMMENT_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.High,After = Priority.High)]
  internal sealed class CommentToken : NFXClassificationFormatDef
  {
    public CommentToken(): base() { }
  }

  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX + Constants.SPECIAL_TOKEN_NAME)]
  [Name(Constants.SPECIAL_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.High)]
  internal sealed class SpecialToken : NFXClassificationFormatDef
  {
    public SpecialToken() : base() { }
  }

  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX + Constants.AREA_TOKEN_NAME)]
  [Name(Constants.AREA_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.High)]
  internal sealed class AreaToken : NFXClassificationFormatDef
  {
    public AreaToken() : base() { }
  }

  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX + Constants.STATEMENT_AREA_TOKEN_NAME)]
  [Name(Constants.STATEMENT_AREA_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.High)]
  internal sealed class StatementAreaToken : NFXClassificationFormatDef
  {
    public StatementAreaToken() : base() { }
  }

  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX + Constants.EXPRESSION_AREA_TOKEN_NAME)]
  [Name(Constants.EXPRESSION_AREA_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.High)]
  internal sealed class ExpressionAreaToken : NFXClassificationFormatDef
  {
    public ExpressionAreaToken() : base() { }
  }

  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX + Constants.GROUP_1_TOKEN_NAME)]
  [Name(Constants.GROUP_1_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.Default)]
  internal sealed class Group1Token : NFXClassificationFormatDef
  {
    public Group1Token() : base() { }
  }

  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX + Constants.GROUP_2_TOKEN_NAME)]
  [Name(Constants.GROUP_2_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.Default)]
  internal sealed class Group2Token : NFXClassificationFormatDef
  {
    public Group2Token() : base() { }
  }

  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX + Constants.GROUP_3_TOKEN_NAME)]
  [Name(Constants.GROUP_3_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.Default)]
  internal sealed class Group3Token : NFXClassificationFormatDef
  {
    public Group3Token() : base() { }
  }

  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX + Constants.GROUP_4_TOKEN_NAME)]
  [Name(Constants.GROUP_4_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.Default)]
  internal sealed class Group4Token : NFXClassificationFormatDef
  {
    public Group4Token() : base() { }
  }

  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX + Constants.GROUP_5_TOKEN_NAME)]
  [Name(Constants.GROUP_5_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.Default)]
  internal sealed class Group5Token : NFXClassificationFormatDef
  {
    public Group5Token() : base() { }
  }

  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX + Constants.GROUP_6_TOKEN_NAME)]
  [Name(Constants.GROUP_6_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.Default)]
  internal sealed class Group6Token : NFXClassificationFormatDef
  {
    public Group6Token() : base() { }
  }

  [Export(typeof(EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = Constants.NFX + Constants.GROUP_7_TOKEN_NAME)]
  [Name(Constants.GROUP_7_TOKEN_NAME)]
  [UserVisible(true)]
  [Order(Before = Priority.Default)]
  internal sealed class Group7Token : NFXClassificationFormatDef
  {
    public Group7Token() : base() { }
  }
}