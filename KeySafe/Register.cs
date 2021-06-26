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
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        /*
        Die Methode "style" ändert die Farben der Form "Register", je nach Angaben.
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
                label6.ForeColor = text;
                label4.ForeColor = text;
                usernametxt.BackColor = SystemColors.ControlText;
                passwordtxt.BackColor = SystemColors.ControlText;
                confirmtxt.BackColor = SystemColors.ControlText;
                hidePasswordBtn1.BackColor = SystemColors.ControlText;
                hidePasswordBtn2.BackColor = SystemColors.ControlText;
                usernametxt.ForeColor = text;
                passwordtxt.ForeColor = text;
                confirmtxt.ForeColor = text;
            }
            else
            {
                label2.ForeColor = Color.FromArgb(164, 165, 169);
                label3.ForeColor = Color.FromArgb(164, 165, 169);
                label6.ForeColor = Color.FromArgb(164, 165, 169);
                label4.ForeColor = Color.FromArgb(164, 165, 169);
                usernametxt.BackColor = Color.FromArgb(230, 231, 233);
                passwordtxt.BackColor = Color.FromArgb(230, 231, 233);
                confirmtxt.BackColor = Color.FromArgb(230, 231, 233);
                hidePasswordBtn1.BackColor = Color.FromArgb(230, 231, 233);
                hidePasswordBtn2.BackColor = Color.FromArgb(230, 231, 233);
                usernametxt.ForeColor = text;
                passwordtxt.ForeColor = text;
                confirmtxt.ForeColor = text;
            }
            loginBtn.ForeColor = normal;
            label1.ForeColor = normal;
            registerBtn.BackColor = normal;
            registerBtn.FlatAppearance.MouseOverBackColor = dark;
            registerBtn.FlatAppearance.MouseDownBackColor = dark;
        }

        /*
        Die Methode "addUser" fügt in der SQL-Datenbank den angegebenen Nutzernamen mit dem Passwort ein.
        */
        public void addUser(string username, string password)
        {
            string connString = "server=server;user=user;password=password;database=database;Connect Timeout=100";
            string sql = "INSERT INTO `user`(`username`, `password`) VALUES (\"" + username + "\",\"" + password + "\")";
            using (var connection = new MySqlConnection(connString))
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception er)
                {
                    MessageBox.Show("Connection Error:" + Environment.NewLine + er.Message, "Information");
                }
            }
        }

        /*
        Die Methode "removeUser" löscht in der SQL-Datenbank den angegebenen Nutzernamen mit dem zugehörigen Passwort.
        */
        public void removeUser(string username, string password)
        {
            string connString = "server=server;user=user;password=password;database=database;Connect Timeout=100";
            string sql = "DELETE FROM `user` WHERE `username`=\"" + username + "\" AND `password`=\"" + password + "\"";
            using (var connection = new MySqlConnection(connString))
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception er)
                {
                    MessageBox.Show("Connection Error:" + Environment.NewLine + er.Message, "Information");
                }
            }
        }

        /*
        Die Methode "getColumn" gibt eine Liste mit allen in der SQL-Datenbank vorhandenen Daten aus einer angegebenen Spalte zurück.
        */
        public List<string> getColumnSQL(string column)
        {
            List<string> list = new List<string>();
            string connString = "server=server;user=user;password=password;database=database;Connect Timeout=100";
            string sql = "SELECT `" + column + "` FROM `user`";
            using (var connection = new MySqlConnection(connString))
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    DataTable dt = new DataTable();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    list = dt.AsEnumerable().Select(r => r.Field<string>(column)).ToList();
                }
                catch(Exception er)
                {
                    MessageBox.Show("Connection Error:" + Environment.NewLine + er.Message, "Information");
                }
            }
            return list;
        }

        /*
        Die Methode "checkPassword" gibt einen Boolean zurück mit dem jeweiligen Wert, ob das angegebene Passwort zu dem angegebenen Nutzernamen in der SQL-Datenbank passt.
        */
        public bool checkPassword(string username, string password)
        {
            string connString = "server=server;user=user;password=password;database=database;Connect Timeout=100";
            string sql = "SELECT `password` FROM `user` WHERE `username`=\"" + username + "\"";
            string passwordDB = "";
            if (getColumnSQL("username").Contains(username))
            {
                using (var connection = new MySqlConnection(connString))
                {
                    try
                    {
                        connection.Open();
                        MySqlCommand cmd = new MySqlCommand(sql, connection);
                        MySqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                        while (reader.Read())
                            passwordDB = reader.GetString(0);
                    }
                    catch (Exception er)
                    {
                        MessageBox.Show("Connection Error:" + Environment.NewLine + er.Message, "Information");
                    }
                    
                }
                return (passwordDB == password);
            }
            else
                return false;
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
        Bei Klick auf den Button "registerBtn" wird die Registration mit den eingegebenen Daten durchgeführt.
        Wenn Fehler auftreten, werden zusätzlich Fehlermeldungen angezeigt.
        Nach Abschluss der Registration wird zur Form "Login" gewechselt.
        */
        private void registerBtn_Click(object sender, EventArgs e)
        {
            if(usernametxt.Text == "" || passwordtxt.Text == "" || confirmtxt.Text == "")
            {
                MessageBox.Show("You have to enter all data!");
            }
            else if (passwordtxt.Text != confirmtxt.Text)
            {
                MessageBox.Show("Passwords do not match!");
            }
            else if (Program.KeySafe.passStrength(passwordtxt.Text) < 2)
            {
                MessageBox.Show("Password is too weak!");
            }
            else if (getColumnSQL("username").Contains(usernametxt.Text))
            {
                MessageBox.Show("Username already taken!");
            }
            else if (Properties.Settings.Default.registered)
            {
                MessageBox.Show("You have already registered an Account!");
            }
            else
            {
                addUser(usernametxt.Text, passwordtxt.Text);
                Properties.Settings.Default.registered = true;
                Properties.Settings.Default.username = usernametxt.Text;
                Properties.Settings.Default.Save();
                Program.Login.usernametxt.Text = Properties.Settings.Default.username;
                usernametxt.Text = "";
                passwordtxt.Text = "";
                confirmtxt.Text = "";
                Program.Login.Show();
                this.Hide();
            }
        }

        /*
        Bei Klick auf den Button "loginBtn" wird die "Login" Form angezeigt und die "Register" Form beendet.
        */
        private void loginBtn_Click(object sender, EventArgs e)
        {
            usernametxt.Text = "";
            passwordtxt.Text = "";
            confirmtxt.Text = "";
            Program.Login.Show();
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
                passwordtxt.isPassword = false;

                if (Properties.Settings.Default.DarkMode)
                    hidePasswordBtn1.Image = Properties.Resources.hidePasswordW;
                else
                    hidePasswordBtn1.Image = Properties.Resources.hidePassword;
                
                show1 = true;
            }
            else
            {
                passwordtxt.isPassword = true;

                if (Properties.Settings.Default.DarkMode)
                    hidePasswordBtn1.Image = Properties.Resources.showPasswordW;
                else
                    hidePasswordBtn1.Image = Properties.Resources.showPassword;

                show1 = false;
            }
        }

        /*
        Bei Klick auf den Button "hidePasswordBtn2" wird das angegebene Passwort entweder versteckt, oder angezeigt.
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
        Bei Änderung des Textfeldes "passwordtxt" wird die Methode "applyStrength" ausgeführt, damit die Passwortstärke angezeigt wird.
        */
        private void passwordtxt_OnValueChanged(object sender, EventArgs e)
        {
            Program.KeySafe.applyStrength(passwordtxt.Text, passStrengthP1);
        }

        /*
        Bei Änderung des Textfeldes "passwordtxt" wird die Methode "applyStrength" ausgeführt, damit die Passwortstärke angezeigt wird.
        */
        private void confirmtxt_OnValueChanged(object sender, EventArgs e)
        {
            Program.KeySafe.applyStrength(confirmtxt.Text, passStrengthP2);
        }

        /*
        Hiermit werden Anpassungen zu dem Darkmode vorgenommen.
        */
        private void Register_Paint(object sender, PaintEventArgs e)
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
