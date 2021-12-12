using RenameRuleLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace TrimRuleLib
{
    class TrimRule : IRenameRule
    {
        public string MagicWord
        {
            get => "Trim";
        }
        //public string Word
        //{
        //    get; set;
        //}

        public string Rename(string original)
        {
            string result = original;
            //char[] firstChar = result.ToCharArray(0, 1);
            // result = string.Join(" ", result.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            result = result.Trim();
            Regex trimmer = new Regex(@"\s\s+");
            result = trimmer.Replace(result, "");
            
            return result;
        }

        public string Config(IRenameRule rule)
        {
            var myrule = rule as TrimRule;
            Window TrimDialog = new Window
            {
                Title = "Trim Config Dialog",
                Content = new UserControl1(),
                Width = 300,
                Height = 250
            };

            var userControl = TrimDialog.Content as UserControl1;
            //userControl.Handler += (string data) => {  };

            TrimDialog.ShowDialog();

            if (TrimDialog.DialogResult == true)
            {
                return $"{MagicWord}ming done!";
            }

                return "";
        }

        public IRenameRule Clone()
        {
            return new TrimRule() {};
        }
    }

    public class TrimRuleParser : IRenameRuleParser
    {
        public string MagicWord
        {
            get => "Trim";
        }

        public IRenameRule Parse(string line)
        {
            var tokens = line.Split('\"');

            IRenameRule rule = new TrimRule() {};
            return rule;
        }
    }
}