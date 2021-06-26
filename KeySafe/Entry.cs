using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeySafe
{
    public class Entry
    {

        /*
        Dieser Konstruktor beinhaltet den Namen, Website, Username, Email und Password als String
        und wird später zum Speichern der Daten verwendet.
        */
        public Entry(string Name, string Website, string Username, string Email, string Password)
        {
            this.Name = Name;
            this.Website = Website;
            this.Username = Username;
            this.Email = Email;
            this.Password = Password;
        }

        public string Name { get; set; } //get und set für den Namen
        public string Website { get; set; } //get und set für die Website
        public string Username { get; set; } //get und set für den Username
        public string Email { get; set; } //get und set für die Email
        public string Password { get; set; } //get und set für das Passwort

    }
}
