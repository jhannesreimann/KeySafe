using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Runtime.InteropServices;

namespace KeySafe
{
    public partial class ConnectUSB : Form
    {
        List<string> USBList = new List<string>();

        public ConnectUSB()
        {
            InitializeComponent();
        }

        /*
        LoadDevices speichert alle verbundenen USB-Geräte in der Liste USBList.
        */
        public void LoadDevices()
        {
            USBList.Clear();
            ManagementObjectCollection collection;
            using (var finddevice = new ManagementObjectSearcher(@"Select * From Win32_USBHub"))
                collection = finddevice.Get();
            foreach (var device in collection)
                USBList.Add(device.GetPropertyValue("DeviceID").ToString());
        }

        /*
        Die Methode "Colors" ändert die Farben der Form "ConnectUSB", je nach Angaben und passt auch die Sprache an.
        */
        public void Colors(Color light, Color normal, Color dark, Color text, Color back)
        {
            bunifuGradientPanel1.GradientBottomLeft = normal;
            bunifuGradientPanel1.GradientBottomRight = light;
            bunifuGradientPanel1.GradientTopLeft = normal;
            bunifuGradientPanel1.GradientTopRight = light;
            mini.FlatAppearance.MouseOverBackColor = dark;
            mini.FlatAppearance.MouseDownBackColor = dark;
            close.FlatAppearance.MouseOverBackColor = dark;
            close.FlatAppearance.MouseDownBackColor = dark;
            if (Properties.Settings.Default.englishB)
            {
                label1.Text = "CONNECT THE USB";
            }
            else
            {
                label1.Text = "VERBINDE DEN USB";
            }
            label1.ForeColor = text;
            this.BackColor = back;
        }

        /*
        Bei Klick auf den Button "mini" wird die Anwendung minimiert.
        */
        private void mini_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        /*
        Bei Klick auf den Button "close" wird die Anwendung geschlossen und der Timer zur Überprüfung des USB-Geräts gestoppt.
        */
        private void close_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            System.Windows.Forms.Application.Exit();
        }

        /*
        Beim Erscheinen der Form "ConnectUSB" wird die Farbe je nach gespeicherten Werten geändert
        und der Timer zur Überprüfung des USB-Geräts gestartet.
        */
        private void ConnectUSB_Shown(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.DarkMode)
                Colors(Properties.Settings.Default.colorLight, Properties.Settings.Default.color, Properties.Settings.Default.colorDark, Color.FromArgb(247, 247, 247), Color.FromArgb(42, 46, 50));
            else
                Colors(Properties.Settings.Default.colorLight, Properties.Settings.Default.color, Properties.Settings.Default.colorDark, SystemColors.ControlText, Color.White);
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            LoadDevices(); //Speichert die aktuellen USB-Geräte in der Liste USBList.
            if (USBList.Contains(Properties.Settings.Default.selection)) //Falls in der Liste die gespeicherte USB-ID vorhanden ist, wird die Form "KeySafe" geöffnet.
            {
                this.Hide();
                Program.KeySafe.Show();
            }
        }
    }
}
