using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLauncherForChrome {
    class Chrome {

        public Chrome () {
            ExePath = Utils.GetPathForExe( "chrome.exe" );
            Users = GetUsers();
            UserNames = GetUserNames();
            ChromeAppsUsageCounter = new Dictionary<string, int>();
            InitializeAppList( "Default" );
        }

        /// <summary>
        /// Contains the list of all of chrome's user profile names
        /// </summary>
        public List<string> Users { get; private set; }

        /// <summary>
        /// Contains the list of all of chrome's user names
        /// </summary>
        public List<string> UserNames { get; private set; }

        /// <summary>
        /// Returns the full path of the chrome executable
        /// </summary>
        public string ExePath { get; private set; }

        /// <summary>
        /// Gets a list of all chrome apps of a user
        /// </summary>
        public List<ChromeApp> ChromeAppsCollection { get; private set; }

        /// <summary>
        /// Gets a list of all chrome apps of a user with usage counter
        /// </summary>
        public Dictionary<string, int> ChromeAppsUsageCounter { get; private set; }

        /// <summary>
        /// Gets the maximum number of all app launches 
        /// </summary>
        public static int MaxCounter { get; private set; }

        /// <summary>
        /// Gets the minimum number of all app launches
        /// </summary>
        public static int MinCounter { get; private set; }

        /// <summary>
        /// Returns the full path of chrome's user data folder
        /// </summary>
        public string UserDataPath
        {
            get
            {
                return System.IO.Path.Combine(
                    Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData ),
                    String.Format( "Google\\Chrome\\User Data\\" ) );
            }
        }

        private List<string> GetUsers () {
            List<string> u = new List<string>();

            System.IO.DirectoryInfo dicInf = new System.IO.DirectoryInfo( UserDataPath );

            // Get all folders that start with the name "Profile"
            // All users folder are named Profile and then followed with a number
            System.IO.DirectoryInfo[] di = dicInf.GetDirectories( "Profile*" );

            // Add the user "Default" if it exists
            if ( System.IO.Directory.Exists( System.IO.Path.Combine( UserDataPath, "Default" ) ) ) {
                u.Add( "Default" );
            }

            // Add all user 
            foreach ( var item in di ) {
                u.Add( item.Name );
            }

            return u;

        }

        /// <summary>
        /// Returns a list of user names for all chrome users
        /// </summary>
        /// <returns></returns>
        public List<string> GetUserNames () {
            List<string> u = new List<string>();

            foreach ( var item in Users ) {
                string pathToUserFolder = System.IO.Path.Combine( UserDataPath, item );
                string json_raw = System.IO.File.ReadAllText(pathToUserFolder + "\\Preferences");
                dynamic json = Codeplex.Data.DynamicJson.Parse( json_raw );

                string name;
                try {
                    // gets the name from loggin accounts
                    name = json.account_info[0].full_name;
                } catch ( Exception ) {
                    // gets the name for an acount
                    name = json.profile.name;
                }

                u.Add( name );

                // for logged-in account, profile image url is at:
                // json.account_info[0].picture_url
            }

            return u;
        }

        public void InitializeAppList ( string profileName ) {
            ChromeAppsCollection = new List<ChromeApp>();

            string pathToUserApps = System.IO.Path.Combine(UserDataPath, profileName, "Web Applications\\");
            System.IO.DirectoryInfo chromeUserAppsDirInfo = new System.IO.DirectoryInfo( pathToUserApps );

            ChromeApp ca;
            foreach ( var item in chromeUserAppsDirInfo.GetDirectories() ) {
                ca = PopulateChromeAppInfo( item );
                ChromeAppsCollection.Add( ca );
            }


        }

        private ChromeApp PopulateChromeAppInfo ( System.IO.DirectoryInfo folder ) {
            ChromeApp ca = new ChromeApp();
            ca.ID = folder.Name.Substring( 5 );
            System.IO.FileInfo[] fi = folder.GetFiles();

            // make it smarter with choosing the icon, if there is more than one

            foreach ( System.IO.FileInfo item in fi ) {
                if ( item.Name.EndsWith( ".ico" ) ) {
                    ca.Name = item.Name.Substring( 0, item.Name.LastIndexOf( '.' ) );
                    ca.IconPath = item.FullName;

                    // Store app usage counter in the app instance, if it's exists. Otherwise add a new entry
                    if ( ChromeAppsUsageCounter.ContainsKey( ca.ID ) ) {
                        ca.Counter = ChromeAppsUsageCounter[ca.ID];
                    } else {
                        ChromeAppsUsageCounter.Add( ca.ID, 0 );
                    }

                    break;
                }
            }

            return ca;
        }


    }
}
