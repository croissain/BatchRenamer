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

namespace BatchRename
{
    /// <summary>
    /// Interaction logic for PresetControl.xaml
    /// </summary>
    public partial class PresetControl : Window
    {
        public PresetControl()
        {
            //InitializeComponent();
            this.LoadViewFromUri("/BatchRename;component/PresetControl.xaml");
        }

        public delegate void MyDelegateType(string data);
        public event MyDelegateType Handler;

        private void btnOk(object sender, RoutedEventArgs e)
        {
            if (Handler != null)
            {
                string text = txtName.Text;
                if (text == "")
                    text = "preset.txt";
                else
                {
                    if(!text.Contains(".txt"))
                        text = $"{text}.txt";
                }
                Handler(text);
            }
            DialogResult = true;
            Close();
        }
    }

    static class Extension
    {
        public static void LoadViewFromUri(this Window window, string baseUri)
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
