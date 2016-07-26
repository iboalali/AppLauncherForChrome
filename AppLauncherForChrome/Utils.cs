using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLauncherForChrome {
    static class Utils {

        private const string keyBase = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths";

        /// <summary>
        /// Returns the full path of an executable
        /// </summary>
        /// <param name="fileName">Name of the executable</param>
        /// <returns></returns>
        public static string GetPathForExe ( string exeName ) {
            // http://stackoverflow.com/a/909966
            // by Fredrik Mörk
            RegistryKey localMachine = Registry.LocalMachine;
            RegistryKey fileKey = localMachine.OpenSubKey(string.Format(@"{0}\{1}", keyBase, exeName));
            object result = null;
            if ( fileKey != null ) {
                result = fileKey.GetValue( string.Empty );
            }
            fileKey.Close();

            return ( string ) result;
        }

        /// <summary>
        /// Returns chrome's user name for a given user
        /// </summary>
        /// <param name="pathToUserFolder">Path to the user folder</param>
        /// <returns></returns>
        public static string getChromeUserName ( string pathToUserFolder ) {
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

            // for logged-in account, profile image url is at:
            // json.account_info[0].picture_url

            return name;

        }

        public static List<string> getChromeUsers () {
            List<string> user = new List<string>();
            List<string> userName = new List<string>();

            System.IO.DirectoryInfo dicInf = new System.IO.DirectoryInfo(Chrome.UserDataPath);
            System.IO.DirectoryInfo[] dl = dicInf.GetDirectories("Profile*");

            user.Add( "Default" );
            userName.Add( getChromeUserName( System.IO.Path.Combine( Chrome.UserDataPath, "Default" ) ) );
            foreach ( var item in dl ) {
                user.Add( item.Name );
                userName.Add( getChromeUserName( System.IO.Path.Combine( Chrome.UserDataPath, item.Name ) ) );
            }

            // continue here
            //ChromeUsers = user.Zip( userName, ( x, y ) => new { x, y } ).ToDictionary( k => k.x, k => k.y );
            //SettingsManager.SaveUserNameInSettings( ChromeUsers );

            return userName;
        }


    }
}
