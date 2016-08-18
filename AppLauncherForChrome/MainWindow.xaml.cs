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
        private Chrome chromeStable;

        public MainWindow () {
            InitializeComponent();

            MainWebView.Navigate( new Uri( "https://www.google.com" ) );
            MainWebView.Visibility = Visibility.Hidden;

            BrushConverter bc = new BrushConverter();
            Brush b = (Brush)bc.ConvertFrom("#F2F2F2");
            b.Freeze();
            this.Background = b;
            ListBoxAppList.Background = b;

            // Initialize for chrome stable 
            chromeStable = new Chrome();

            // List all user names
            ComboBoxChromeUser.ItemsSource = chromeStable.GetUserNames();
            ComboBoxChromeUser.SelectedIndex = 0;

            // Load the last used user name
            // for testing 
            //ComboBoxChromeUser.SelectedValue = "Ibrahim Al-Alali";

            // List the apps ordered by the usage counter by descending order, then take the first 12 items
            ListBoxAppList.ItemsSource = chromeStable.ChromeAppsCollection.OrderByDescending( x => x.Counter ).Take( 12 );

            EventManager.RegisterClassHandler( typeof( ListBoxItem ),
                ListBoxItem.MouseLeftButtonDownEvent,
                new RoutedEventHandler( OnMouseLeftButtonDown ) );


            TextBoxSearchField.Focus();
        }

        private void MainWebView_LoadCompleted ( object sender, NavigationEventArgs e ) {
            WebBrowser wb = ( sender as WebBrowser );
            wb.LoadCompleted -= MainWebView_LoadCompleted;

            // Hide scrollbars
            mshtml.IHTMLDocument2 dom = (mshtml.IHTMLDocument2) wb.Document;
            dom.body.style.overflow = "hidden";

            // remove the unneccesery elements from the google website
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

            // Hides the script error masseges
            Utils.HideScriptErrors( sender as WebBrowser, true );

            // make the logo visible after loading and removing
            ( sender as WebBrowser ).Visibility = Visibility.Visible;
        }



        private void ComboBoxChromeUser_SelectionChanged ( object sender, SelectionChangedEventArgs e ) {
            chromeStable.SelectedUser = ( sender as ComboBox ).SelectedItem as String;


            if ( TextBoxSearchField.Text == string.Empty ) {
                ListBoxAppList.ItemsSource = chromeStable.ChromeAppsCollection
                    .OrderByDescending( x => x.Counter ).Take( 12 );
            } else {
                ListBoxAppList.ItemsSource = chromeStable.ChromeAppsCollection
                    .Where( x => x.Name.ToUpper().Contains( TextBoxSearchField.Text.ToUpper() ) ).Take( 12 );
            }



        }

        private void TextBoxSearchField_TextChanged ( object sender, TextChangedEventArgs e ) {
            if ( TextBoxSearchField.Text == string.Empty ) {
                ListBoxAppList.ItemsSource = chromeStable.ChromeAppsCollection.OrderByDescending( x => x.Counter ).Take( 12 );
            } else {
                ListBoxAppList.ItemsSource = chromeStable.ChromeAppsCollection
                    .Where( x => x.Name.ToUpper().Contains( TextBoxSearchField.Text.ToUpper() ) );
            }
        }

        private void TextBoxSearchField_KeyDown ( object sender, KeyEventArgs e ) {
            if ( e.Key == Key.Enter ) {
                if ( TextBoxSearchField.Text != string.Empty ) {
                    if ( ListBoxAppList.Items.Count != 0 ) {
                        ListBoxItem lbi = ListBoxAppList.ItemContainerGenerator.ContainerFromIndex(0) as ListBoxItem;
                        OnMouseLeftButtonDown( lbi, null );
                    } else {
                        System.Diagnostics.Process.Start( chromeStable.ExePath, "\"? " + ( sender as TextBox ).Text + "\"" );

                    }
                }

            }
        }

        private void OnMouseLeftButtonDown ( object sender, RoutedEventArgs e ) {
            if ( sender.GetType() == typeof( ListBoxItem ) ) {
                ListBoxItem lbi = sender as ListBoxItem;

                System.Diagnostics.Process chromeExe = new System.Diagnostics.Process();
                chromeExe.StartInfo.FileName = chromeStable.ExePath;
                chromeExe.StartInfo.Arguments = string.Format( "--profile-directory=\"{0}\" --app-id=", chromeStable.SelectedUser ) + ( lbi.Content as ChromeApp ).ID;

                bool result = chromeExe.Start();

                // increase counter of chrome app
            }
        }

        protected override void OnDeactivated ( EventArgs e ) {
            base.OnDeactivated( e );
            Environment.Exit( Environment.ExitCode );

        }



    }
}
