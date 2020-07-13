using System;
using System.Collections.Generic;
using System.Text;

namespace ForumLibrary
{
    public class Comments : IPosts
    {
        public string PostCreator { get; set; }
        public DateTime TimeOfCreation { get; set; }
        public string Context { get; set; }

        public Comments(User creator, string context)
        {
            TimeOfCreation = DateTime.Now;
            PostCreator = creator.UserName;
            Context = context;
        }

    }
}
