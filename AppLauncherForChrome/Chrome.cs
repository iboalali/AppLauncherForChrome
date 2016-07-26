using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLauncherForChrome {
    static class Chrome {

        static Chrome () {
            ExePath = Utils.GetPathForExe( "chrome.exe" );
            Users = new List<string>();
            UserNames = new List<string>();


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

        

    }
}
