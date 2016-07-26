using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppLauncherForChrome {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow () {
            InitializeComponent();

            MainWebView.Navigate( new Uri( "https://www.google.com" ) );
            MainWebView.Visibility = Visibility.Hidden;

            BrushConverter bc = new BrushConverter();
            Brush b = (Brush)bc.ConvertFrom("#F2F2F2");
            b.Freeze();
            this.Background = b;
        }

        private void MainWebView_LoadCompleted ( object sender, NavigationEventArgs e ) {
            WebBrowser wb = ( sender as WebBrowser );
            wb.LoadCompleted -= MainWebView_LoadCompleted;

            // Hide scrollbars
            mshtml.IHTMLDocument2 dom = (mshtml.IHTMLDocument2)wb.Document;
            dom.body.style.overflow = "hidden";

            // remove  unneccesery elements
            dynamic d = ((mshtml.IHTMLDocument3) wb.Document).getElementById( "searchform" );
            d.parentNode.removeChild( d );
            d = ( ( mshtml.IHTMLDocument3 ) wb.Document ).getElementById( "prm-pt" );
            d.parentNode.removeChild( d );
            d = ( ( mshtml.IHTMLDocument3 ) wb.Document ).getElementById( "footer" );
            d.parentNode.removeChild( d );
            d = ( ( mshtml.IHTMLDocument3 ) wb.Document ).getElementById( "gb" );
            d.parentNode.removeChild( d );

            // Set Background color of the page to #F2F2F2
            ( ( mshtml.IHTMLDocument2 ) wb.Document ).body.style.backgroundColor = "#f2f2f2";

            // Remove the logo's subtext.
            // eg. in Germany the subtext is "Deutschland"
            d = ( ( mshtml.IHTMLDocument3 ) wb.Document ).getElementById( "hplogo" );
            if ( ( ( string ) d.IHTMLElement_innerHTML ).Contains( "logo-subtext" ) ) {
                d.removeChild( d.IHTMLDOMNode_firstChild );
            }

            // scroll into view and anchor to the top
            //( ( mshtml.IHTMLDocument3 ) wb.Document ).getElementById( "hplogo" ).scrollIntoView( false );

            // make the logo visible after loading and removeing
            ( sender as WebBrowser ).Visibility = Visibility.Visible;
        }

        private void ComboBoxChromeUser_SelectionChanged ( object sender, SelectionChangedEventArgs e ) {

        }

        private void TextBoxSearchField_TextChanged ( object sender, TextChangedEventArgs e ) {

        }

        private void TextBoxSearchField_KeyDown ( object sender, KeyEventArgs e ) {

        }
    }
}
