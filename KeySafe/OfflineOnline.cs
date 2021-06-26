using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeySafe
{
    public partial class OfflineOnline : Form
    {
        public OfflineOnline()
        {
            InitializeComponent();
        }

        /*
        Die Methode "style" ändert die Farben der Form "OfflineOnline", je nach Angaben.
        */
        public void style(Color light, Color normal, Color dark, Color text, Color back)
        {
            bunifuGradientPanel1.GradientBottomLeft = normal;
            bunifuGradientPanel1.GradientBottomRight = normal;
            bunifuGradientPanel1.GradientTopLeft = normal;
            bunifuGradientPanel1.GradientTopRight = normal;
            mini.BackColor = normal;
            mini.FlatAppearance.MouseOverBackColor = dark;
            mini.FlatAppearance.MouseDownBackColor = dark;
            close.BackColor = normal;
            close.FlatAppearance.MouseOverBackColor = dark;
            close.FlatAppearance.MouseDownBackColor = dark;
            this.BackColor = back;
            onlineBtn.BackColor = back;
            offlineBtn.BackColor = back;
            label1.ForeColor = normal;
            if (Properties.Settings.Default.DarkMode)
            {
                label2.ForeColor = text;
                label3.ForeColor = text;
                onlineBtn.Image = Properties.Resources.onlineLogoW;
                offlineBtn.Image = Properties.Resources.onlineLogoW;
            }
            else
            {
                onlineBtn.Image = Properties.Resources.onlineLogo;
                offlineBtn.Image = Properties.Resources.onlineLogo;
            }
        }

        /*
        Beim Laden der Form "OfflineOnline" werden Änderungen zum Darkmode und zu den ToolTips vorgenommen.
        */
        private void OfflineOnline_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.DarkMode)
                style(Properties.Settings.Default.colorLight, Properties.Settings.Default.color, Properties.Settings.Default.colorDark, Color.FromArgb(247, 247, 247), Color.FromArgb(42, 46, 50));
            else
                style(Properties.Settings.Default.colorLight, Properties.Settings.Default.color, Properties.Settings.Default.colorDark, SystemColors.ControlText, Color.White);

            onlineToolTip.SetToolTip(onlineBtn, "Login with an Username and Password.");
            offlineToolTip.SetToolTip(offlineBtn, "Login with an USB-Device.");
        }

        /*
        Beim Klick auf den Button "onlineBtn" wird die Einstellung "onlineLogin" gespeichert und die "Register" Form angezeigt und die jetzige Form geschlossen.
        */
        private void onlineBtn_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.onlineLogin = true;
            Properties.Settings.Default.Save();
            Program.Register.Show();
            this.Hide();
        }

        /*
        Beim Klick auf den Button "onlineBtn" wird die Einstellung "offlineLogin" gespeichert und die "MasterKey" Form angezeigt und die jetzige Form geschlossen.
        */
        private void offlineBtn_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.onlineLogin = false;
            Properties.Settings.Default.Save();
            Program.MasterKey.Show();
            this.Hide();
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
        Beim Verlassen des Buttons "offlineBtn" wird die Farbe angepasst.
        */
        private void offlineBtn_MouseLeave(object sender, EventArgs e)
        {
                offlineBtn.BackColor = Color.Transparent;
        }

        /*
        Beim Verlassen des Buttons "onlineBtn" wird die Farbe angepasst.
        */
        private void onlineBtn_MouseLeave(object sender, EventArgs e)
        {
                onlineBtn.BackColor = Color.Transparent;
        }

        /*
        Beim Hovern über dem Button "onlineBtn" wird die Farbe angepasst.
        */
        private void onlineBtn_MouseHover(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.DarkMode)
                onlineBtn.BackColor = Color.FromArgb(36, 39, 41);
            else
                onlineBtn.BackColor = SystemColors.Control;
        }

        /*
        Beim Hovern über dem Button "offlineBtn" wird die Farbe angepasst.
        */
        private void offlineBtn_MouseHover(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.DarkMode)
                offlineBtn.BackColor = Color.FromArgb(36, 39, 41);
            else
                offlineBtn.BackColor = SystemColors.Control;
        }

        /*
        Beim Anzeigen der Form "OfflineOnline" wird, falls man das Programm nicht zum ersten mal startet, der Login-Screen angezeigt, oder beim offline-Login die "Masterkey" Form.
        */
        private void OfflineOnline_Shown(object sender, EventArgs e)
        {
            if (!Properties.Settings.Default.firstTime)
            {
                
                if (Properties.Settings.Default.onlineLogin)
                {
                    Program.Login.Show();
                }
                else
                {
                    Program.MasterKey.Show();
                }
                this.Hide();
            }
        }

        /*
        Hiermit werden Anpassungen zu dem Darkmode vorgenommen.
        */
        private void OfflineOnline_Paint(object sender, PaintEventArgs e)
        {
            if (Properties.Settings.Default.DarkMode)
                style(Properties.Settings.Default.colorLight, Properties.Settings.Default.color, Properties.Settings.Default.colorDark, Color.FromArgb(247, 247, 247), Color.FromArgb(42, 46, 50));
            else
                style(Properties.Settings.Default.colorLight, Properties.Settings.Default.color, Properties.Settings.Default.colorDark, SystemColors.ControlText, Color.White);
        }
    }
}
