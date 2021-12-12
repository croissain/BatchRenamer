using RenameRuleLib;
using System;
using System.Windows;

namespace ReplaceRuleLib
{
    enum TypeEnum : int
    {
        Name = 0,
        Extension = 1,
    }
    class ReplaceRule : IRenameRule
    {
        public string MagicWord { get => "Replace"; }
        public string Needle { get; set; }
        public string Replacer { get; set; }
        public int Type { get; set; }

        public string Rename(string original)
        {
            string result = original;
            if (Needle != null && Needle != "" && Replacer != null)
            {
                if (Type == (int)TypeEnum.Extension)
                {
                    int i = result.LastIndexOf('.');
                    string name = result.Substring(0, i);
                    string extension = result.Substring(i, original.Length - name.Length);
                    extension = extension.Replace(Needle, Replacer);

                    result = $"{name}{extension}";
                }
                else
                {
                    result = result.Replace(Needle, Replacer);
                }
            }
            return result;
        }

        public string Config(IRenameRule rule)
        {
            var myrule = rule as ReplaceRule;
            Window replaceDialog = new Window
            {
                Title = "Replace Config Dialog",
                Content = new UserControl1(myrule.Needle, myrule.Replacer, myrule.Type),
                Width = 300,
                Height = 250
            };

            var userControl = replaceDialog.Content as UserControl1;
            userControl.Handler += (string textFrom, string textTo, int type) => 
            {
                Needle = textFrom;
                Replacer = textTo;
                Type = type;
            };

            replaceDialog.ShowDialog();

            if (replaceDialog.DialogResult == true)
            {
                return $"{MagicWord} {Type} \"{Needle}\" => \"{Replacer}\"";
            }
            return "";
        }

        public IRenameRule Clone()
        {
            return new ReplaceRule() { Needle = "", Replacer = "", Type = 0};
        }
    }

    public class ReplaceRuleParser : IRenameRuleParser
    {
        public string MagicWord { get => "Replace"; }

        public IRenameRule Parse(string line)
        {
            var tokens = line.Split('\"');

            int type = Int32.Parse(tokens[0].Replace($"{MagicWord} ", "").Replace(" ", ""));
            string needle = tokens[1];
            string replacer = tokens[3].Replace(" => ", "");

            IRenameRule rule = new ReplaceRule() { Needle = needle, Replacer = replacer, Type = type };
            return rule;
        }
    }
}
