using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLauncherForChrome {
    static class Utils {
        private const string keyBase = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths";
        public static string GetPathForExe ( string fileName ) {
            RegistryKey localMachine = Registry.LocalMachine;
            RegistryKey fileKey = localMachine.OpenSubKey(string.Format(@"{0}\{1}", keyBase, fileName));
            object result = null;
            if ( fileKey != null ) {
                result = fileKey.GetValue( string.Empty );
            }
            fileKey.Close();

            return ( string ) result;
        }
    }
}
