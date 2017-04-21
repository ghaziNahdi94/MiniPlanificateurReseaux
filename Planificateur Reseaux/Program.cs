using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Planificateur_Reseaux
{
    static class Program
    {

        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags,IntPtr dwItem1,IntPtr dwItem2);


        public static bool Registary { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(String[] args)
        {


            if(!isAssociated())
               associate();
           

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length == 0)
                Application.Run(new Form1());
            else
                Application.Run(new Form1(args[0]));


        }




        public static bool isAssociated() {

            return (Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts\.rt", false) == null);
                 }


        public static void associate() {

            RegistryKey fileReg = Registry.CurrentUser.CreateSubKey(@"Software\Classes\.rt") ;
            RegistryKey appReg = Registry.CurrentUser.CreateSubKey(@"Software\Classes\Applications\Planificateur Reseaux.exe");
            RegistryKey appAssoc = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts\.rt");


            fileReg.CreateSubKey("DefaultIcon").SetValue("",Application.StartupPath + "\\Resources\\images.ico");
           


            appReg.CreateSubKey(@"shell\open\command").SetValue("","\""+Application.ExecutablePath+ "\" %1") ;
            appReg.CreateSubKey(@"shell\edit\command").SetValue("", "\"" + Application.ExecutablePath + "\" %1");
            appReg.CreateSubKey("DefaultIcon").SetValue("", Application.StartupPath + "\\Resources\\images.ico");

            appAssoc.CreateSubKey("UserChoice").SetValue("Progid","Planificateur Reseaux.exe");
            appAssoc.CreateSubKey("OpenWithList").SetValue("a", "Planificateur Reseaux.exe");

            SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);

        }


    }
}
