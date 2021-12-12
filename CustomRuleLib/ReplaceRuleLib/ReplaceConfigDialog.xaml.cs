using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Packaging;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Navigation;

namespace ReplaceRuleLib
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1(string txbFrom, string txbTo, int type)
        {
            //InitializeComponent();
            this.LoadViewFromUri("/ReplaceRuleLib;component/ReplaceConfigDialog.xaml");
            replaceFromTextbox.Text = txbFrom;
            replaceToTextbox.Text = txbTo;
            _type = type;
        }

        private int _type { get; set; }
        Dictionary<int, string> _options;
        public delegate void MyDelegateType(string textFrom, string textTo, int type);
        public event MyDelegateType Handler;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _options = new Dictionary<int, string>();
            _options.Add(0, TypeEnum.Name.ToString());
            _options.Add(1, TypeEnum.Extension.ToString());
            
            TypeCombobox.ItemsSource = _options;
            TypeCombobox.SelectedIndex = _type;
            DataContext = this;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).DialogResult = true;
        }

        private void TypeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string textFrom = replaceFromTextbox.Text;
            string textTo = replaceToTextbox.Text;
            int type = TypeCombobox.SelectedIndex;
            if (Handler != null)
            {
                Handler(textFrom, textTo, type);
            }
        }

        private void replaceFromTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string textFrom = replaceFromTextbox.Text;
            string textTo = replaceToTextbox.Text;
            int type = TypeCombobox.SelectedIndex;
            if (Handler != null)
            {
                Handler(textFrom, textTo, type);
            }
        }

        private void replaceToTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string textFrom = replaceFromTextbox.Text;
            string textTo = replaceToTextbox.Text;
            int type = TypeCombobox.SelectedIndex;
            if (Handler != null)
            {
                Handler(textFrom, textTo, type);
            }
        }
    }
}

static class Extension
{
    public static void LoadViewFromUri(this UserControl window, string baseUri)
    {
        try
        {
            var resourceLocater = new Uri(baseUri, UriKind.Relative);
            var exprCa = (PackagePart)typeof(Application).GetMethod("GetResourceOrContentPart", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { resourceLocater });
            var stream = exprCa.GetStream();
            var uri = new Uri((Uri)typeof(BaseUriHelper).GetProperty("PackAppBaseUri", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null, null), resourceLocater);
            var parserContext = new ParserContext
            {
                BaseUri = uri
            };
            typeof(XamlReader).GetMethod("LoadBaml", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { stream, parserContext, window, true });

        }
        catch (Exception)
        {
            //log
        }
    }
}