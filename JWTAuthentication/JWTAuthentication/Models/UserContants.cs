using System.Collections.Generic;

namespace JWTAuthentication.Models
{
    public class UserContants
    {
        public static List<UserModel> Users = new List<UserModel>()
        {
            new UserModel()
            {
                Username = "draken_admin",
                Email = "draken@gmail.com",
                Password = "draken281",
                Surname = "draken",
                GivenName = "ahop",
                Role = "Admin"
            },
            new UserModel()
            {
                Username = "draken_client",
                Email = "draken1@gmail.com",
                Password = "draken281",
                Surname = "draken",
                GivenName = "ahop",
                Role = "Client"
            }
        };
    }
}
