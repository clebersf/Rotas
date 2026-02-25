using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vale.Tops.Domain
{
    public class UserInfo
    {
        public UserInfo ()
        {

        }
        public string account { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string UserDisplayName { get; set; }
        public string TelephoneNumber { get; set; }
    }
}
