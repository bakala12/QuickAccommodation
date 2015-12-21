using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationShared.Dtos
{
    /// <summary>
    /// Dto dla danych potrzebnych do logowania
    /// </summary>
    public class UserCredentialDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
