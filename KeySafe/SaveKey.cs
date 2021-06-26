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
using System.Text.RegularExpressions;
using System.IO;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace KeySafe
{
    public partial class SaveKey : Form
    {
        public int _Index = 0;
        public bool change = false;
        public string changeIndex;

        public SaveKey()
        {
            InitializeComponent();
        }

        /*
        Die Methode IsValidEmail überprüft einen String auf die richtige Schreibweise einer Email-Adresse
        und gibt je nachdem einen Boolean aus.
        */
        public bool IsValidEmail(string email)
        {
            return new EmailAddressAttribute().IsValid(email);
        }

        /*
        Die Methode IsValidHttpURL überprüft, on ein eingegebener String einer gültigen URL entspricht
        und gibt dementsprechend einen Boolean zurück.
        */
        public static bool IsValidHttpURL(string website)
        {
            if (!Regex.IsMatch(website, @"^https?:\/\/", RegexOptions.IgnoreCase)) // überprüft, ob "http://" oder "https://" am Anfang ist, falls nicht, wird ein "http://" vorne rangehangen
                website = "http://" + website;

            if (Uri.IsWellFormedUriString(website, UriKind.Absolute)) // überprüft, ob die Form einer URL eingehalten wird
                return true;
            else
                return false;
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            if (change) // Falls ein Item editiert werden soll
            {
                foreach (PasswordItem item in Program.KeySafe.flowLayoutPanel1.Controls) // für jedes Item in dem flowLayoutPanel (also alle vorhandenen Passwörter) ...
                {
                    if (item.Name == changeIndex) // ... wird überprüft, ob der Index dieses Items dem Index, welcher editiert werden soll, entspricht. Falls ja werden ...
                    {
                        // ... alle Daten des Items (Website, Username, Email, Password) mit den Daten aus der Benutzeroberfläche SaveKey ersetzt.
                        item.Website = WebsiteTxt.Text;
                        item.Username = UsernameTxt.Text;
                        item.Email = EmailTxt.Text;
                        item.Password = PasswordTxt.Text;

                        // In der Liste Entry an dem Index, welcher bearbeitet werden soll, werden die Daten mit den Daten des Items ersetzt und ...
                        Program.KeySafe.Entry[Convert.ToInt32(changeIndex)] = (new Entry(item.Name, item.Website, item.Username, item.Email, item.Password));

                        // ... anschließend wird die bearbeitete List die json Datei überschreiben.
                        File.WriteAllText(@"accounts.json", JsonConvert.SerializeObject(Program.KeySafe.Entry));

                        this.Hide();
                    }
                }
            }
            else // Wenn ein Item hinzugefügt werden soll ... 
            {
                // ... wird ein neues UserControl passwordItem erstellt, mit den Werten von der Benutzeroberfläche SaveKey.
                PasswordItem passwordItem = new PasswordItem();

                passwordItem.Website = WebsiteTxt.Text;
                passwordItem.Username = UsernameTxt.Text;
                passwordItem.Email = EmailTxt.Text;
                passwordItem.Password = PasswordTxt.Text;

                // Der Name und die Eigenschaft "Index" des Items, soll dem Integer "_Index" entsprechen, damit darüber die Reihenfolge festgelegt wird.
                passwordItem.Name = _Index.ToString();
                passwordItem.Index = _Index.ToString();
                Program.KeySafe.flowLayoutPanel1.Controls.Add(passwordItem);

                // Der Index wird nach dem hinzufügen um 1 erhöht, damit das nächste hinzugefügte Item nicht den gleichen Namen zugeschrieben bekommt.
                _Index++;

                // Der Liste Entry wird dieses Item hinzugefügt, damit sie anschließend die json Datei überschreiben kann.
                Program.KeySafe.Entry.Add(new Entry(passwordItem.Name, passwordItem.Website, passwordItem.Username, passwordItem.Email, passwordItem.Password));
                File.WriteAllText(@"accounts.json", JsonConvert.SerializeObject(Program.KeySafe.Entry));

                // Die Textfelder werden geleert und die Form wird geschlossen.
                WebsiteTxt.Text = "";
                UsernameTxt.Text = "";
                EmailTxt.Text = "";
                PasswordTxt.Text = "";
                this.Hide();
            }

            // Da sich die vorhandenen Accounts verändern müssen auch die Statistiken für den Home-/Darknet-Aktivität-Tab aktualisiert werden
            Program.KeySafe.LoadUnsafePasswords();
            Program.KeySafe.LoadSafetyStats();
            Program.KeySafe.LoadPasswordAmount();
            Program.KeySafe.LoadLeakStats();
            Program.KeySafe.LoadLeakedPasswords();
        }

        /*
        Bei Klick auf den Button "close" wird die Form beendet und die Textfelder geleert.
        */
        private void close_Click(object sender, EventArgs e)
        {
            WebsiteTxt.Text = "";
            UsernameTxt.Text = "";
            EmailTxt.Text = "";
            PasswordTxt.Text = "";
            this.Hide();
        }

        /*
        Bei Klick auf den Button "mini" wird die Form minimiert.
        */
        private void mini_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        /*
        Bei Änderung der Textbox "PasswordTxt" wird die Passwortstärke aktualisiert.
        */
        private void PasswordTxt_OnValueChanged(object sender, EventArgs e)
        {
            Program.KeySafe.applyStrength(PasswordTxt.Text, passStrengthP);
        }

        /*
        Bei Klick auf den Button copyWebsiteBtn wird der Text der Textbox "WebsiteTxt" kopiert.
        */
        private void copyWebsiteBtn_Click(object sender, EventArgs e)
        {
            if (WebsiteTxt.Text != "")
            {
                Clipboard.SetText(WebsiteTxt.Text);
                MessageBox.Show("Copied to Clipboard!");
            }
            else
                MessageBox.Show("No Website found.");
        }

        /*
        Bei Klick auf den Button copyUsernameBtn wird der Text der Textbox "UsernameTxt" kopiert.
        */
        private void copyUsernameBtn_Click(object sender, EventArgs e)
        {
            if (UsernameTxt.Text != "")
            {
                Clipboard.SetText(UsernameTxt.Text);
                MessageBox.Show("Copied to Clipboard!");
            }
            else
                MessageBox.Show("No Username found.");
        }

        /*
        Bei Klick auf den Button copyEmailBtn wird der Text der Textbox "EmailTxt" kopiert.
        */
        private void copyEmailBtn_Click(object sender, EventArgs e)
        {
            if (EmailTxt.Text != "")
            {
                Clipboard.SetText(EmailTxt.Text);
                MessageBox.Show("Copied to Clipboard!");
            }
            else
                MessageBox.Show("No Email found.");
        }

        /*
        Bei Klick auf den Button copypasswordBtn wird der Text der Textbox "PasswordTxt" kopiert.
        */
        private void copyPasswordBtn_Click(object sender, EventArgs e)
        {
            if (PasswordTxt.Text != "")
            {
                Clipboard.SetText(PasswordTxt.Text);
                MessageBox.Show("Copied to Clipboard!");
            }
            else
                MessageBox.Show("No Password found.");
        }

        private void SaveKey_Shown(object sender, EventArgs e)
        {
            timer1.Start();

            if (File.Exists(@"accounts.json")) // Falls eine json Datei existiert (also schon Passwörter gespeichert wurden) ...
            {
                // ... wird eine Liste mit allen Items aus der bestehenden json Datei erstellt.
                List<Entry> itemList = JsonConvert.DeserializeObject<List<Entry>>(File.ReadAllText(@"accounts.json"));

                // Für jedes item in dieser Liste wird dann der Integer "_Index" mit dem Namen des Items + 1 überschrieben, damit man nach dem letzten Index neue Items erstellen kann.
                foreach (Entry item in itemList)
                {
                    _Index = Convert.ToInt32(item.Name) + 1;
                }
            }
            else // Falls keine Datei exisitert (also noch keine Passwörter gespeichert wurden) ist der _Index 0.
            {
                _Index = 0;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // hiermit wird überprüft, ob die Werte richtig eingegeben wurden. Je nachdem wird der Button aktiviert, oder nicht.
            if (WebsiteTxt.Text != "" && PasswordTxt.Text != "" && IsValidEmail(EmailTxt.Text) && IsValidHttpURL(WebsiteTxt.Text))
            {
                addBtn.Enabled = true;
            }
            else
            {
                addBtn.Enabled = false;
            }
        }
    }
}
