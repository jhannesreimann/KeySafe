using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Matrixsoft.PwnedPasswords;

namespace KeySafe
{
    public partial class PasswordItem : UserControl
    {
        public PasswordItem()
        {
            InitializeComponent();
        }
        #region Properties
        private string _Index;
        private string _website;
        private string _username;
        private string _email;
        private string _password;

        // get und set von den Eigenschaften des Items
        [Category("Custom Props")]
        public string Index
        {
            get { return _Index; }
            set { _Index = value; label1.Text = (Convert.ToInt32(value) + 1).ToString(); } // Setzt den Index + 1 des Items als Text des labels, da dadurch die Nummer des Items angezeigt wird.
        }

        [Category("Custom Props")]
        public string Website
        {
            get { return _website; }
            set 
            { 
                _website = value; website.Text = value;

                // Lädt das Logo aus der URL zusammengesetzt aus der angegebenen Website und dem Standartpfad eines Logos einer Website, falls vorhanden.
                try
                {
                    logo.Load("http://" + _website + "/favicon.ico");
                }
                catch (Exception) {}
            }
        }

        [Category("Custom Props")]
        public string Username
        {
            get { return _username; }
            set { _username = value; username.Text = value; }
        }

        [Category("Custom Props")]
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        [Category("Custom Props")]
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        #endregion

        private void close_Click(object sender, EventArgs e)
        {
            this.Dispose(); // Entfernt das ausgewählte Item von dem flowLayoutPanel
            //Program.KeySafe.flowLayoutPanel1.Controls.RemoveAt(Convert.ToInt32(this.Name));
            Program.KeySafe.Entry.RemoveAt(Convert.ToInt32(this.Name)); // Entfernt das ausgewählte Item aus der Liste Entry, durch den Namen (Index)
            Program.SaveKey._Index --; // Durch das entfernen eines Items, muss der allgemeine Index für das nächste Item - 1 gerechnet werden.

            /* 
            Für jedes Item aus der Liste Entry wird, falls dessen Index in der Liste nicht seinem Namen entspricht, 
            der Name gleich dem Index des Items in der Liste gesetzt.
            Dieser Algorithmus überschreibt damit alle Indexe, nach dem gelöschten Item.
            */
            foreach (Entry item in Program.KeySafe.Entry)
            {
                if (Program.KeySafe.Entry.IndexOf(item) != Convert.ToInt32(item.Name)) 
                {
                    item.Name = Program.KeySafe.Entry.IndexOf(item).ToString();
                }
            }

            /* 
            Für jedes Item aus dem flowLayoutPanel wird, falls dessen Index in der Liste nicht seinem Namen entspricht, 
            der Name und der Index gleich dem Index des Items in dem flowLayoutPanel gesetzt. 
            Dieser Algorithmus überschreibt damit alle Indexe, nach dem gelöschten Item.
            */
            foreach (PasswordItem item in Program.KeySafe.flowLayoutPanel1.Controls)
            {
                if(Program.KeySafe.flowLayoutPanel1.Controls.IndexOf(item) != Convert.ToInt32(item.Name))
                {
                    item.Name = Program.KeySafe.flowLayoutPanel1.Controls.IndexOf(item).ToString();
                    item.Index = item.Name;
                }
            }

            // Alle Accounts aus der Liste "Entry" werden als JSON-Datei gespeichert.
            File.WriteAllText(@"accounts.json", JsonConvert.SerializeObject(Program.KeySafe.Entry));

            Program.KeySafe.LoadUnsafePasswords();
            Program.KeySafe.LoadSafetyStats();
            Program.KeySafe.LoadPasswordAmount();
            Program.KeySafe.LoadLeakStats();
            Program.KeySafe.LoadLeakedPasswords();
        }

        /*
        Bei Klick auf den Button "editBtn" wird die Form SaveKey geöffnet mit den Daten des PasswordItems.
        */
        private void editBtn_Click(object sender, EventArgs e)
        {   
            Program.SaveKey.change = true;
            Program.SaveKey.changeIndex = this.Name;
            
            Program.SaveKey.WebsiteTxt.Text = this.Website;
            Program.SaveKey.UsernameTxt.Text = this.Username;
            Program.SaveKey.EmailTxt.Text = this.Email;
            Program.SaveKey.PasswordTxt.Text = this.Password;
            Program.SaveKey.Show();
        }

        /*
        Anpassen der Farben und auch Anzeigen der Darknet-Aktivität mithilfe das zeigen/ausblenden des Panels "pwnedPanel".
        */
        private async void PasswordItem_Paint(object sender, PaintEventArgs e)
        {
            label1.Visible = Properties.Settings.Default.ShowIndex;
            if (Properties.Settings.Default.DarkMode)
            {
                this.BackColor = Color.FromArgb(33, 37, 41);
                website.ForeColor = Color.FromArgb(247, 247, 247);
                username.ForeColor = Color.FromArgb(247, 247, 247);
                label1.ForeColor = Color.FromArgb(247, 247, 247);
                editBtn.FlatAppearance.MouseOverBackColor = Color.FromArgb(27, 30, 33);
                editBtn.FlatAppearance.MouseDownBackColor = Color.FromArgb(27, 30, 33);
                editBtn.ForeColor = Color.FromArgb(33, 37, 41);
                close.FlatAppearance.MouseOverBackColor = Color.FromArgb(27, 30, 33);
                close.FlatAppearance.MouseDownBackColor = Color.FromArgb(27, 30, 33);
            }
            else
            {
                this.BackColor = Color.WhiteSmoke;
                website.ForeColor = SystemColors.ControlText;
                username.ForeColor = SystemColors.ControlText;
                label1.ForeColor = SystemColors.ControlText;
                editBtn.FlatAppearance.MouseOverBackColor = SystemColors.ControlLight;
                editBtn.FlatAppearance.MouseDownBackColor = SystemColors.ControlLight;
                editBtn.ForeColor = Color.WhiteSmoke;
                close.FlatAppearance.MouseOverBackColor = SystemColors.ControlLight;
                close.FlatAppearance.MouseDownBackColor = SystemColors.ControlLight;
            }

            if (Program.KeySafe.checkInternet())
            {
                var client = new PwnedPasswordsClient();
                bool flag = await client.IsPasswordPwnedAsync(Password);
                if (flag)
                {
                    pwnedPanel.Visible = true;
                }
                else
                {
                    pwnedPanel.Visible = false;
                }
            }
            else
            {
                pwnedPanel.Visible = false;
            }
            
        }

        /*
        Bei Klick auf den Button "website" wird die jeweilig gespeicherte Website geöffnet.
        */
        private void website_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://" + this.Website);
        }

        /*
        Bei Klick auf das Logo wird die jeweilig gespeicherte Website geöffnet.
        */
        private void logo_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://" + this.Website);
        }
    }
}
