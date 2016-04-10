using System.Collections.Generic;
using System.IO;
using System.Globalization;

using NFX.Environment;

namespace NFX.VisualStudio
{
  /// <summary>
  /// Used to access config for extension
  /// </summary>
  public static class Configurator
  {
    public const string CONFIG_KEYWORDS_SECTION = "keywords";
    public const string CONFIG_GROUP_SECTION = "group";
    public const string CONFIG_ENTRY_SECTION = "entry";
    public const string CONFIG_FORECOLOR_ATTR = "fg";
    public const string CONFIG_BACKCOLOR_ATTR = "bg";
    public const string CONFIG_FORMATS_SECTION = "formats";
    public const string CONFIG_BOLD_ATTR = "b";
    public const string CONFIG_ITALIC_ATTR = "i";
    public const string CONFIG_BACKOPACITYCOLOR_ATTR = "bgo";

    private static object s_Lock = new object();
    private static ConfigSectionNode s_Root;
    private static Dictionary<string, HashSet<string>> m_groups = new Dictionary<string, HashSet<string>>();

    /// <summary>
    /// Provides access to root of config file vs-ext located at  C:\Users\[USER NAME]\AppData\Roaming\nfx\vs-ext\config.*
    /// where * is any of supported nfx config extensions(xml, laconf, json...)
    /// </summary>
    public static IConfigSectionNode Root
    {
      get
      {
        lock (s_Lock)
        {
          if (s_Root == null)
          {
            var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            path = Path.Combine(path, "nfx", "vs-ext");
            var fPath = Path.Combine(path, "config");

            if (!Directory.Exists(path))
            {
              NFX.IOMiscUtils.EnsureAccessibleDirectory(path);
              var content = typeof(Configurator).GetText("config.laconf");
              File.WriteAllText(fPath + ".laconf", content);
            }

            try
            {
              s_Root = Configuration.ProviderLoadFromAnySupportedFormatFile(fPath).Root;
            }
            catch
            {
              s_Root = Configuration.NewEmptyRoot();
            }
          }

          return s_Root;
        }
      }
    }

    public static string GetFormatAttr(NFXClassificationFormatDef def, string aName)
    {
      if (def == null || aName.IsNullOrWhiteSpace()) return string.Empty;

      var t = def.GetType().Name;
      var sFormats = Root[CONFIG_FORMATS_SECTION];
      var sDef = sFormats[t];

      var attr = sDef.AttrByName(aName);
      if (!attr.Exists) attr = sFormats.AttrByName(aName);

      return attr.Value;
    }

    public static System.Windows.Media.Color? ParseColor(string value)
    {
      if (value.IsNullOrWhiteSpace()) return null;
      byte a = 0xff;
      byte r = 0;
      byte g = 0;
      byte b = 0;
      value = value.Trim();

      if (value.Length == 6 || value.Length == 8) //rgb
      {
        if (!byte.TryParse(value.Substring(0, 2), NumberStyles.HexNumber, null, out r)) return null;
        if (!byte.TryParse(value.Substring(2, 2), NumberStyles.HexNumber, null, out g)) return null;
        if (!byte.TryParse(value.Substring(4, 2), NumberStyles.HexNumber, null, out b)) return null;
        if (value.Length == 8) //rgba
        {
          if (!byte.TryParse(value.Substring(6, 2), NumberStyles.HexNumber, null, out a)) return null;
        }
      }
      else return null;

      return System.Windows.Media.Color.FromArgb(a, r, g, b);
    }

    public static HashSet<string> GetGroupKeywords(string group)
    {
      if (m_groups.ContainsKey(group)) return m_groups[group];

      var ks = Root[CONFIG_KEYWORDS_SECTION];

      var attr = ks.AttrByName(group);
      if (!attr.Exists) return new HashSet<string>();

      var res = new HashSet<string>(attr.Value.Split(','));
      m_groups[group] = res;

      return res;
    }
  }
}
