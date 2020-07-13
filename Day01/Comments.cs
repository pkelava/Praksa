using System;
using System.Collections.Generic;
using System.Text;

namespace ForumLibrary
{
    public class Comments : IPosts
    {
        public string PostCreator { get; private set; }
        public DateTime TimeOfCreation { get; private set; }
        public string Context { get; private set; }

        public Comments(User creator, string context)
        {
            PostCreator = creator.UserName;
            Context = context;
        }

    }
}
