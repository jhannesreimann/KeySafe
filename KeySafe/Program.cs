using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeySafe
{
    static class Program
    {
        public static KeySafe KeySafe;
        public static SaveKey SaveKey;
        public static Register Register;
        public static Login Login;
        public static Masterkey MasterKey;
        public static OfflineOnline OfflineOnline;
        public static ChangeLoginData ChangeLoginData;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            KeySafe = new KeySafe();
            SaveKey = new SaveKey();
            Register = new Register();
            Login = new Login();
            MasterKey = new Masterkey();
            ChangeLoginData = new ChangeLoginData();
            OfflineOnline = new OfflineOnline();
            Application.Run(new OfflineOnline());
            
    }
    }
}
