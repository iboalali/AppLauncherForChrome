using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLauncherForChrome {
    static class Chrome {

        static Chrome () {
            ExePath = Utils.GetPathForExe( "chrome.exe" );
            Users = GetUsers();
            UserNames = GetUserNames();

        }

        /// <summary>
        /// Contains the list of all of chrome's user profile names
        /// </summary>
        public static List<string> Users { get; private set; }

        /// <summary>
        /// Contains the list of all of chrome's user names
        /// </summary>
        public static List<string> UserNames { get; private set; }

        /// <summary>
        /// Returns the full path of the chrome executable
        /// </summary>
        public static string ExePath { get; private set; }

        /// <summary>
        /// Returns the full path of chrome's user data folder
        /// </summary>
        public static string UserDataPath
        {
            get
            {
                return System.IO.Path.Combine(
                    Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData ),
                    String.Format( "Google\\Chrome\\User Data\\" ) );
            }
        }

        private static List<string> GetUsers () {
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
        /// Returns chrome's user name for a given user
        /// </summary>
        /// <param name="pathToUserFolder">Path to the user folder</param>
        /// <returns></returns>
        public static List<string> GetUserNames () {
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




    }
}
