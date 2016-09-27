using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace AppLauncherForChrome.Properties {

    struct ChromeAppSaveFile {
        public string id;
        public int count;
    }

    struct ChromeAppsSaveFile {
        public List<ChromeAppSaveFile> AppList;
    }

    [Serializable]
    class SaveFile {
        private static string SavePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        static SaveFile () {
            if ( !Directory.Exists( SavePath ) ) {
                Directory.CreateDirectory( SavePath );
            }
        }

        public static void save ( ChromeAppsSaveFile saveFile ) {
            IFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(Path.Combine(SavePath, "data"),
                         FileMode.Create,
                         FileAccess.Write, FileShare.None);
            formatter.Serialize( stream, saveFile );
            stream.Close();
        }

        public static ChromeAppsSaveFile load () {
            IFormatter formatter = new BinaryFormatter();
            if ( !File.Exists( Path.Combine( SavePath, "data" ) ) ) {
                return new ChromeAppsSaveFile() { AppList = null };
            }

            Stream stream = new FileStream(Path.Combine(SavePath, "data"),
                          FileMode.Open,
                          FileAccess.Read,
                          FileShare.Read);
            ChromeAppsSaveFile obj = (ChromeAppsSaveFile) formatter.Deserialize(stream);
            stream.Close();
            return obj;
        }



    }
}
