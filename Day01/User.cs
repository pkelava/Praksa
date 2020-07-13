using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ForumLibrary
{
    public class User
    {
        private static Dictionary<string, string> allUsers = new Dictionary<string, string>();
        public bool logged_in = false;
        public string UserName = "";
        protected string Password = "";
        public DateTime DateOfCreation { get; set; }


        public User(string name, string pass)
        {
            if (!UserExists(name))
            {
                UserName = name;
                Password = pass;
                DateOfCreation = DateTime.Now;
                allUsers.Add(name, pass);
            }
            else
            {
                Console.WriteLine("Err!");
            }

        }

        public void Log_in(string name, string pass)
        {
            if (logged_in == true)
            {
                Console.WriteLine("Err: Already logged in");
            }
            else if (CredentialsCheck(name, pass))
            {
                logged_in = true;
                Console.WriteLine("Logged in!");
            }
            else
            {
                Console.WriteLine("Err: Username/Password is wrong");
            }
        }

        public void Logg_Out()
        {
            logged_in = false;
        }





        private bool CredentialsCheck(string name, string pass)
        {
            if (UserExists(name))
            {
                return allUsers[name] == pass;
            }
            return false;
        }


        private bool UserExists(string name)
        {
            return allUsers.ContainsKey(name);

        }


    }

}
