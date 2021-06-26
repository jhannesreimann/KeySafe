using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Management;

namespace KeySafe
{
    public partial class Masterkey : Form
    {
        ConnectUSB connectUSB = new ConnectUSB();
        string message = "How to create a key:\n\n1. Disconnect the USB\n2. Restart KeySafe.exe\n3. Connect the USB";
        List<string> USBLoad = new List<string>();

        public Masterkey()
        {
            InitializeComponent();
        }

        /*
        LoadDevices speichert alle verbundenen USB-Geräte in einer Liste
        und gibt diese aus.
        */
        public List<string> LoadDevices()
        {
            List<string> USBList = new List<string>(); //erstellt die Liste USBList
            USBList.Clear(); //Leert die Liste
            ManagementObjectCollection collection; //erstellt eine Kollektion von Objekten
            using (var finddevice = new ManagementObjectSearcher(@"Select * From Win32_USBHub")) //füllt die Kollektion mit allen USB-Geräten
                collection = finddevice.Get();
            foreach (var device in collection) //Fügt alle Objekte aus der Kollektion zur Liste USBList als String hinzu
                USBList.Add((string)device.GetPropertyValue("DeviceID"));
            return USBList;
        }

        /*
        Die Methode checkKey Überprüft, ob die gespeicherte USB-ID in der Liste enthalten ist
        und gibt gegebenfalls das Programm frei.
        */
        public void checkKey()
        {
            if (Properties.Settings.Default.selection == "a") //Falls das Programm zum ersten mal gestartet wird, wird dieses Window sich nicht schließen.
                return;
            else if (LoadDevices().Contains(Properties.Settings.Default.selection)) //Falls in den geladenen USB-Geräten die gespeicherte USB-ID vorhanden ist, wird das Programm geöffnet.
            {
                Properties.Settings.Default.firstTime = false;
                this.Hide();
                Program.KeySafe.Show();
            }
            else //wenn die USB-ID nicht in der Liste vorhanden isst, wird die Form "ConnectUSB" geöffnet.
            {
                this.Hide();
                connectUSB.Show();
            }
        }

        /*
        Die Methode "Colors" ändert die Farben der Form "Masterkey", je nach Angaben und passt auch die Sprache an.
        */
        public void Colors(Color light, Color normal, Color dark, Color text, Color back)
        {
            topPanel.GradientBottomLeft = normal;
            topPanel.GradientBottomRight = normal;
            topPanel.GradientTopLeft = normal;
            topPanel.GradientTopRight = normal;
            helpBtn.BackColor = normal;
            helpBtn.FlatAppearance.MouseOverBackColor = dark;
            helpBtn.FlatAppearance.MouseDownBackColor = dark;
            mini.FlatAppearance.MouseOverBackColor = dark;
            mini.FlatAppearance.MouseDownBackColor = dark;
            close.FlatAppearance.MouseOverBackColor = dark;
            close.FlatAppearance.MouseDownBackColor = dark;

            if (Properties.Settings.Default.englishB)
            {
                label1.Text = "Please connect an USB," + Environment.NewLine + "which will be used for unlocking the KeySafe.";
                label1.TextAlign = ContentAlignment.MiddleCenter;
                label1.Location = new Point(77, 90);
                message = "How to create a key:\n\n1. Disconnect the USB\n2. Restart KeySafe.exe\n3. Connect the USB";
            }
            else
            {
                label1.Text = "Bitte schließen Sie ein USB-Gerät an," + Environment.NewLine + "welches zum Entriegeln des KeySafes verwendet wird.";
                label1.TextAlign = ContentAlignment.MiddleCenter;
                label1.Location = new Point(77, 90);
                message = "So erstellen Sie einen Schlüssel:\n\n1. Trennen Sie das USB-Gerät\n2. KeySafe.exe neu starten\n3. Verbinden Sie das USB-Gerät";
            }

            this.BackColor = back;
            label1.ForeColor = text;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Ändern der Farben der Form,, je nach gespeicherten Daten
            if (Properties.Settings.Default.DarkMode)
                Colors(Properties.Settings.Default.colorLight, Properties.Settings.Default.color, Properties.Settings.Default.colorDark, Color.FromArgb(247, 247, 247), Color.FromArgb(42, 46, 50));
            else
                Colors(Properties.Settings.Default.colorLight, Properties.Settings.Default.color, Properties.Settings.Default.colorDark, SystemColors.ControlText, Color.White);

            checkKey(); //Überprüft, ob das USB-Gerät eingeführt wurde.
            USBLoad = LoadDevices(); //Speichert alle beim Start vorhandenen USB-IDs
            timer1.Start(); //Startet den Timer für die Überprüfung der USB-Geräte
        }

        /*
        Bei Klick auf den Button "close" wird die Anwendung geschlossen.
        */
        private void close_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        /*
        Bei Klick auf den Button "mini" wird die Anwendung minimiert.
        */
        private void mini_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        /*
        Gleiches wie bei dem Laden der Form, nur für den Fall eines Zurücksetzens des USB-Geräts.
        */
        private void Masterkey_Shown(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.DarkMode)
                Colors(Properties.Settings.Default.colorLight, Properties.Settings.Default.color, Properties.Settings.Default.colorDark, Color.FromArgb(247, 247, 247), Color.FromArgb(42, 46, 50));
            else
                Colors(Properties.Settings.Default.colorLight, Properties.Settings.Default.color, Properties.Settings.Default.colorDark, SystemColors.ControlText, Color.White);
            checkKey();
            USBLoad = LoadDevices();
            timer1.Start();
        }

        /*
        Bei jedem Tick des Timers "timer1" wird auf ein neu eingeführtes USB-Gerät überprüft, 
        welches anschließend als Key gespeichert wird und zum Entsperren des Programms benutzt wird.
        Die Form "KeySafe" zeigt sich.
        */
        private void timer1_Tick(object sender, EventArgs e)
        {
            List<string> USB = LoadDevices().Except(USBLoad).ToList(); //Speichert die Schnittmenge von den aktuellen Geräten mit den alten Geräte in einer Liste.
            foreach (string a in USB)
            {
                Properties.Settings.Default.selection = a; //Speichert die eingeführte USB-ID in dem Programm.
                Properties.Settings.Default.firstTime = false;
                Properties.Settings.Default.Save();
                this.Hide();
                Program.KeySafe.Show();
                timer1.Stop();
            }
        }

        /*
        Bei Klick auf den Button "helpBtn" wird eine kurze Anleitung der Registration für ein USB-Gerät in einer MessageBox angezeigt.
        */
        private void helpBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show(message);
        }
    }
}
