using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AddWordRuleLib
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1(string txbWord, int type)
        {
            //InitializeComponent();
            this.LoadViewFromUri("/AddWordRuleLib;component/AddWordConfigDialog.xaml");
            wordTextbox.Text = txbWord;
            _type = type;
        }

        private int _type { get; set; }
        Dictionary<int, string> _options;
        public delegate void MyDelegateType(string txbWord, int type);
        public event MyDelegateType Handler;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _options = new Dictionary<int, string>();
            _options.Add(0, "Add Prefix");
            _options.Add(1, "Add Suffix");

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
            string txbWord = wordTextbox.Text;
            int type = TypeCombobox.SelectedIndex;
            if (Handler != null)
            {
                Handler(txbWord, type);
            }
        }

        private void wordTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string txbWord = wordTextbox.Text;
            int type = TypeCombobox.SelectedIndex;
            if (Handler != null)
            {
                Handler(txbWord, type);
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
}