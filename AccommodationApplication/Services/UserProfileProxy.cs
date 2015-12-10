using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AccommodationDataAccess.Model;
using AccommodationShared.Dtos;

namespace AccommodationApplication.Services
{
    public class UserProfileProxy : WebApiProxy
    {
        public UserProfileProxy() : base("UserData", true)
        {
        }

        public async Task<UserBasicDataDto> GetUserAsync(string username)
        {
            if (string.IsNullOrEmpty(username)) return null;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            UserCredentialDto dto = new UserCredentialDto() {Username = username};
            return await Post<UserCredentialDto, UserBasicDataDto>("data", dto);
        }
    }
}
