using RenameRuleLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AddWordRuleLib
{
    enum TypeEnum : int
    {
         Prefix = 0,
         Suffix = 1
    }

    class AddWordRule : IRenameRule
    {
        public string MagicWord { get => "AddWord"; }
        public string Word { get; set; }
        public int Type { get; set; }

        public string Rename(string original)
        {
            string result = original;
            if (Type == (int)TypeEnum.Suffix)
            {
                int i = result.LastIndexOf('.');
                string name = result.Substring(0, i);
                string extension = result.Substring(i, original.Length - name.Length);

                result = $"{name}{Word}{extension}";
            }
            else
            {
                result = $"{Word}{result}";
            }

            return result;
        }

        public string Config(IRenameRule rule)
        {
            var myrule = rule as AddWordRule;
            Window addWordDialog = new Window
            {
                Title = "Add Word Config Dialog",
                Content = new UserControl1(myrule.Word, myrule.Type),
                Width = 300,
                Height = 250
            };

            var userControl = addWordDialog.Content as UserControl1;
            userControl.Handler += (string txbWord, int type) => { Word = txbWord; Type = type; };

            addWordDialog.ShowDialog();

            if (addWordDialog.DialogResult == true)
            {
                return $"{MagicWord} {Type} \"{Word}\"";
            }
            return "";
        }

        public IRenameRule Clone()
        {
            return new AddWordRule() { Word = "", Type= 0};
        }
    }

    public class AddWordRuleParser : IRenameRuleParser
    {
        public string MagicWord { get => "AddWord"; }

        public IRenameRule Parse(string line)
        {
            var tokens = line.Split('\"');

            int type = Int32.Parse(tokens[0].Replace($"{MagicWord} ", "").Replace(" ", ""));
            string word = tokens[1];

            IRenameRule rule = new AddWordRule() { Word = word, Type = type };
            return rule;
        }
    }
}
