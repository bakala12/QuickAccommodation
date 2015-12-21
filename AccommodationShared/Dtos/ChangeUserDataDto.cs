using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationShared.Dtos
{
    /// <summary>
    /// Dto dla zmiany danych użytkownika
    /// </summary>
    public class ChangeUserDataDto : UserBasicDataDto
    {
        public string Username { get; set; }
    }
}
