using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationShared.Dtos
{
    /// <summary>
    /// Dto dla nowego hasła
    /// </summary>
    public class UserNewPasswordDto : UserCredentialDto
    {
        public string NewPassword { get; set; }
    }
}
