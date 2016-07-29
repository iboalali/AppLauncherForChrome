using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLauncherForChrome {
    class ChromeApp {
        /// <summary>
        /// The full path to the icon file
        /// </summary>
        private string iconPath;

        /// <summary>
        /// Gets or Sets the path to the icon path of the Chrome App.
        /// If you set the path the image will be loaded into the 
        /// property ImageSource
        /// </summary>
        public string IconPath
        {
            get
            {
                return iconPath;
            }
            set
            {
                IconImage = new System.Windows.Media.Imaging.BitmapImage( new Uri( value ) );
                iconPath = value;
            }
        }

        /// <summary>
        /// Gets or Sets the app ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Gets or Sets the app name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the number of times this app was launched
        /// </summary>
        public int Counter { get; private set; }

        /// <summary>
        /// Gets the icon image
        /// </summary>
        public System.Windows.Media.Imaging.BitmapImage IconImage { get; private set; }

        /// <summary>
        /// Gets the maximum number of all app launches 
        /// </summary>
        public static int MaxCounter { get; private set; }

        /// <summary>
        /// Gets the minimum number of all app launches
        /// </summary>
        public static int MinCounter { get; private set; }

        /// <summary>
        /// Increase the counter of this app by one
        /// </summary>
        public void IncreaseCounterByOne () {
            Counter++;
        }

    }
}
