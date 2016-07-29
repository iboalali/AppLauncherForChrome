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

        

        


    }
}
