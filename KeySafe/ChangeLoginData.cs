using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySqlConnector;

namespace KeySafe
{
    public partial class ChangeLoginData : Form
    {
        public bool username; //Boolean zur bestimmung, ob ein Nutzername, oder Password geändert werden soll
        public ChangeLoginData()
        {
            InitializeComponent();
        }

        /*
        Die Methode "style" ändert die Farben der Form "ChangeLoginData", je nach Angaben.
        */
        public void style(Color light, Color normal, Color dark, Color text, Color back)
        {
            bunifuGradientPanel1.GradientBottomLeft = normal;
            bunifuGradientPanel1.GradientBottomRight = normal;
            bunifuGradientPanel1.GradientTopLeft = normal;
            bunifuGradientPanel1.GradientTopRight = normal;
            mini.FlatAppearance.MouseOverBackColor = dark;
            mini.FlatAppearance.MouseDownBackColor = dark;
            close.FlatAppearance.MouseOverBackColor = dark;
            close.FlatAppearance.MouseDownBackColor = dark;
            this.BackColor = back;
            if (Properties.Settings.Default.DarkMode)
            {
                label3.ForeColor = text;
                label6.ForeColor = text;
                datatxt.BackColor = SystemColors.ControlText;
                confirmtxt.BackColor = SystemColors.ControlText;
                hidePasswordBtn1.BackColor = SystemColors.ControlText;
                hidePasswordBtn2.BackColor = SystemColors.ControlText;
                datatxt.ForeColor = text;
                confirmtxt.ForeColor = text;
            }
            else
            {
                label3.ForeColor = Color.FromArgb(164, 165, 169);
                label6.ForeColor = Color.FromArgb(164, 165, 169);
                datatxt.BackColor = Color.FromArgb(230, 231, 233);
                confirmtxt.BackColor = Color.FromArgb(230, 231, 233);
                hidePasswordBtn1.BackColor = Color.FromArgb(230, 231, 233);
                hidePasswordBtn2.BackColor = Color.FromArgb(230, 231, 233);
            }
            label1.ForeColor = normal;
            changeBtn.BackColor = normal;
            changeBtn.FlatAppearance.MouseOverBackColor = dark;
            changeBtn.FlatAppearance.MouseDownBackColor = dark;
        }

        /*
        Die Methode "changeUsername" ändert in der SQL-Datenbank den angegebenen Nutzernamen mit der angegebenen Erneuerung.
        */
        public void changeUsername(string oldusername, string newusername)
        {
            string connString = "server=server;user=user;password=password;database=database;Connect Timeout=100";
            string sql = "UPDATE `user` SET `username`=\"" + newusername + "\" WHERE `username`=\"" + oldusername + "\"";
            using (var connection = new MySqlConnection(connString))
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    cmd.ExecuteNonQuery();
                    Properties.Settings.Default.username = newusername;
                    Properties.Settings.Default.Save();
                }
                catch (Exception er)
                {
                    MessageBox.Show("Connection Error ! " + er.Message, "Information");
                }
            }
        }

        /*
        Die Methode "changePassword" ändert in der SQL-Datenbank das Passwort mit angegebenen Usernamen zu dem Wunschpasswort.
        */
        public void changePassword(string username, string newpassword)
        {
            string connString = "server=server;user=user;password=password;database=database;Connect Timeout=100";
            string sql = "UPDATE `user` SET `password`=\"" + newpassword + "\" WHERE `username`=\"" + username + "\"";
            using (var connection = new MySqlConnection(connString))
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    cmd.ExecuteNonQuery();
                    if (Properties.Settings.Default.stayLoggedIn)
                    {
                        Properties.Settings.Default.password = newpassword;
                        Properties.Settings.Default.Save();
                    }
                }
                catch (Exception er)
                {
                    MessageBox.Show("Connection Error ! " + er.Message, "Information");
                }
            }
        }

        /*
        Bei Klick auf den Button "mini" wird die Anwendung minimiert.
        */
        private void mini_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        /*
        Bei Klick auf den Button "close" wird die Anwendung geschlossen.
        */
        private void close_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        /*
        Bei Klick auf den Button "hidePasswordBtn1" wird das angegebene Passwort entweder versteckt, oder angezeigt.
        */
        bool show1 = false;
        private void hidePasswordBtn1_Click(object sender, EventArgs e)
        {
            if (!show1)
            {
                datatxt.isPassword = false;

                if (Properties.Settings.Default.DarkMode)
                    hidePasswordBtn1.Image = Properties.Resources.hidePasswordW;
                else
                    hidePasswordBtn1.Image = Properties.Resources.hidePassword;

                show1 = true;
            }
            else
            {
                datatxt.isPassword = true;

                if (Properties.Settings.Default.DarkMode)
                    hidePasswordBtn1.Image = Properties.Resources.showPasswordW;
                else
                    hidePasswordBtn1.Image = Properties.Resources.showPassword;

                show1 = false;
            }
        }

        /*
        Bei Klick auf den Button "hidePasswordBtn2" wird das Passwort zur Bestätigung entweder versteckt, oder angezeigt.
        */
        bool show2 = false;
        private void hidePasswordBtn2_Click(object sender, EventArgs e)
        {
            if (!show2)
            {
                confirmtxt.isPassword = false;

                if (Properties.Settings.Default.DarkMode)
                    hidePasswordBtn2.Image = Properties.Resources.hidePasswordW;
                else
                    hidePasswordBtn2.Image = Properties.Resources.hidePassword;

                show2 = true;
            }
            else
            {
                confirmtxt.isPassword = true;

                if (Properties.Settings.Default.DarkMode)
                    hidePasswordBtn2.Image = Properties.Resources.showPasswordW;
                else
                    hidePasswordBtn2.Image = Properties.Resources.showPassword;

                show2 = false;
            }
        }

        /*
        Bei Klick auf den Button "changeBtn" wird je nachdem, ob ein Nutzername, oder Passwort geändert werden soll, dies durchgeführt.
        Zusätzlich werden Fehlermeldungen angezeigt, falls z.B. die Eingabe leer ist, oder der Nutzername bereits exisitert.
        */
        private void changeBtn_Click(object sender, EventArgs e)
        {
            if (username)
            {
                if (datatxt.Text == "" || confirmtxt.Text == "")
                {
                    MessageBox.Show("You have to enter all data!");
                }
                else if (datatxt.Text != confirmtxt.Text)
                {
                    MessageBox.Show("Confirmation doesn't match!");
                }
                else if (datatxt.Text == Properties.Settings.Default.username)
                {
                    MessageBox.Show("Enter a different Username!");
                }
                else if ((Properties.Settings.Default.username != datatxt.Text) && Program.Register.getColumnSQL("username").Contains(datatxt.Text))
                {
                    MessageBox.Show("Username already taken!");
                }
                else
                {
                    if (MessageBox.Show("Are you sure you want to\nchange the Username to: " + datatxt.Text + "?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        changeUsername(Properties.Settings.Default.username, datatxt.Text);
                    }
                }
            }
            else
            {
                if (datatxt.Text == "" || confirmtxt.Text == "")
                {
                    MessageBox.Show("You have to enter all data!");
                }
                else if (datatxt.Text != confirmtxt.Text)
                {
                    MessageBox.Show("Confirmation doesn't match!");
                }
                else if (Program.Register.checkPassword(Properties.Settings.Default.username, confirmtxt.Text))
                {
                    MessageBox.Show("Enter a different Password!");
                }
                else
                {
                    if (MessageBox.Show("Are you sure you want to\nchange the Password to: " + datatxt.Text + "?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        changePassword(Properties.Settings.Default.username, datatxt.Text);
                    }
                }
            }
        }

        /*
        Beim Hovern über dem Button "hidePasswordBtn1" wird die Farbe angepasst.
        */
        private void hidePasswordBtn1_MouseHover(object sender, EventArgs e)
        {
            if (!Properties.Settings.Default.DarkMode)
            {
                hidePasswordBtn1.BackColor = SystemColors.Control;
            }
            else
            {
                hidePasswordBtn1.BackColor = ControlPaint.Light(SystemColors.ControlText);
            }
        }

        /*
        Beim Verlassen des Buttons "hidePasswordBtn1" wird die Farbe angepasst.
        */
        private void hidePasswordBtn1_MouseLeave(object sender, EventArgs e)
        {
            if (!Properties.Settings.Default.DarkMode)
            {
                hidePasswordBtn1.BackColor = Color.FromArgb(230, 231, 233);
            }
            else
            {
                hidePasswordBtn1.BackColor = SystemColors.ControlText;
            }
        }

        /*
        Beim Hovern über dem Button "hidePasswordBtn2" wird die Farbe angepasst.
        */
        private void hidePasswordBtn2_MouseHover(object sender, EventArgs e)
        {
            if (!Properties.Settings.Default.DarkMode)
            {
                hidePasswordBtn2.BackColor = SystemColors.Control;
            }
            else
            {
                hidePasswordBtn2.BackColor = ControlPaint.Light(SystemColors.ControlText);
            }
        }

        /*
        Beim Verlassen des Buttons "hidePasswordBtn2" wird die Farbe angepasst.
        */
        private void hidePasswordBtn2_MouseLeave(object sender, EventArgs e)
        {
            if (!Properties.Settings.Default.DarkMode)
            {
                hidePasswordBtn2.BackColor = Color.FromArgb(230, 231, 233);
            }
            else
            {
                hidePasswordBtn2.BackColor = SystemColors.ControlText;
            }
        }

        /*
        Hiermit werden Anpassungen zu dem Darkmode vorgenommen.
        */
        private void ChangeLoginData_Paint(object sender, PaintEventArgs e)
        {
            if (Properties.Settings.Default.DarkMode)
                style(Properties.Settings.Default.colorLight, Properties.Settings.Default.color, Properties.Settings.Default.colorDark, Color.FromArgb(247, 247, 247), Color.FromArgb(42, 46, 50));
            else
                style(Properties.Settings.Default.colorLight, Properties.Settings.Default.color, Properties.Settings.Default.colorDark, SystemColors.ControlText, Color.White);

            if (Properties.Settings.Default.DarkMode)
            {
                hidePasswordBtn1.Image = Properties.Resources.hidePasswordW;
                hidePasswordBtn2.Image = Properties.Resources.hidePasswordW;
            }
            else
            {
                hidePasswordBtn1.Image = Properties.Resources.hidePassword;
                hidePasswordBtn2.Image = Properties.Resources.hidePassword;
            }
        }
    }
}
