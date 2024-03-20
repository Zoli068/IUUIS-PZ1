using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PZ1.Model
{

    public enum UserRole {

        [XmlEnum(Name = "Visitor")]
        Visitor,

        [XmlEnum(Name = "Admin")]
        Admin 
    }

    [Serializable]
    public class User
    {

        public String UserName { get; set; }
        public UserRole Role { get; set; }

        public string Password { get; set; }

        public User()
        {

        }
        public User(string userName, UserRole role,string password)
        {
            UserName = userName;
            Role = role;
            Password = password;
        }

        public bool CheckPassword(string password) 
        {
            if (Password.Equals(password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}
