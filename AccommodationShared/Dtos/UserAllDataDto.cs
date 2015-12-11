using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Model;

namespace AccommodationShared.Dtos
{
    public class UserAllDataDto
    {
        public User User { get; set; }
        public UserData UserData { get; set; }
        public Address Address { get; set; }
    }
}
