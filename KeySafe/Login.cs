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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        /*
        Die Methode "style" ändert die Farben der Form "Login", je nach Angaben.
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
            if (Properties.Settings.Default.DarkMode)
            {
                label2.ForeColor = text;
                label3.ForeColor = text;
                stayLoggedInC.ForeColor = normal;
                label4.ForeColor = text;
                usernametxt.BackColor = SystemColors.ControlText;
                passwordtxt.BackColor = SystemColors.ControlText;
                hidePasswordBtn.BackColor = SystemColors.ControlText;
                usernametxt.ForeColor = text;
                passwordtxt.ForeColor = text;
            }
            else
            {
                label2.ForeColor = Color.FromArgb(164, 165, 169);
                label3.ForeColor = Color.FromArgb(164, 165, 169);
                stayLoggedInC.ForeColor = Color.FromArgb(164, 165, 169);
                label4.ForeColor = Color.FromArgb(164, 165, 169);
                usernametxt.BackColor = Color.FromArgb(230, 231, 233);
                passwordtxt.BackColor = Color.FromArgb(230, 231, 233);
                hidePasswordBtn.BackColor = Color.FromArgb(230, 231, 233);
                usernametxt.ForeColor = SystemColors.WindowText;
                passwordtxt.ForeColor = SystemColors.WindowText;
            }
            stayLoggedInC.FlatAppearance.CheckedBackColor = normal;
            loginBtn.BackColor = normal;
            label1.ForeColor = normal;
            registerBtn.ForeColor = normal;
            loginBtn.FlatAppearance.MouseOverBackColor = dark;
            loginBtn.FlatAppearance.MouseDownBackColor = dark;
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
        Bei Klick auf den Button "loginBtn" wird der Login mit angegebenen Username und Password vollzogen, 
        falls die Daten stimmen und zusätzlich Fehlermeldungen angezeigt.
        */
        private void loginBtn_Click(object sender, EventArgs e)
        {
            if (usernametxt.Text == "" || passwordtxt.Text == "")
            {
                MessageBox.Show("You have to enter all data!");
            }
            else
            {
                if (Program.Register.checkPassword(usernametxt.Text, passwordtxt.Text))
                {
                    Properties.Settings.Default.username = usernametxt.Text;
                    Properties.Settings.Default.firstTime = false;
                    if (Properties.Settings.Default.stayLoggedIn)
                        Properties.Settings.Default.password = passwordtxt.Text;
                    Properties.Settings.Default.Save();
                    usernametxt.Text = "";
                    passwordtxt.Text = "";
                    Program.KeySafe.Show();
                    this.Hide();
                }
                else
                    MessageBox.Show("Username or Password incorrect!");
            }
        }

        /*
        Bei Klick auf den Button "registerBtn" wird die Register Form angezeigt und die Login Form beendet.
        */
        private void registerBtn_Click(object sender, EventArgs e)
        {
            usernametxt.Text = "";
            passwordtxt.Text = "";
            Program.Register.Show();
            this.Hide();
        }

        /*
        Bei Klick auf den Button "hidePasswordBtn1" wird das angegebene Passwort entweder versteckt, oder angezeigt.
        */
        bool show = false;
        private void hidePasswordBtn_Click(object sender, EventArgs e)
        {
            if (!show)
            {
                passwordtxt.isPassword = false;

                if (Properties.Settings.Default.DarkMode)
                    hidePasswordBtn.Image = Properties.Resources.hidePasswordW;
                else
                    hidePasswordBtn.Image = Properties.Resources.hidePassword;

                show = true;
            }
            else
            {
                passwordtxt.isPassword = true;

                if (Properties.Settings.Default.DarkMode)
                    hidePasswordBtn.Image = Properties.Resources.showPasswordW;
                else
                    hidePasswordBtn.Image = Properties.Resources.showPassword;

                show = false;
            }
        }

        /*
        Bei Änderung der Checkbox "stayLoggedInC" wird die Einstellung zu dem "eingeloggt bleiben" geändert.
        */
        private void stayLoggedInC_CheckedChanged(object sender, EventArgs e)
        {
            if (stayLoggedInC.Checked)
            {
                Properties.Settings.Default.stayLoggedIn = true;
            }
            else
            {
                Properties.Settings.Default.stayLoggedIn = false;
            }
        }

        /*
        Beim Hovern über dem Button "hidePasswordBtn" wird die Farbe angepasst.
        */
        private void hidePasswordBtn_MouseHover(object sender, EventArgs e)
        {
            if (!Properties.Settings.Default.DarkMode)
            {
                hidePasswordBtn.BackColor = SystemColors.Control;
            }
            else
            {
                hidePasswordBtn.BackColor = ControlPaint.Light(SystemColors.ControlText);
            }
        }

        /*
        Beim Verlassen des Buttons "hidePasswordBtn" wird die Farbe angepasst.
        */
        private void hidePasswordBtn_MouseLeave(object sender, EventArgs e)
        {
            if (!Properties.Settings.Default.DarkMode)
            {
                hidePasswordBtn.BackColor = Color.FromArgb(230, 231, 233);
            }
            else
            {
                hidePasswordBtn.BackColor = SystemColors.ControlText;
            }
        }

        /*
        Wenn die Form "Login" angezeigt wird, wird versucht sich mit dem gespeicherten Username und Password automatisch einzuloggen, 
        falls die Einstellung zu dem "eingeloggt bleiben" aktiviert wurde.
        */
        private void Login_Shown(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.stayLoggedIn)
            {
                usernametxt.Text = Properties.Settings.Default.username;
                passwordtxt.Text = Properties.Settings.Default.password;
                try
                {
                    if (Program.Register.checkPassword(Properties.Settings.Default.username, Properties.Settings.Default.password))
                    {
                        Properties.Settings.Default.firstTime = false;
                        Properties.Settings.Default.Save();
                        Program.KeySafe.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Username or Password incorrect!");
                    }

                }
                catch (Exception)
                {
                    MessageBox.Show("Error while connnecting to server!");
                }
            }
        }

        /*
        Hiermit werden Anpassungen zu dem Darkmode vorgenommen.
        */
        private void Login_Paint(object sender, PaintEventArgs e)
        {
            if (Properties.Settings.Default.DarkMode)
                style(Properties.Settings.Default.colorLight, Properties.Settings.Default.color, Properties.Settings.Default.colorDark, Color.FromArgb(247, 247, 247), Color.FromArgb(42, 46, 50));
            else
                style(Properties.Settings.Default.colorLight, Properties.Settings.Default.color, Properties.Settings.Default.colorDark, SystemColors.ControlText, Color.White);

            if (Properties.Settings.Default.DarkMode)
            {
                hidePasswordBtn.Image = Properties.Resources.hidePasswordW;
            }
            else
            {
                hidePasswordBtn.Image = Properties.Resources.hidePassword;
            }
        }
    }
}
