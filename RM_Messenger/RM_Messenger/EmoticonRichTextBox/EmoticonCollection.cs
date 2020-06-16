using System;
using System.Collections.ObjectModel;

namespace RM_Messenger
{
  public class EmoticonCollection// : Collection<EmoticonMapper>
  {
    private static EmoticonCollection _instance;
    public static EmoticonCollection Instance => _instance ?? (_instance = new EmoticonCollection());

    public Collection<EmoticonMapper> Emoticons;

    public EmoticonCollection()
    {
      Emoticons = new Collection<EmoticonMapper>();
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/1.gif", Text = ":)" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/1.gif", Text = ":-)" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/2.gif", Text = ":(" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/3.gif", Text = ";)" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/4.gif", Text = ":D" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/5.gif", Text = ";;)" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/6.gif", Text = ">:D<" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/7.gif", Text = ":-/" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/7.gif", Text = ":/" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/8.gif", Text = ":x" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/8.gif", Text = ":X" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/9.gif", Text = "=\">" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/10.gif", Text = ":p" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/10.gif", Text = ":-p" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/10.gif", Text = ":P" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/10.gif", Text = ":-P" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/10.gif", Text = ":P" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/11.gif", Text = ":*" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/12.gif", Text = "=((" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/13.gif", Text = ":o" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/13.gif", Text = ":O" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/14.gif", Text = "x(" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/15.gif", Text = ":>" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/16.gif", Text = "b-)" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/17.gif", Text = ":s" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/18.gif", Text = "#:-S" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/19.gif", Text = ">:)" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/20.gif", Text = ":((" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/21.gif", Text = ":))" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/22.gif", Text = ":|" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/23.gif", Text = "/:)" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/24.gif", Text = "=))" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/25.gif", Text = "O:)" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/25.gif", Text = "o:)" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/26.gif", Text = ":-B" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/26.gif", Text = ":-b" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/27.gif", Text = "=;" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/28.gif", Text = "|-)" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/29.gif", Text = "8-|" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/30.gif", Text = "/:)" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/31.gif", Text = ":-&" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/32.gif", Text = ":-$" });      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/29.gif", Text = ":))" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/33.gif", Text = "[-(" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/34.gif", Text = ":O)" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/34.gif", Text = ":o)" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/35.gif", Text = "8-}" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/36.gif", Text = "<:-P" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/36.gif", Text = "<:-p" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/37.gif", Text = "(:|" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/38.gif", Text = "=P~" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/39.gif", Text = ":-?" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/40.gif", Text = "#-o" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/41.gif", Text = "=D>" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/42.gif", Text = ":-SS" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/42.gif", Text = ":-ss" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/43.gif", Text = "@-)" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/44.gif", Text = ":^O" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/44.gif", Text = ":^o" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/45.gif", Text = ":-W" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/45.gif", Text = ":-w" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/46.gif", Text = ":-<" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/47.gif", Text = ">:P" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/48.gif", Text = "<):)" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/49.gif", Text = ":@)" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/50.gif", Text = "3:-O" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/51.gif", Text = ":(|)" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/52.gif", Text = "~:>" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/53.gif", Text = "@};-" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/54.gif", Text = "%%-" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/55.gif", Text = "**==" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/56.gif", Text = "(~~)" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/57.gif", Text = "~o)" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/58.gif", Text = "*-:)" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/65.gif", Text = ":-\"" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/67.gif", Text = ":)>-" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/68.gif", Text = "[-x" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/69.gif", Text = "\\:D/" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/79.gif", Text = "(*)" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/102.gif", Text = "~x(" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/103.gif", Text = ":-h" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/103.gif", Text = ":-H" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/105.gif", Text = "8->" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/108.gif", Text = ":o3" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/113.gif", Text = ":bd" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/115.gif", Text = ":bz" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/120.gif", Text = "~^o^" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/121.gif", Text = "~’@^@|||" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/122.gif", Text = "[]-" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/123.gif", Text = "^O^||3" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/124.gif", Text = ":-(||>" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/125.gif", Text = "‘+_+" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/126.gif", Text = ":::^^:::" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/127.gif", Text = "o|^_^|o" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/128.gif", Text = ":puke!" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/120.gif", Text = "o|\\~" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/130.gif", Text = "o|:-)" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/131.gif", Text = "[]==[]" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/132.gif", Text = ":-)/\\:-)" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/133.gif", Text = ":(game)" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/134.gif", Text = "‘@-@" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/135.gif", Text = ":->~~" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/136.gif", Text = "?@_@?" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/137.gif", Text = ":(tv)" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/138.gif", Text = "&[]" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/139.gif", Text = "%||:-{" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/140.gif", Text = "%*-{" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/141.gif", Text = ":(fight)"});
    }
  }
}
