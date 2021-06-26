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
using Newtonsoft.Json;
using System.IO;
using System.Security.Cryptography;
using System.Net;
using System.Diagnostics;
using AutoUpdaterDotNET;
using System.Xml;
using System.Globalization;
using Matrixsoft.PwnedPasswords;

namespace KeySafe
{
    public partial class KeySafe : Form
    {
        #region Attributes
        List<string> USBList = new List<string>();
        int passwordLength = 4;
        public List<Entry> Entry = new List<Entry>();
        int r = 255, g = 0, b = 0;
        #endregion

        #region Initialisierung
        public KeySafe()
        {
            InitializeComponent();

            
            // Automatisches Übernehmen der Daten der vorherigen Version nach dem Update.
            
            if (File.Exists(@".\updating.txt"))
            {
                if (File.ReadAllText(@".\updating.txt") == "true")
                {
                    Properties.Settings.Default.selection = File.ReadAllText(@".\selection.txt");
                    Properties.Settings.Default.key = File.ReadAllText(@".\key.txt");
                    Properties.Settings.Default.USBClose = File.ReadAllText(@".\USBClose.txt").Equals("1");
                    Properties.Settings.Default.ShowIndex = File.ReadAllText(@".\ShowIndex.txt").Equals("1");
                    Properties.Settings.Default.color = Color.FromArgb(Convert.ToInt32(File.ReadAllText(@".\color.txt").Split(',')[0]), Convert.ToInt32(File.ReadAllText(@".\color.txt").Split(',')[1]), Convert.ToInt32(File.ReadAllText(@".\color.txt").Split(',')[2]));
                    Properties.Settings.Default.colorDark = Color.FromArgb(Convert.ToInt32(File.ReadAllText(@".\colorDark.txt").Split(',')[0]), Convert.ToInt32(File.ReadAllText(@".\colorDark.txt").Split(',')[1]), Convert.ToInt32(File.ReadAllText(@".\colorDark.txt").Split(',')[2]));
                    Properties.Settings.Default.colorLight = Color.FromArgb(Convert.ToInt32(File.ReadAllText(@".\colorLight.txt").Split(',')[0]), Convert.ToInt32(File.ReadAllText(@".\colorLight.txt").Split(',')[1]), Convert.ToInt32(File.ReadAllText(@".\colorLight.txt").Split(',')[2]));
                    Properties.Settings.Default.resetColor = File.ReadAllText(@".\resetColor.txt").Equals("1");
                    Properties.Settings.Default.RainbowColor = File.ReadAllText(@".\RainbowColor.txt").Equals("1");
                    Properties.Settings.Default.opacity = float.Parse(File.ReadAllText(@".\opacity.txt"), CultureInfo.InvariantCulture.NumberFormat);
                    Properties.Settings.Default.englishB = File.ReadAllText(@".\englishB.txt").Equals("1");
                    Properties.Settings.Default.DarkMode = File.ReadAllText(@".\DarkMode.txt").Equals("1");
                    Properties.Settings.Default.firstTime = File.ReadAllText(@".\firstTime.txt").Equals("1");
                    Properties.Settings.Default.onlineLogin = File.ReadAllText(@".\onlineLogin.txt").Equals("1");
                    Properties.Settings.Default.username = File.ReadAllText(@".\username.txt");
                    Properties.Settings.Default.password = File.ReadAllText(@".\password.txt");
                    Properties.Settings.Default.stayLoggedIn = File.ReadAllText(@".\stayLoggedIn.txt").Equals("1");
                    Properties.Settings.Default.registered = File.ReadAllText(@".\registered.txt").Equals("1");
                    Properties.Settings.Default.Save();
                    File.Delete(@".\selection.txt");
                    File.Delete(@".\key.txt");
                    File.Delete(@".\USBClose.txt");
                    File.Delete(@".\ShowIndex.txt");
                    File.Delete(@".\color.txt");
                    File.Delete(@".\colorDark.txt");
                    File.Delete(@".\colorLight.txt");
                    File.Delete(@".\resetColor.txt");
                    File.Delete(@".\RainbowColor.txt");
                    File.Delete(@".\opacity.txt");
                    File.Delete(@".\englishB.txt");
                    File.Delete(@".\DarkMode.txt");
                    File.Delete(@".\firstTime.txt");
                    File.Delete(@".\onlineLogin.txt");
                    File.Delete(@".\username.txt");
                    File.Delete(@".\password.txt");
                    File.Delete(@".\stayLoggedIn.txt");
                    File.Delete(@".\registered.txt");
                    File.Delete(@".\updating.txt");
                }
            }

            // Speichern der Daten der alten Version und automatisches Updaten auf die neuste Version.

            if (checkInternet())
            {
                try
                {
                    if (getNewestVersion("https://www.dropbox.com/s/9c0th4blf947wz4/KeySafe.xml?dl=1") != System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString())
                    {
                        File.WriteAllText(@".\selection.txt", Properties.Settings.Default.selection);
                        File.WriteAllText(@".\USBClose.txt", boolToString(Properties.Settings.Default.USBClose));
                        File.WriteAllText(@".\ShowIndex.txt", boolToString(Properties.Settings.Default.ShowIndex));
                        File.WriteAllText(@".\key.txt", Properties.Settings.Default.key);
                        File.WriteAllText(@".\color.txt", colorToRGB(Properties.Settings.Default.color));
                        File.WriteAllText(@".\colorDark.txt", colorToRGB(Properties.Settings.Default.colorDark));
                        File.WriteAllText(@".\colorLight.txt", colorToRGB(Properties.Settings.Default.colorLight));
                        File.WriteAllText(@".\resetColor.txt", boolToString(Properties.Settings.Default.resetColor));
                        File.WriteAllText(@".\RainbowColor.txt", boolToString(Properties.Settings.Default.RainbowColor));
                        File.WriteAllText(@".\opacity.txt", Properties.Settings.Default.opacity.ToString("0.00", CultureInfo.InvariantCulture));
                        File.WriteAllText(@".\englishB.txt", boolToString(Properties.Settings.Default.englishB));
                        File.WriteAllText(@".\DarkMode.txt", boolToString(Properties.Settings.Default.DarkMode));
                        File.WriteAllText(@".\firstTime.txt", boolToString(Properties.Settings.Default.firstTime));
                        File.WriteAllText(@".\onlineLogin.txt", boolToString(Properties.Settings.Default.onlineLogin));
                        File.WriteAllText(@".\username.txt", Properties.Settings.Default.username);
                        File.WriteAllText(@".\password.txt", Properties.Settings.Default.password);
                        File.WriteAllText(@".\stayLoggedIn.txt", boolToString(Properties.Settings.Default.stayLoggedIn));
                        File.WriteAllText(@".\registered.txt", boolToString(Properties.Settings.Default.registered));
                        File.WriteAllText(@".\updating.txt", "true");
                        AutoUpdater.Mandatory = true;
                        AutoUpdater.UpdateMode = Mode.ForcedDownload;
                        AutoUpdater.Start("https://www.dropbox.com/s/9c0th4blf947wz4/KeySafe.xml?dl=1");
                    }
                }
                catch (Exception er)
                {
                    MessageBox.Show("Error while Downloading/Checking for the newest Version:" + Environment.NewLine + er.Message, "Information");
                }
                
            }
            
        }
        #endregion

        #region Methods
        /*
        Übberschreiben der CreateParams, um den Programmablauf flüssiger zu machen.
        */
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleparam = base.CreateParams;
                handleparam.ExStyle |= 0x02000000;
                return handleparam;
            }
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
            {
                USBList.Add(device.GetPropertyValue("DeviceID").ToString());
            }
        }

        /*
        Die Methode style legt gewisse Eigenschaften für die Benutzeroberfläche fest,
        welche man mit WinForms nicht umsetzen kann.
        */
        public void style()
        {
            // Lässt die Tabs von den TabControl verschwinden
            tabControl1.Region = new Region(new RectangleF(passwords.Left, passwords.Top, passwords.Width, passwords.Height));
            tabControl1.Appearance = TabAppearance.FlatButtons;
            tabControl1.ItemSize = new Size(0, 1);
            tabControl1.SizeMode = TabSizeMode.Fixed;
            tabControl1.Multiline = true;
            foreach (TabPage tab in tabControl1.TabPages)
                tab.Text = "";

            // Macht aus den 2 Labels "Seperator"
            label3.AutoSize = false;
            label3.Height = 1;
            label4.AutoSize = false;
            label4.Height = 1;
            
            // Lässt die Scrollbars von dem FlowLayoutPanel verschwinden, aber lässt die Eigenschaft des Scrollens bestehen
            flowLayoutPanel1.AutoScroll = false;
            flowLayoutPanel1.HorizontalScroll.Enabled = false;
            flowLayoutPanel1.HorizontalScroll.Visible = false;
            flowLayoutPanel1.HorizontalScroll.Maximum = 0;
            flowLayoutPanel1.VerticalScroll.Enabled = false;
            flowLayoutPanel1.VerticalScroll.Visible = false;
            flowLayoutPanel1.VerticalScroll.Maximum = 0;
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel2.AutoScroll = false;
            flowLayoutPanel2.HorizontalScroll.Enabled = false;
            flowLayoutPanel2.HorizontalScroll.Visible = false;
            flowLayoutPanel2.HorizontalScroll.Maximum = 0;
            flowLayoutPanel2.VerticalScroll.Enabled = false;
            flowLayoutPanel2.VerticalScroll.Visible = false;
            flowLayoutPanel2.VerticalScroll.Maximum = 0;
            flowLayoutPanel2.AutoScroll = true;
            settings.AutoScroll = false;
            settings.HorizontalScroll.Enabled = false;
            settings.HorizontalScroll.Visible = false;
            settings.HorizontalScroll.Maximum = 0;
            settings.VerticalScroll.Enabled = false;
            settings.VerticalScroll.Visible = false;
            settings.VerticalScroll.Maximum = 0;
            settings.AutoScroll = true;

            // Festlegen der Farbe des Seperators bei Wiederherstellung der Standartfarbe.
            if (Properties.Settings.Default.resetColor)
            {
                label3.BackColor = Color.FromArgb(54, 75, 122);
            }

            // Beheben eines Bugs der Progessbars, bei welchem die Prozentzahl nich angezeigt wird.
            safetyAverageP.LabelVisible = false;
            safetyAverageP.LabelVisible = true;
            leakAmountP.LabelVisible = false;
            leakAmountP.LabelVisible = true;

            // Rotieren des Indikators der Dropbox für die Sprachauswahl.
            languageBtn.Image.RotateFlip(RotateFlipType.Rotate270FlipXY);

            version.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

        }

        /*
        Die Methode createPassword erstellt ein zufälliges Passwort mit einer bestimmten Länge
        und mit einer bestimmten Auswahl aus Buchstaben, Nummern und spezielle Charaktere.
        Inspiration: https://stackoverflow.com/a/54997
        */
        public string createPassword(int length, bool l, bool n, bool s)
        {
            const string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"; //Ein String aus den zu verwendenen Buchstaben
            const string numbers = "0123456789"; //Ein String aus den zu verwendenen Nummern
            const string special = "!#%&()+-./:;=<>?{}_"; //Ein String aus den zu verwendenen speziellen Charakteren
            string valid = ""; //Ein String, welcher die Auswahl aus Buchstaben, Nummern und/oder speziellen Charakteren darstellt.
            if (l)
                valid = letters;
            if (n)
                valid = numbers;
            if (s)
                valid = special;
            if (l && n)
                valid = letters + numbers;
            if (l && s)
                valid = letters + special;
            if (n && s)
                valid = numbers + special;
            if (l && n && s)
                valid = letters + numbers + special;

            StringBuilder password = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--) //Erstellt ein ein Password mit der angegebenen Länge und den ausgewählten Zeichen, mithilfe eines StringBuilders (password) und zufälligen Zeichen aus der Auswahl (valid)
                password.Append(valid[rnd.Next(valid.Length)]);
            return password.ToString();
        }

        /*
        Die Methode passStrength überprüft die Sicherheit eines angegebenen Passworts,
        indem sie diesem eine Bewertung in Form von einemn Integer (score) zuordnet und ausgibt.
        Inspiration: https://stackoverflow.com/questions/12899876/checking-strings-for-a-strong-enough-password
        */
        public int passStrength(string password)
        {
            int score = 0;

            if (password.Length == 1)
                return 0;
            if (password.Length < 4)
                return 1;
            if (password.Length >= 8)
                score++;
            if (password.Length >= 12)
                score++;
            if (Regex.Match(password, @"[0-9]+(\.[0-9][0-9]?)?", RegexOptions.ECMAScript).Success) //Überprüft, ob in dem Passwort Zahlen von 0-9 vorhanden sind.
                score++;
            if (Regex.Match(password, @"^(?=.*[a-z])(?=.*[A-Z]).+$", RegexOptions.ECMAScript).Success) //Überprüft, ob in dem Passwort Groß- und Kleinbuchstaben verwendet wurden.
                score++;
            if (Regex.Match(password, @"[!,#,%,&,?,_,-,(,),+,.,/,:,;,=,<,>,{,}]", RegexOptions.ECMAScript).Success) //Überprüft, ob in dem Passwort mindestens ein Sonderzeichen enthalten ist.
                score++;

            return score;
        }

        /*
        Die Methode applyStrength passt den Wert einer angegebene ProgressBar,
        je nach der Stärke des angegebenen Passworts an.
        */
        public void applyStrength(string password, Bunifu.Framework.UI.BunifuProgressBar progress)
        {
            if ((Program.KeySafe.passStrength(password) == 0))
            {
                progress.Value = 5;
                progress.ProgressColor = Color.Red;
            }
            if (Program.KeySafe.passStrength(password) == 1)
            {
                progress.Value = 10;
                progress.ProgressColor = Color.OrangeRed;
            }
            if (Program.KeySafe.passStrength(password) == 2)
            {
                progress.Value = 25;
                progress.ProgressColor = Color.Orange;
            }
            if (Program.KeySafe.passStrength(password) == 3)
            {
                progress.Value = 50;
                progress.ProgressColor = Color.Yellow;
            }
            if (Program.KeySafe.passStrength(password) == 4)
            {
                progress.Value = 70;
                progress.ProgressColor = Color.GreenYellow;
            }
            if (Program.KeySafe.passStrength(password) == 5)
            {
                progress.Value = 100;
                progress.ProgressColor = Color.Lime;
            }
            
        }

        /*
        Die Methode LoadSettings lädt Benutzereinstellungen, damit die gespeicherten
        Einstellungen auch nach erneutem Start des Programms auswirkungen haben.
        */
        public void LoadSettings()
        {
            if (!Properties.Settings.Default.onlineLogin)
            {
                if (Properties.Settings.Default.USBClose)
                {
                    timer1.Start();
                    USBCloseCheck.Checked = true;
                }
                else
                    USBCloseCheck.Checked = false;
            }
            else
            {
                if (Properties.Settings.Default.stayLoggedIn)
                {
                    USBCloseCheck.Checked = true;
                }
                else
                    USBCloseCheck.Checked = false;
            }

            ShowIndexCheck.Checked = Properties.Settings.Default.ShowIndex;

            if (Properties.Settings.Default.RainbowColor)
            {
                rainbowTimer.Start();
                rainbowCheck.Checked = true;
            }
            else
                rainbowCheck.Checked = false;

            Program.KeySafe.Opacity = Properties.Settings.Default.opacity;
            opacitySlider.Value = (int)Decimal.Subtract((Decimal.Multiply((decimal)Properties.Settings.Default.opacity,100)), 50);
            if (Properties.Settings.Default.englishB)
            {
                english();
            }
            else
            {
                german();
            }

            if (Properties.Settings.Default.DarkMode)
            {
                darkMode(Color.FromArgb(33, 37, 41), Color.FromArgb(247, 247, 247), Color.FromArgb(42, 46, 50), Color.FromArgb(33, 37, 41), Color.FromArgb(27, 30, 33));
                darkModeCheck.Checked = true;
            }
            else
            {
                darkModeCheck.Checked = false;
            }

            if (Properties.Settings.Default.onlineLogin)
            {
                changeUsernameBtn.Visible = true;
                changePasswordBtn.Width = 353;
                changePasswordBtn.Height = 40;
            }
            else
            {
                changeUsernameBtn.Visible = false;
                changePasswordBtn.Width = 714;
                changePasswordBtn.Height = 40;
            }
        }

        /*
        Ausgabe von der Verschlüsselung von einem eingegebenen Plaintext mit dem angegebenen Schlüssel mithilfe von AES.
        Quelle: https://www.c-sharpcorner.com/article/encryption-and-decryption-using-a-symmetric-key-in-c-sharp/
        */
        public static string EncryptString(string plainText, string key)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        /*
        Ausgabe von der Entschlüsselung von einem eingegebenen Ciphertext mit dem angegebenen Schlüssel mithilfe von AES.
        Quelle: https://www.c-sharpcorner.com/article/encryption-and-decryption-using-a-symmetric-key-in-c-sharp/
        */
        public static string DecryptString(string cipherText, string key)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        /*
        Entschlüsseln aller Passwörter und Überschreiben der JSON-Datei für die gespeicherten Accounts.
        */
        public void EncryptPasswords()
        {
            // Jedes Passwort von jedem Item in der Liste "Entry" wird verschlüsselt mithilfe der EncryptString Methode und dem gespeicherten Schlüssel
            foreach (Entry item in Entry)
            {
                item.Password = EncryptString(item.Password, Properties.Settings.Default.key);
            }

            // Die aktualisierte Liste mit den verschlüsselten Passwörtern wird in die json Datei übernommen
            File.WriteAllText(@"accounts.json", JsonConvert.SerializeObject(Entry));
        }

        /*
        Methode zum Generieren und Speichern eines neuen Keys für die Ver-/Entschlüsselung 
        */
        public void GenerateNewKey()
        {
            Properties.Settings.Default.key = createPassword(32, true, true, false);
            Properties.Settings.Default.Save();
        }

        /*
        Methode zum Anzeigen von der durchschnittlichen Sicherheit der Passwörter in Prozent.
        Wird später verwendet um die Statistiken auf dem Home-Tab anzuzeigen.
        */
        public void LoadSafetyStats()
        {
            List<int> safetyList = new List<int>();
            int safetySum = 0;
            int safetyAverage;
            safetyList.Clear();

            foreach (Entry item in Entry)
            {
                safetyList.Add(passStrength(item.Password));
            }

            foreach (int item in safetyList)
            {
                safetySum = safetySum + item;
            }

            if (safetyList.Count() != 0)
                safetyAverage = safetySum / safetyList.Count();
            else
                safetyAverage = 0;

            safetyAverageP.Value = safetyAverage * 20;
            safetyAverageP.Font = new System.Drawing.Font("Arial", 20.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            safetyAverageP.Font = new System.Drawing.Font("Arial", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        /*
        Methode zum Anzeigen von unsicheren Passwörtern auf dem Home-Tab.
        */
        public async void LoadUnsafePasswords()
        {
            flowLayoutPanel2.Controls.Clear();

            foreach (Entry item in Entry)
            {
                bool flag;

                if (checkInternet())
                {
                    var client = new PwnedPasswordsClient();
                    flag = await client.IsPasswordPwnedAsync(item.Password);
                }
                else
                {
                    flag = false;
                }

                if ((passStrength(item.Password) <= 3) || flag)
                {
                    PasswordItem passwordItem = new PasswordItem();
                    passwordItem.Name = item.Name;
                    passwordItem.Index = item.Name;
                    passwordItem.Website = item.Website;
                    passwordItem.Username = item.Username;
                    passwordItem.Email = item.Email;
                    passwordItem.Password = item.Password;
                    flowLayoutPanel2.Controls.Add(passwordItem);
                }
            }
        }

        /*
        Methode zum Anzeigen von Statistiken zu der Allgemeinen Sicherheit der Passwörter.
        Wird später verwendet um die Statistiken auf dem Home-Tab anzuzeigen.
        */
        public void LoadPasswordAmount()
        {
            // Anzeigen der gesamten Passwordanzahl
            passwordAmount.Text = Entry.Count().ToString();

            // Anzeigen der Anzahl der wiederholten Passwörter
            List<Entry> Repeated = new List<Entry>();
            List<string> RepeatedPasswords = new List<string>();

            foreach (Entry item in Program.KeySafe.Entry)
            {
                string password = item.Password;
                int repeatedint = 0;

                foreach (Entry x in Program.KeySafe.Entry)
                {
                    if(password == x.Password)
                    {
                        repeatedint ++;
                    }
                }

                if(repeatedint > 1)
                {
                    Repeated.Add(item);
                }
            }

            foreach (Entry item in Repeated)
            {
                RepeatedPasswords.Add(item.Password);
            }
            
            repeatedAmount.Text = RepeatedPasswords.Distinct().ToList().Count().ToString();

            // Anzeigen der Anzahl der sicheren Passwörter
            int secureint = 0;
            foreach (Entry item in Program.KeySafe.Entry)
            {
                if (passStrength(item.Password) > 3)
                {
                    secureint++;
                }
            }

            secureAmount.Text = secureint.ToString();

            // Anzeigen der Anzahl der unsicheren Passwörter
            int weakint = 0;
            foreach (Entry item in Program.KeySafe.Entry)
            {
                if (passStrength(item.Password) < 3)
                {
                    weakint++;
                }
            }

            weakAmount.Text = weakint.ToString();
        }

        /*
        Methode zum Anzeigen von Statistiken zu der Darknet-Aktivität der Passwörter. (Progressbar & Anzahl der Leaks/Passwörter)
        Wird später verwendet um die Statistiken auf dem Darknet-Aktivität-Tab anzuzeigen.
        */
        public async void LoadLeakStats()
        {
            if (checkInternet())
            {
                List<string> unsafePasswords = new List<string>();
                foreach (Entry item in Entry)
                {
                    var client = new PwnedPasswordsClient();
                    bool flag = await client.IsPasswordPwnedAsync(item.Password);
                    if (flag)
                    {
                        unsafePasswords.Add(item.Password);
                    }
                }

                leakedPasswords.Text = unsafePasswords.Count().ToString();

                if(Entry.Count() != 0)
                {
                    leakAmountP.MaxValue = Entry.Count();
                }
            
                leakAmountP.Value = unsafePasswords.Count();

                
            }
            else
            {
                leakedPasswords.Text = "?";
                leakAmountP.MaxValue = 1;
                leakAmountP.Value = 0;
            }

            totalPasswords.Text = Entry.Count().ToString();

            leakAmountP.Font = new System.Drawing.Font("Arial", 20.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            leakAmountP.Font = new System.Drawing.Font("Arial", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        /*
        Methode zum Anzeigen der geleakten Passwörtern auf dem Darknet-Aktivität-Tab.
        */
        public async void LoadLeakedPasswords()
        {
            pwnedPasswordsPanel.Controls.Clear();

            if (checkInternet())
            {
                foreach (Entry item in Entry)
                {
                    var client = new PwnedPasswordsClient();
                    bool flag = await client.IsPasswordPwnedAsync(item.Password);
                    if (flag)
                    {
                        PasswordItem passwordItem = new PasswordItem();
                        passwordItem.Name = item.Name;
                        passwordItem.Index = item.Name;
                        passwordItem.Website = item.Website;
                        passwordItem.Username = item.Username;
                        passwordItem.Email = item.Email;
                        passwordItem.Password = item.Password;
                        pwnedPasswordsPanel.Controls.Add(passwordItem);
                    }
                }
            }
            
        }

        /*
        Methode zum Ändern der UI-/Akzentfarbe (einzelne Elemente).
        Wird in dem Settings-Tab verwendet.
        */
        public void changeColors(Color light, Color normal, Color dark)
        {
            // Grundelemente
            mini.ForeColor = normal;
            close.ForeColor = normal;
            bunifuGradientPanel1.GradientBottomRight = light;

            if (Properties.Settings.Default.resetColor)
            {
                bunifuGradientPanel1.GradientBottomLeft = dark;
                bunifuGradientPanel1.GradientTopLeft = dark;
                bunifuGradientPanel1.GradientTopRight = dark;
            }
            else
            {
                bunifuGradientPanel1.GradientBottomLeft = normal;
                bunifuGradientPanel1.GradientTopLeft = normal;
                bunifuGradientPanel1.GradientTopRight = normal;
            }
            
            homeBtn.Activecolor = dark;
            homeBtn.OnHovercolor = dark;
            passwordsBtn.Activecolor = dark;
            passwordsBtn.OnHovercolor = dark;
            passGenBtn.Activecolor = dark;
            passGenBtn.OnHovercolor = dark;
            darknetBtn.Activecolor = dark;
            darknetBtn.OnHovercolor = dark;
            settingsBtn.Activecolor = dark;
            settingsBtn.OnHovercolor = dark;
            label3.BackColor = normal;

            // Settings Tab
            changeColor.FlatAppearance.MouseOverBackColor = dark;
            changeColor.FlatAppearance.MouseDownBackColor = dark;
            changeColor.BackColor = normal;
            resetColor.FlatAppearance.MouseOverBackColor = dark;
            resetColor.FlatAppearance.MouseDownBackColor = dark;
            resetColor.BackColor = normal;
            resetUSB.FlatAppearance.MouseOverBackColor = dark;
            resetUSB.FlatAppearance.MouseDownBackColor = dark;
            resetUSB.BackColor = normal;
            addKey.FlatAppearance.MouseOverBackColor = dark;
            addKey.FlatAppearance.MouseDownBackColor = dark;
            addKey.BackColor = normal;
            USBCloseCheck.CheckedOnColor = normal;
            ShowIndexCheck.CheckedOnColor = normal;
            ShowIndexCheck.ChechedOffColor = SystemColors.ControlLight;
            darkModeCheck.CheckedOnColor = normal;
            darkModeCheck.ChechedOffColor = SystemColors.ControlLight;
            opacitySlider.IndicatorColor = normal;
            rainbowCheck.CheckedOnColor = normal;
            rainbowCheck.ChechedOffColor = SystemColors.ControlLight;
            saveData.FlatAppearance.MouseOverBackColor = dark;
            saveData.FlatAppearance.MouseDownBackColor = dark;
            saveData.BackColor = normal;
            loadData.FlatAppearance.MouseOverBackColor = dark;
            loadData.FlatAppearance.MouseDownBackColor = dark;
            loadData.BackColor = normal;
            languageBtn.BackColor = normal;
            languageBtn.FlatAppearance.MouseOverBackColor = dark;
            languageBtn.FlatAppearance.MouseDownBackColor = dark;
            changePasswordBtn.BackColor = normal;
            changePasswordBtn.FlatAppearance.MouseOverBackColor = dark;
            changePasswordBtn.FlatAppearance.MouseDownBackColor = dark;
            changeUsernameBtn.BackColor = normal;
            changeUsernameBtn.FlatAppearance.MouseOverBackColor = dark;
            changeUsernameBtn.FlatAppearance.MouseDownBackColor = dark;

            // PassGen Tab
            generatedPassword.LineFocusedColor = dark;
            generatedPassword.LineMouseHoverColor = dark;
            generatedPassword.LineIdleColor = normal;
            generateBtn.FlatAppearance.MouseOverBackColor = dark;
            generateBtn.FlatAppearance.MouseDownBackColor = dark;
            generateBtn.BackColor = normal;
            copyBtn.FlatAppearance.MouseOverBackColor = dark;
            copyBtn.FlatAppearance.MouseDownBackColor = dark;
            copyBtn.BackColor = normal;
            bunifuSlider1.IndicatorColor = normal;
            lettersC.BackColor = normal;
            lettersC.CheckedOnColor = normal;
            numbersC.BackColor = normal;
            numbersC.CheckedOnColor = normal;
            specialC.BackColor = normal;
            specialC.CheckedOnColor = normal;

            // Home Tab
            safetyAverageP.ForeColor = normal;
            safetyAverageP.ProgressColor = normal;
            passwordAmount.ForeColor = normal;
            secureAmount.ForeColor = normal;
            repeatedAmount.ForeColor = normal;
            weakAmount.ForeColor = normal;

            //Darknet Tab
            leakAmountP.ForeColor = normal;
            leakAmountP.ProgressColor = normal;
            totalPasswords.ForeColor = normal;
            leakedPasswords.ForeColor = normal;

            // SaveKey
            Program.SaveKey.bunifuGradientPanel1.GradientBottomLeft = normal;
            Program.SaveKey.bunifuGradientPanel1.GradientBottomRight = normal;
            Program.SaveKey.bunifuGradientPanel1.GradientTopLeft = normal;
            Program.SaveKey.bunifuGradientPanel1.GradientTopRight = normal;
            Program.SaveKey.WebsiteTxt.LineFocusedColor = dark;
            Program.SaveKey.WebsiteTxt.LineIdleColor = normal;
            Program.SaveKey.WebsiteTxt.LineMouseHoverColor = dark;
            Program.SaveKey.UsernameTxt.LineFocusedColor = dark;
            Program.SaveKey.UsernameTxt.LineIdleColor = normal;
            Program.SaveKey.UsernameTxt.LineMouseHoverColor = dark;
            Program.SaveKey.EmailTxt.LineFocusedColor = dark;
            Program.SaveKey.EmailTxt.LineIdleColor = normal;
            Program.SaveKey.EmailTxt.LineMouseHoverColor = dark;
            Program.SaveKey.PasswordTxt.LineFocusedColor = dark;
            Program.SaveKey.PasswordTxt.LineIdleColor = normal;
            Program.SaveKey.PasswordTxt.LineMouseHoverColor = dark;
            Program.SaveKey.copyWebsiteBtn.BackColor = normal;
            Program.SaveKey.copyWebsiteBtn.FlatAppearance.MouseOverBackColor = dark;
            Program.SaveKey.copyWebsiteBtn.FlatAppearance.MouseDownBackColor = dark;
            Program.SaveKey.copyUsernameBtn.BackColor = normal;
            Program.SaveKey.copyUsernameBtn.FlatAppearance.MouseOverBackColor = dark;
            Program.SaveKey.copyUsernameBtn.FlatAppearance.MouseDownBackColor = dark;
            Program.SaveKey.copyEmailBtn.BackColor = normal;
            Program.SaveKey.copyEmailBtn.FlatAppearance.MouseOverBackColor = dark;
            Program.SaveKey.copyEmailBtn.FlatAppearance.MouseDownBackColor = dark;
            Program.SaveKey.copyPasswordBtn.BackColor = normal;
            Program.SaveKey.copyPasswordBtn.FlatAppearance.MouseOverBackColor = dark;
            Program.SaveKey.copyPasswordBtn.FlatAppearance.MouseDownBackColor = dark;
            Program.SaveKey.addBtn.BackColor = normal;
            Program.SaveKey.addBtn.FlatAppearance.MouseOverBackColor = dark;
            Program.SaveKey.addBtn.FlatAppearance.MouseDownBackColor = dark;
            Program.SaveKey.mini.FlatAppearance.MouseOverBackColor = dark;
            Program.SaveKey.mini.FlatAppearance.MouseDownBackColor = dark;
            Program.SaveKey.close.FlatAppearance.MouseOverBackColor = dark;
            Program.SaveKey.close.FlatAppearance.MouseDownBackColor = dark;
        }

        /*
        Methode zum Ändern der Sprache auf Englisch durch anpassen der Texte der einzelnen Elemente.
        Wird in dem Settings-Tab verwendet.
        */
        public void english()
        {
            // Grundelemente
            addKey.Text = "Add Key";
            homeBtn.Text = "Home";
            passwordsBtn.Text = "Passwords";
            passGenBtn.Text = "PassGen";
            darknetBtn.Text = "Darknet Activity";
            settingsBtn.Text = "Settings";

            // Home
            label14.Text = "Password-Safety:";
            label100.Text = "Total";
            label20.Text = "Secure";
            label18.Text = "Reused";
            label22.Text = "Weak";
            label15.Text = "Unsecure Passwords:";

            // PassGen
            label7.Text = "Password";
            generateBtn.Text = "Generate";
            copyBtn.Text = "Copy";
            label11.Text = "Character types";
            label8.Text = "Letters (z.B. Aa)";
            label9.Text = "Numbers (z.B. 345)";
            label10.Text = "Special characters (z.B. !_#%)";
            lengthTxt.Text = "Length (" + Decimal.Add(bunifuSlider1.Value,4) + ")";

            // Darknet
            label23.Text = "Leak Amount";
            label28.Text = "Total";
            label26.Text = "Leaked";
            label24.Text = "Leaked Passwords:";

            // Settings
            label12.Text = "Appereance";
            changeColor.Text = "Change Color";
            resetColor.Text = "Reset Color";
            label17.Text = "Enable Rainbow Color";
            label13.Text = "Show Account Index";
            label5.Text = "Login Settings";
            resetUSB.Text = "Reset Login Type";
            
            label16.Text = "Password Settings";
            saveData.Text = "Save Data in Folder";
            loadData.Text = "Load Data from Folder";
            label19.Text = "Language";
            englishBtn.Text = "English";
            germanBtn.Text = "German";
            opacityLabel.Text = "Opacity (" + Decimal.Add(opacitySlider.Value, 50) + "%)";
            languageBtn.Text = "English";
            if (Properties.Settings.Default.onlineLogin)
            {
                changePasswordBtn.Text = "Change Password";
                label6.Text = "Stay Logged In";
            }
            else
            {
                changePasswordBtn.Text = "Reset USB-Device";
                label6.Text = "Close App when USB isn't connected";
            }

            //SaveKey
            Program.SaveKey.label1.Text = "Username";
            Program.SaveKey.label3.Text = "Password";
            Program.SaveKey.addBtn.Text = "Save";
        }

        /*
        Methode zum Ändern der Sprache auf Deutsch durch anpassen der Texte der einzelnen Elemente.
        Wird in dem Settings-Tab verwendet.
        */
        public void german()
        {
            // Grundelemente
            addKey.Text = "Passwort hinzufügen";
            homeBtn.Text = "Home";
            passwordsBtn.Text = "Passwörter";
            passGenBtn.Text = "PassGen";
            darknetBtn.Text = "Darknet Aktivität";
            settingsBtn.Text = "Einstellungen";

            // Home
            label14.Text = "Passwort-Sicherheit:";
            label100.Text = "Insgesamt";
            label20.Text = "Sicher";
            label18.Text = "Wiederverwendet";
            label22.Text = "Schwach";
            label15.Text = "Unsichere Passwörter:";

            // PassGen
            label7.Text = "Passwort";
            generateBtn.Text = "Generieren";
            copyBtn.Text = "Kopieren";
            label11.Text = "Charaktere";
            label8.Text = "Buchstaben (z.B. Aa)";
            label9.Text = "Nummern (z.B. 345)";
            label10.Text = "Speciale Charaktere (z.B. !_#%)";
            lengthTxt.Text = "Länge (" + Decimal.Add(bunifuSlider1.Value, 4) + ")";

            //DarkNet
            label23.Text = "Leak Anzahl";
            label28.Text = "Insgesamt";
            label26.Text = "Geleaked";
            label24.Text = "Geleakte Passwörter:";

            // Settings
            label12.Text = "Aussehen";
            changeColor.Text = "Farbe ändern";
            resetColor.Text = "Farbe zurücksetzen";
            label17.Text = "Regenbogen Farben";
            label13.Text = "Account Index anzeigen";
            label5.Text = "Login Einstellungen";
            resetUSB.Text = "Login Form zurücksetzen";
            
            label16.Text = "Passwort Einstellungen";
            saveData.Text = "Daten in einem Ordner sichern";
            loadData.Text = "Daten aus einem Ordner laden";
            label19.Text = "Sprache";
            englishBtn.Text = "Englisch";
            germanBtn.Text = "Deutsch";
            opacityLabel.Text = "Deckkraft (" + Decimal.Add(opacitySlider.Value, 50) + "%)";
            languageBtn.Text = "Deutsch";
            if (Properties.Settings.Default.onlineLogin)
            {
                changePasswordBtn.Text = "Passwort ändern";
                label6.Text = "Eingeloggt bleiben";
            }
            else
            {
                changePasswordBtn.Text = "USB-Gerät zurücksetzen";
                label6.Text = "App schließen, wenn der USB nicht verbunden ist";
            }

            //SaveKey
            Program.SaveKey.label1.Text = "Benutzername";
            Program.SaveKey.label3.Text = "Passwort";
            Program.SaveKey.addBtn.Text = "Speichern";
        }

        /*
        Methode zum Ändern der Hintergrundfarbe und Textfarbe einzelner Elemente.
        Wird in dem Settings-Tab für Darkmode verwendet.
        */
        public void darkMode(Color top, Color text, Color lightBack, Color darkBack, Color hover)
        {
            // Grundelemente
            panel1.BackColor = top;
            Program.KeySafe.BackColor = lightBack;
            home.BackColor = lightBack;
            passwords.BackColor = lightBack;
            flowLayoutPanel1.BackColor = lightBack;
            passGen.BackColor = lightBack;
            darknet.BackColor = lightBack;
            settings.BackColor = lightBack;
            label4.Visible = !Properties.Settings.Default.DarkMode;
            mini.FlatAppearance.MouseOverBackColor = hover;
            mini.FlatAppearance.MouseDownBackColor = hover;
            close.FlatAppearance.MouseOverBackColor = hover;
            close.FlatAppearance.MouseDownBackColor = hover;

            // Home
            flowLayoutPanel2.BackColor = lightBack;
            safetyAverageP.ProgressBackColor = darkBack;
            label14.ForeColor = text;
            label100.ForeColor = text;
            label20.ForeColor = text;
            label18.ForeColor = text;
            label22.ForeColor = text;
            label15.ForeColor = text;

            // passGen
            label7.ForeColor = text;
            if (Properties.Settings.Default.DarkMode)
                generatedPassword.BackColor = darkBack;
            else
                generatedPassword.BackColor = Color.WhiteSmoke;
            generatedPassword.ForeColor = text;
            passStrengthP.BackColor = darkBack;
            lengthTxt.ForeColor = text;
            bunifuSlider1.BackgroudColor = darkBack;
            label11.ForeColor = text;
            lettersC.ChechedOffColor = darkBack;
            numbersC.ChechedOffColor = darkBack;
            specialC.ChechedOffColor = darkBack;
            label8.ForeColor = text;
            label9.ForeColor = text;
            label10.ForeColor = text;

            // Darknet
            pwnedPasswordsPanel.BackColor = lightBack;
            leakAmountP.ProgressBackColor = darkBack;
            label23.ForeColor = text;
            label28.ForeColor = text;
            label26.ForeColor = text;
            label24.ForeColor = text;

            // Settings
            label12.ForeColor = text;
            opacityLabel.ForeColor = text;
            opacitySlider.BackgroudColor = darkBack;
            rainbowCheck.ChechedOffColor = darkBack;
            darkModeCheck.ChechedOffColor = darkBack;
            ShowIndexCheck.ChechedOffColor = darkBack;
            label17.ForeColor = text;
            label21.ForeColor = text;
            label13.ForeColor = text;
            label5.ForeColor = text;
            USBCloseCheck.ChechedOffColor = darkBack;
            label6.ForeColor = text;
            label16.ForeColor = text;
            label19.ForeColor = text;
            panel2.BackColor = darkBack;
            englishBtn.BackColor = darkBack;
            englishBtn.FlatAppearance.MouseOverBackColor = hover;
            englishBtn.FlatAppearance.MouseDownBackColor = hover;
            germanBtn.BackColor = darkBack;
            germanBtn.FlatAppearance.MouseOverBackColor = hover;
            germanBtn.FlatAppearance.MouseDownBackColor = hover;
            englishBtn.ForeColor = text;
            germanBtn.ForeColor = text;

            // SaveKey
            Program.SaveKey.label7.ForeColor = text;
            Program.SaveKey.label1.ForeColor = text;
            Program.SaveKey.label2.ForeColor = text;
            Program.SaveKey.label3.ForeColor = text;
            Program.SaveKey.WebsiteTxt.ForeColor = text;
            Program.SaveKey.UsernameTxt.ForeColor = text;
            Program.SaveKey.EmailTxt.ForeColor = text;
            Program.SaveKey.PasswordTxt.ForeColor = text;

            if (Properties.Settings.Default.DarkMode)
            {
                Program.SaveKey.BackColor = darkBack;
                Program.SaveKey.WebsiteTxt.BackColor = hover;
                Program.SaveKey.UsernameTxt.BackColor = hover;
                Program.SaveKey.EmailTxt.BackColor = hover;
                Program.SaveKey.PasswordTxt.BackColor = hover;
                Program.SaveKey.passStrengthP.BackColor = hover;
            }
            else
            {
                Program.SaveKey.BackColor = SystemColors.Control;
                Program.SaveKey.WebsiteTxt.BackColor = Color.Gainsboro;
                Program.SaveKey.UsernameTxt.BackColor = Color.Gainsboro;
                Program.SaveKey.EmailTxt.BackColor = Color.Gainsboro;
                Program.SaveKey.PasswordTxt.BackColor = Color.Gainsboro;
                Program.SaveKey.passStrengthP.BackColor = SystemColors.ActiveBorder;
            }
            
        }

        /*
        Lesen der Version aus einer angegebenen XML-Datei und Ausgabe dieser Version als String.
        */
        public string getNewestVersion(string url)
        {
            WebClient client = new WebClient();
            string xmlFile = client.DownloadString(url);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlFile);
            return xmlDoc.SelectSingleNode("item/version").InnerText;
        }

        /*
        Ausgabe eines Booleans, je nachdem ob "http://google.com/generate_204" als html-Code geladen werden kann, oder eine Exception auswirft.
        Wird dafür verwendet um Exceptions beim Internetzugriff zu verhindern, falls keine Internet-Verbindung vorhanden ist.
        */
        public bool checkInternet()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                        return true;
            }
            catch
            {
                return false;
            }
        }

        /*
        Ausgabe der RGB-Werte als String einer angegebenen Farbe mit Komma getrennt.
        */
        public static string colorToRGB(Color c)
        {
            return c.R.ToString() + "," + c.G.ToString() + "," + c.B.ToString();
        }

        public static string boolToString(bool b)
        {
            if (b)
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }
        #endregion

        #region Events


        #region Allgemein

        /*
        Der Timer timer1 überprüft bei jedem Tick, ob das gespeicherte USB-Gerät
        in den aktuellen USB-IDs vorhanden ist und schließt, wenn es nicht vorhanden ist das Programm.
        */
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!Properties.Settings.Default.onlineLogin)
            {
                LoadDevices();
                if (!USBList.Contains(Properties.Settings.Default.selection))
                {
                    timer1.Stop();
                    System.Windows.Forms.Application.Exit();
                }
            }
        }

        private void KeySafe_Shown(object sender, EventArgs e)
        {
            // Akzentdarbe auf die vorher gespeicherte Farbe ändern.
            changeColors(Properties.Settings.Default.colorLight, Properties.Settings.Default.color, Properties.Settings.Default.colorDark);

            // Einstellungen laden.
            LoadSettings();

            // Aussehen laden.
            style();

            try
            {
                // Erstellen einer Liste des Typen Entry's, in welcher alle Objekte aus der zuvor gespeicherten json Datei enthalten sind, mithilfe von der Newtonsoft-Library
                List<Entry> itemList = JsonConvert.DeserializeObject<List<Entry>>(File.ReadAllText(@"accounts.json"));
                // Für jedes Item in itemList mit dem Typen Entry wird ein passwordItem erstellt, welches in der Benutzeroberfläche angezeigt wird
                foreach (Entry item in itemList)
                {
                    // Jedes Passwort wird mithilfe der DecryptString Methode entschlüsselt, mit dem abgespeicherten Key
                    item.Password = DecryptString(item.Password, Properties.Settings.Default.key);

                    // Speichern der Items aus der Liste "Entry", damit später Objekte editiert werden können.
                    Program.KeySafe.Entry.Add(new Entry(item.Name, item.Website, item.Username, item.Email, item.Password));
                    // Für jedes Item in der Liste wird ein neues passwordItem erstellt, mit den Daten von dem Item und anschließend zum flowLayoutPanel hinzugefügt.
                    PasswordItem passwordItem = new PasswordItem();
                    passwordItem.Name = item.Name;
                    passwordItem.Index = item.Name;
                    passwordItem.Website = item.Website;
                    passwordItem.Username = item.Username;
                    passwordItem.Email = item.Email;
                    passwordItem.Password = item.Password;
                    flowLayoutPanel1.Controls.Add(passwordItem);
                }
            }
            catch (Exception){ } // damit falls keine json-Datei vorhanden ist, sich das Programm nicht durch ein Error aufhängt

            // Ein neuer Key für die Verschlüsselung wird generiert.
            GenerateNewKey();

            // Lädt die  Sicherheit der Passwörter
            LoadSafetyStats();
            LoadUnsafePasswords();
            LoadPasswordAmount();
            LoadLeakStats();
            LoadLeakedPasswords();

            // Überprüfen auf eine Internetverbindung
            offlineTimer.Start();
        }
        
        /*
        Beim Schließen des Programms wird die Überprüfung auf eine Internetverbindung beendet und alle Passwörter verschlüsselt.
        */
        private void KeySafe_FormClosing(object sender, FormClosingEventArgs e)
        {
            offlineTimer.Stop();
            EncryptPasswords();
        }

        /*
        Bei Klick auf den Button "close" wird die Anwendung beendet.
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

        private void label3_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle, this.BackColor, ButtonBorderStyle.Solid); // Erstellen der Farbeigenschaft des Seperators
        }

        private void label4_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle, this.BackColor, ButtonBorderStyle.Solid); // Erstellen der Farbeigenschaft des Seperators
        }

        /*
        Bei Klick auf den Button "addKey" wird die Form "SaveKey" aufgerufen, um ein neuen Account hinzuzufügen.
        */
        private void addKey_Click(object sender, EventArgs e)
        {
            Program.SaveKey.change = false; // Dies ist für die Form SaveKey, damit sie je nach dem Wert des Booleans entscheiden kann, ob ein neues Item hinzugefügt werden soll, oder editiert werden soll.
            Program.SaveKey.Show();
        }

        /*
        Der Timer "offlineTimer" überprüft bei jedem Tick, ob eine Internetverbindung besteht.
        Je nachdem wird das "offlinePanel" angezeigt, oder nicht.
        */
        private void offlineTimer_Tick(object sender, EventArgs e)
        {
            if (!checkInternet())
            {
                offlinePanel.Visible = true;
                addKey.Location = new Point(25, 27);
                mini.Location = new Point(677, 27);
                close.Location = new Point(717, 27);
            }
            else
            {
                offlinePanel.Visible = false;
                addKey.Location = new Point(25, 21);
                mini.Location = new Point(677, 21);
                close.Location = new Point(717, 21);
            }
        }

        #endregion


        #region Navigation

        /*
        Bei Klick auf den Button "homeBtn" wird der Tab "home" angezeigt und es werden die Statistiken für diesen Tab geladen.
        */
        private void homeBtn_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = home;
            LoadSafetyStats();
            LoadUnsafePasswords();
            LoadPasswordAmount();
        }

        /*
        Bei Klick auf den Button "passwordsBtn" wird der Tab "passwords" angezeigt und es werden alle vorhandenen Accounts/Passwörter geladen.
        */
        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = passwords;
            flowLayoutPanel1.Controls.Clear();
            foreach (Entry item in Entry)
            {
                PasswordItem passwordItem = new PasswordItem();
                passwordItem.Name = item.Name;
                passwordItem.Index = item.Name;
                passwordItem.Website = item.Website;
                passwordItem.Username = item.Username;
                passwordItem.Email = item.Email;
                passwordItem.Password = item.Password;
                flowLayoutPanel1.Controls.Add(passwordItem);
            }
        }

        /*
        Bei Klick auf den Button "passGenBtn" wird der Tab "passGen" angezeigt.
        */
        private void bunifuFlatButton1_Click_1(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = passGen;
        }

        /*
        Bei Klick auf den Button "darknetBtn" wird der Tab "darknet" angezeigt und es werden die Statistiken für diesen Tab geladen.
        */
        private void darknetBtn_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = darknet;
            LoadLeakStats();
            LoadLeakedPasswords();
        }

        /*
        Bei Klick auf den Button "settingsBtn" wird der Tab "settings" angezeigt.
        */
        private void settingsBtn_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = settings;
        }

        #endregion


        #region PassGen

        /*
        Bei Änderung des Wertes des Sliders "bunifuSlider1" wird die Länge des zu generierenen Passworts geändert.
        */
        private void bunifuSlider1_ValueChanged(object sender, EventArgs e)
        {
            passwordLength = bunifuSlider1.Value + 4;
            if (Properties.Settings.Default.englishB)
            {
                lengthTxt.Text = "Length (" + passwordLength + ")";
            }
            else
            {
                lengthTxt.Text = "Länge (" + passwordLength + ")";
            }
            
        }

        /*
        Bei Klick auf den Button "copyBtn" Wird das generierte Passwort kopiert.
        */
        private void copyBtn_Click(object sender, EventArgs e)
        {
            if (generatedPassword.Text != "")
            {
                Clipboard.SetText(generatedPassword.Text);
                MessageBox.Show("Copied to Clipboard!");
            }
            else
                MessageBox.Show("No password generated yet!");
            
        }

        /*
        Bei Klick auf den Button "generateBtn" wird ein Passwort mit bestimmter Länge und Charakteren generiert.
        */
        private void generateBtn_Click(object sender, EventArgs e)
        {
            if(!lettersC.Checked && !numbersC.Checked && !specialC.Checked)
                MessageBox.Show("Please select at least one character type!");
            else
                generatedPassword.Text = createPassword(passwordLength, lettersC.Checked, numbersC.Checked, specialC.Checked);
        }

        /*
        Bei Änderung des generierten Passworts wird die Stärke neu angezeigt.
        */
        private void generatedPassword_OnValueChanged(object sender, EventArgs e)
        {
            applyStrength(generatedPassword.Text, passStrengthP);
        }

        #endregion


        #region Settings

        /*
        Beim Klick auf den Button "resetUSB" wird das ausgewählte USB-Gerät zurückgesetzt und man wird auf die Form "Masterkey" umgeleitet.
        */
        private void resetUSB_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.onlineLogin)
            {
                if (Program.Register.getColumnSQL("username").Contains(Properties.Settings.Default.username))
                {
                    Program.Register.removeUser(Properties.Settings.Default.username, Properties.Settings.Default.password);
                }
            }
            Properties.Settings.Default.username = "";
            Properties.Settings.Default.password = "";
            Properties.Settings.Default.registered = false;
            Properties.Settings.Default.selection = "a"; // Der String selection für die gespeicherte USB-ID wird auf "a" gesetzt, damit man in der Form Masterkey fortfahren kann.
            Properties.Settings.Default.firstTime = true;
            Properties.Settings.Default.stayLoggedIn = false;
            Properties.Settings.Default.Save();
            timer1.Stop();
            this.Hide();
            Program.OfflineOnline.Show();
        }

        /*
        Bei Änderung der Checkbox "USBCloseCheck" wird die jeweilige Einstellung für 
        das Schließen des Programms beim Entfernen des USB-Geräts gespeichert und angewandt.
        */
        private void USBCloseCheck_OnChange(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.onlineLogin)
            {
                if (USBCloseCheck.Checked)
                {
                    Properties.Settings.Default.stayLoggedIn = true;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    Properties.Settings.Default.stayLoggedIn = false;
                    Properties.Settings.Default.Save();
                }
            }
            else
            {
                if (USBCloseCheck.Checked)
                {
                    timer1.Start();
                    Properties.Settings.Default.USBClose = true;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    timer1.Stop();
                    Properties.Settings.Default.USBClose = false;
                    Properties.Settings.Default.Save();
                }
            }
            
        }

        /*
        Bei Klick auf den Button "changeColor" wird ein Farbdialog geöffnet, durch welchen die Akzentfarbe geändert werden kann.
        */
        private void changeColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                Properties.Settings.Default.resetColor = false;
                changeColors(ControlPaint.Light(colorDialog1.Color), colorDialog1.Color, ControlPaint.Dark(colorDialog1.Color));
                Properties.Settings.Default.colorLight = ControlPaint.Light(colorDialog1.Color);
                Properties.Settings.Default.color = colorDialog1.Color;
                Properties.Settings.Default.colorDark = ControlPaint.Dark(colorDialog1.Color);
                Properties.Settings.Default.Save();
            }
        }

        /*
        Bei Änderung der CheckBox "ShowIndexCheck" wird die Einstellung gespeichert und angewandt,
        ob bei den Accounts (PassswordItem) der Index angezeigt werden soll.
        */
        private void ShowIndexCheck_OnChange(object sender, EventArgs e)
        {
            // Zeigt je nach der ausgewählten Einstellung den Index der Items an.
            if (ShowIndexCheck.Checked)
            {
                foreach (PasswordItem item in flowLayoutPanel1.Controls)
                    item.label1.Visible = true;
                foreach (PasswordItem item in flowLayoutPanel2.Controls)
                    item.label1.Visible = true;
                foreach (PasswordItem item in pwnedPasswordsPanel.Controls)
                    item.label1.Visible = true;

                Properties.Settings.Default.ShowIndex = true;
                Properties.Settings.Default.Save();
            }
            else
            {
                foreach (PasswordItem item in flowLayoutPanel1.Controls)
                    item.label1.Visible = false;
                foreach (PasswordItem item in flowLayoutPanel2.Controls)
                    item.label1.Visible = false;
                foreach (PasswordItem item in pwnedPasswordsPanel.Controls)
                    item.label1.Visible = false;

                Properties.Settings.Default.ShowIndex = false;
                Properties.Settings.Default.Save();
            }
        }

        /*
        Bei Klick auf den Button "resetColor" wird die standartmäßige Akzentfarbe wiederhergestellt und gespeichert.
        */
        private void resetColor_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.resetColor = true;
            changeColors(Color.FromArgb(110, 152, 191), Color.FromArgb(57, 77, 123), Color.FromArgb(41, 55, 103));
            Properties.Settings.Default.color = Color.FromArgb(57, 77, 123);
            Properties.Settings.Default.colorLight = Color.FromArgb(110, 152, 191);
            Properties.Settings.Default.colorDark = Color.FromArgb(41, 55, 103);
            Properties.Settings.Default.Save();
            label3.BackColor = Color.FromArgb(54, 75, 122);
            bunifuGradientPanel1.GradientBottomRight = Color.FromArgb(110, 152, 191);
        }

        /*
        Bei Änderung der CheckBox "rainbowCheck" wird je nach der Auswahl die Akzentfarbe mithilfe eines Timers auf die Regenbogenfarben geändert, oder nicht.
        */
        private void rainbowCheck_OnChange(object sender, EventArgs e)
        {
            if (rainbowCheck.Checked)
            {
                rainbowTimer.Start();
                Properties.Settings.Default.RainbowColor = true;
                Properties.Settings.Default.Save();
            }
            else
            {
                rainbowTimer.Stop();
                Properties.Settings.Default.RainbowColor = false;
                Properties.Settings.Default.Save();
                changeColors(Properties.Settings.Default.colorLight, Properties.Settings.Default.color, Properties.Settings.Default.colorDark);
            }
        }

        /*
        Der Timer "rainbowTimer" ändert bei jedem Tick die Akzentfarbe in Reihenfolge der Regenbogenfarben.
        Er wird für die Einstellung "Regenbogenfarben" verwendet.
        */
        private void rainbowTimer_Tick(object sender, EventArgs e)
        {
            changeColors(ControlPaint.Light(Color.FromArgb(r,g,b)), Color.FromArgb(r,g,b), ControlPaint.Dark(Color.FromArgb(r,g,b)));

            if (r > 0 && b == 0)
            {
                r--;
                g++;
            }
            if (g > 0 && r == 0)
            {
                g--;
                b++;
            }
            if (b > 0 && g == 0)
            {
                b--;
                r++;
            }
        }

        /*
        Bei Änderung des Wertes des "opacitySlider" wird die Durchsichtigkeit der Anwendung gespeichert, angewendet und auch angezeigt. 
        */
        private void opacitySlider_ValueChanged(object sender, EventArgs e)
        {
            float opacity = (float)Decimal.Divide(opacitySlider.Value + 50,100);
            Properties.Settings.Default.opacity = (float)Decimal.Divide(opacitySlider.Value + 50, 100);
            Properties.Settings.Default.Save();
            if(Properties.Settings.Default.englishB)
                opacityLabel.Text = "Opacity (" + Decimal.Add(opacitySlider.Value, 50) + "%)";
            else
                opacityLabel.Text = "Deckkraft (" + Decimal.Add(opacitySlider.Value, 50) + "%)";
            Program.KeySafe.Opacity = opacity;
        }        

        /*
        Bei Klick auf den Button "saveData" werden alle Accounts in einem ausgewählen Verzeichnis nach Websiten als Textdateien gespeichert.
        */
        private void saveData_Click(object sender, EventArgs e)
        {
            string savePath = "";
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK) // Öffnen eines FolderBrowserDialogs zur Auswahl des Verzeichnisses
            {
                savePath = folderBrowserDialog1.SelectedPath; // Ausgewähltes Verzeichnis

                // Falls der Ordner "Passwords" nicht schon exisitert, wird er hier erstellt
                bool folderExists = Directory.Exists(savePath + @"\Passwords");
                if (!folderExists)
                    Directory.CreateDirectory(savePath + @"\Passwords");

                foreach (Entry item in Entry)
                {
                    // Falls der Ordner für die jeweilige Webste nicht schon exisitiert, wird er hier erstellt
                    bool websiteExists = Directory.Exists(savePath + @"\Passwords\" + item.Website);
                    if (!websiteExists)
                        Directory.CreateDirectory(savePath + @"\Passwords\" + item.Website);

                    // Text für die zu erstellende Textdatei
                    string text = "Website: " + item.Website + Environment.NewLine + "Username: " + item.Username + Environment.NewLine + "E-Mail: " + item.Email + Environment.NewLine + "Password: " + item.Password;

                    // Berechnen der Anzahl der für diese Website bereits exisitierenen Accounts/Textdateien
                    System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(savePath + @"\Passwords\" + item.Website);
                    int count = dir.GetFiles().Length;

                    // Falls die Anzahl 0 ist, wird die Erste Account-Textdatei erstellt
                    if(count == 0)
                        File.WriteAllText(savePath + @"\Passwords\" + item.Website + @"\Account 1.txt", text);
                    else // Falls schon Account-Textdateien exisitieren:
                    {
                        for (int i = 1; i <= count; i++)
                        {
                            // Falls die bereits existierende Textdatei die Email, oder den Username enthält wird diese überschrieben und es geht zum nächsten Account weiter.
                            string check = File.ReadAllText(savePath + @"\Passwords\" + item.Website + @"\Account " + i + ".txt");
                            if (check.Contains("E-Mail: " + item.Email) || (check.Contains("Username: " + item.Username) && item.Username != ""))
                            {
                                File.WriteAllText(savePath + @"\Passwords\" + item.Website + @"\Account " + i + ".txt", text);
                                break;
                            }
                            else // Falls sie dies aber nicht enthält
                            {
                                // wird überprüft, ob keine weitere Textdatei unter dieser Website existiert.
                                // Falls das der Fall ist wird der Account als Textdatei gespeichert,
                                if (!File.Exists(savePath + @"\Passwords\" + item.Website + @"\Account " + Decimal.Add(i, 1) + ".txt"))
                                    File.WriteAllText(savePath + @"\Passwords\" + item.Website + @"\Account " + Decimal.Add(i, 1) + ".txt", text);
                                else
                                    continue; // wenn aber eine weitere Textdatei unter dieser Website existiert, wird diese als nächstes überprüft.
                            }
                        }
                    }
                }
            }
        }

        private void loadData_Click(object sender, EventArgs e)
        {
            string loadPath = "";
            if(folderBrowserDialog2.ShowDialog() == DialogResult.OK)
            {
                loadPath = folderBrowserDialog2.SelectedPath;
                
                bool folderExists = Directory.Exists(loadPath + @"\Passwords");
                if (folderExists)
                {
                    String[] websites = Directory.GetDirectories(loadPath + @"\Passwords\");
                    try
                    {
                        foreach (String item in websites)
                        {
                            
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.ToString());
                    }
                }
            }
        }

        /*
        Bei Klick auf den Button "languageBtn" wird je nach dem Status des Dropmenus (extended)
        das Dropmenu ausgefahren, oder eingefahren und das Icon gedreht (zum Anzeiges des Status)
        */
        bool extended = false;
        private void languageBtn_Click(object sender, EventArgs e)
        {
            if (extended)
            {
                extended = false;
                panel2.Height = 0;
                languageBtn.Image.RotateFlip(RotateFlipType.Rotate270FlipXY);
            }
            else
            {
                extended = true;
                panel2.Height = 72;
                languageBtn.Image.RotateFlip(RotateFlipType.Rotate90FlipXY);
            }
        }

        /*
        Bei Klick auf den Button "englishBtn" wird die Sprache auf Englisch geändert und als Einstellung gespeichert.
        */
        private void englishBtn_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.englishB = true;
            Properties.Settings.Default.Save();
            languageBtn.Text = "English";
            english();
            if (Properties.Settings.Default.DarkMode)
            {
                englishBtn.BackColor = Color.FromArgb(27, 30, 33);
                germanBtn.BackColor = Color.FromArgb(33, 37, 41);
            }
            else
            {
                englishBtn.BackColor = SystemColors.ControlDark;
                germanBtn.BackColor = SystemColors.ControlLight;
            }
            
        }

        /*
        Bei Klick auf den Button "changePasswordBtn" wird die Form "ChangeLoginData" angezeigt, falls der online-Login ausgewählt wurde, oder im anderen Fall die "MasterKey" Form.
        */
        private void changePasswordBtn_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.onlineLogin)
            {
                if (Properties.Settings.Default.englishB)
                {
                    Program.ChangeLoginData.label1.Text = "Change Password";
                    Program.ChangeLoginData.label3.Text = "New Password";
                    Program.ChangeLoginData.label6.Text = "Confirm New Password";
                }
                else
                {
                    Program.ChangeLoginData.label1.Text = "Passwort ändern";
                    Program.ChangeLoginData.label3.Text = "Neues Passwort";
                    Program.ChangeLoginData.label6.Text = "Neues Passwort bestätigen";
                }
                Program.ChangeLoginData.datatxt.Width = 179;
                Program.ChangeLoginData.confirmtxt.Width = 179;
                Program.ChangeLoginData.hidePasswordBtn1.Visible = true;
                Program.ChangeLoginData.hidePasswordBtn2.Visible = true;
                Program.ChangeLoginData.username = false;
                Program.ChangeLoginData.Show();
            }
            else
            {
                Properties.Settings.Default.selection = "a"; // Der String selection für die gespeicherte USB-ID wird auf "a" gesetzt, damit man in der Form Masterkey fortfahren kann.
                Properties.Settings.Default.Save();
                timer1.Stop();
                this.Hide();
                Program.MasterKey.Show();
            }
        }

        /*
        Bei Klick auf den Button "changeUsernameBtn" wird die Form "ChangeLoginData" angezeigt.
        */
        private void changeUsernameBtn_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.englishB)
            {
                Program.ChangeLoginData.label1.Text = "Change Username";
                Program.ChangeLoginData.label3.Text = "New Username";
                Program.ChangeLoginData.label6.Text = "Confirm New Username";
            }
            else
            {
                Program.ChangeLoginData.label1.Text = "Benutzernamen ändern";
                Program.ChangeLoginData.label3.Text = "Neuer Benutzername";
                Program.ChangeLoginData.label6.Text = "Neuen Benutzernamen bestätigen";
            }
            Program.ChangeLoginData.datatxt.Width = 209;
            Program.ChangeLoginData.confirmtxt.Width = 209;
            Program.ChangeLoginData.hidePasswordBtn1.Visible = false;
            Program.ChangeLoginData.hidePasswordBtn2.Visible = false;
            Program.ChangeLoginData.datatxt.isPassword = false;
            Program.ChangeLoginData.confirmtxt.isPassword = false;
            Program.ChangeLoginData.username = true;
            Program.ChangeLoginData.Show();
        }

        /*
        Bei Klick auf den Button "germanBtn" wird die Sprache auf Deutsch geändert und als Einstellung gespeichert.
        */
        private void germanBtn_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.englishB = false;
            Properties.Settings.Default.Save();
            languageBtn.Text = "Deutsch";
            german();
            if (Properties.Settings.Default.DarkMode)
            {
                germanBtn.BackColor = Color.FromArgb(27, 30, 33);
                englishBtn.BackColor = Color.FromArgb(33, 37, 41);
            }
            else
            {
                germanBtn.BackColor = SystemColors.ControlDark;
                englishBtn.BackColor = SystemColors.ControlLight;
            }
        }
        
        /*
        Bei Änderung der Checkbox "darkModeCheck" wird je nach dem Wert Darkmode aktiviert, oder deaktiviert und als Einstellung gespeichert.
        */
        private void darkModeCheck_OnChange(object sender, EventArgs e)
        {
            if (darkModeCheck.Checked)
            {
                Properties.Settings.Default.DarkMode = true;
                Properties.Settings.Default.Save();
                darkMode(Color.FromArgb(33, 37, 41), Color.FromArgb(247, 247, 247), Color.FromArgb(42, 46, 50), Color.FromArgb(33, 37, 41), Color.FromArgb(27, 30, 33));
            }
            else
            {
                Properties.Settings.Default.DarkMode = false;
                Properties.Settings.Default.Save();
                darkMode(Color.White, SystemColors.ControlText, Color.White, SystemColors.ControlLight, SystemColors.ControlDark);
            }
        }

        #endregion


        #endregion
    }
}