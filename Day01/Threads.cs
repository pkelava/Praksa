using System;
using System.Collections.Generic;
using System.Text;

namespace ForumLibrary
{
    public class Threads : IThreadsPosts
    {
        static public List<Threads> AllThreads = new List<Threads>();

        public string Title { get; set; }
        public List<Comments> listComments = new List<Comments>();
        public string PostCreator { get;  set; }
        public DateTime TimeOfCreation { get; set; }
        public string Context { get; set; }

        public Threads(User creator, string title, string context)
        {
            PostCreator = creator.UserName;
            Context = context;
            Title = title;
            TimeOfCreation = DateTime.Now;
            AllThreads.Add(this);
        }        

       public void AddComment(User creator, string context)
        {
            Comments comment = new Comments(creator, context);
            listComments.Add(comment);
        }

        static public void PrintThreads()
        {
            foreach(Threads clanak in AllThreads)
            {
                Console.WriteLine(clanak.Title);
                Console.WriteLine("\t" + clanak.Context);
            }
        }

        public void PrintComments()
        {
            Console.WriteLine(Title);
            foreach (Comments comment in listComments)
            {
                Console.WriteLine(comment.Context);
            }
        }

    }
}
