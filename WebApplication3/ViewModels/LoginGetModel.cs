using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3.ViewModels
{
    //ce ii raspunde serverul utilizatorului dupa ce acesta se autentifica
    //ce trebuie sa stie utilizatorul dupa ce se autentifica
    public class LoginGetModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
