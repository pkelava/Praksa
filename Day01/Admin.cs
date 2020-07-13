using System;
using System.Collections.Generic;
using System.Text;


//Composition over Inheritance
namespace ForumLibrary
{
    public class Admin
    {
        public User User { get; set; }

        public Admin(string name, string pass)
        {
            User = new User(name, pass);
        }
        
        public string UserName()
        {
            return User.UserName;
        }

        public DateTime DateOfCreation()
        {
            return User.DateOfCreation;
        }

        public void Logg_in (string name, string pass)
        {
            User.Log_in(name, pass);
        }

        public void DeleteComment(ref Comments commentToDelete)
        {
            commentToDelete = null;
        }


    }
}
