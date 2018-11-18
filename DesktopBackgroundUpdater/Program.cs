using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


/*Tested on Windows 10 Vers 1803 Build 17134.407*/
namespace DesktopBackgroundUpdater
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            string newImagePath = GetImageFolderFromConfig();

            Wallpaper.ChangeImage(newImagePath, Wallpaper.Style.Stretched);
            Console.WriteLine("Set new image as background");

            Console.ReadLine();
        }

        /*Get FilePath key from app.config 
         Returns randomly selected image from folder*/
        private static string GetImageFolderFromConfig()
        {
            const string FILE_KEY_NAME = "FilePath";
            var appSettings = ConfigurationManager.AppSettings;
            Random rnd = new Random();

            //Check if app settings isn't working
            if (appSettings.Count == 0)
            {
                Console.WriteLine("App settings empty");
                throw new System.IO.FileNotFoundException("Nothing in App Settings file");
            }

            System.IO.DirectoryInfo direct = new System.IO.DirectoryInfo(appSettings[FILE_KEY_NAME]);

            var files = GetFilesByExtensions(direct, ".jpg", ".png");
            int randNum = rnd.Next(1, files.Count());

            var randomFileName = files.ElementAt(randNum);

            Console.WriteLine("Chosen file name : " + randomFileName);

            return appSettings[FILE_KEY_NAME] + "\\" + randomFileName;

            
          
        }

        public static IEnumerable<FileInfo> GetFilesByExtensions( System.IO.DirectoryInfo dir, params string[] extensions)
        {
            if (extensions == null)
                throw new ArgumentNullException("extensions");
            IEnumerable<FileInfo> files = dir.EnumerateFiles();
            return files.Where(f => extensions.Contains(f.Extension));
        }

        private static void PickImage()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                foreach (var path in System.IO.Directory.GetFiles(fbd.SelectedPath))
                {
                    Console.WriteLine("Full path : " + path);
                }
            }
        }


    }
}
